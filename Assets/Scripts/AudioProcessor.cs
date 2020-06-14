using System;
using UnityEngine;
using UnityEngine.Events;

public class AudioProcessor : MonoBehaviour
{
	private AudioSource audioSource;

	private long lastT, nowT, diff, entries, sum;

	public int bufferSize = 1024;
	// fft size
	private int samplingRate;
	// fft sampling frequency

	/* log-frequency averaging controls */
	private readonly int nBand = 12;
	// number of bands

	public float gThresh = 0.1f;
	// sensitivity

	readonly int blipDelayLen = 16;
	int[] blipDelay;

	private int sinceLast = 0;
	// counter to suppress double-beats

	private float framePeriod;

	/* storage space */
	private readonly int colmax = 120;
	float[] spectrum;
	float[] averages;
	float[] acVals;
	float[] onsets;
	float[] scorefun;
	float[] dobeat;
	int now = 0;
	// time index for circular buffer within above

	float[] spec;
	// the spectrum of the previous step

	/* Autocorrelation structure */
	readonly int maxlag = 100;
	// (in frames) largest lag to track
	readonly float decay = 0.997f;
	// smoothing constant for running average
	AutoCorrelator auco;

	private float alph;
	// trade-off constant between tempo deviation penalty and onset strength

	[Header("Events")]
	public OnBeatEventHandler onBeat;
	public OnSpectrumEventHandler onSpectrum;

	//////////////////////////////////
	private long GetCurrentTime()
	{
		long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		return milliseconds;
	}

	private void InitArrays()
	{
		blipDelay = new int[blipDelayLen];
		onsets = new float[colmax];
		scorefun = new float[colmax];
		dobeat = new float[colmax];
		spectrum = new float[bufferSize];
		averages = new float[12];
		acVals = new float[maxlag];
		alph = 100 * gThresh;
	}

	// Use this for initialization
	void Start()
	{
		InitArrays();

		audioSource = GetComponent<AudioSource>();
		samplingRate = audioSource.clip.frequency;

		framePeriod = (float)bufferSize / (float)samplingRate;

		//initialize record of previous spectrum
		spec = new float[nBand];
		for (int i = 0; i < nBand; ++i)
			spec[i] = 100.0f;

		auco = new AutoCorrelator(maxlag, decay, framePeriod, Bandwidth());

		lastT = GetCurrentTime();
	}

	public void TapTempo()
	{
		nowT = GetCurrentTime();
		diff = nowT - lastT;
		lastT = nowT;
		sum += diff;
		entries++;

		int average = (int)(sum / entries);

		Debug.Log("average = " + average);
	}

	// Update is called once per frame
	void Update()
	{
		if (audioSource.isPlaying)
		{
			audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
			ComputeAverages(spectrum);
			onSpectrum.Invoke(averages);

			/* calculate the value of the onset function in this frame */
			float onset = 0;
			for (int i = 0; i < nBand; i++)
			{
				float specVal = (float)Math.Max(-100.0f, 20.0f * (float)Math.Log10(averages[i]) + 160); // dB value of this band
				specVal *= 0.025f;
				float dbInc = specVal - spec[i]; // dB increment since last frame
				spec[i] = specVal; // record this frome to use next time around
				onset += dbInc; // onset function is the sum of dB increments
			}

			onsets[now] = onset;

			/* update autocorrelator and find peak lag = current tempo */
			auco.NewValue(onset);
			// record largest value in (weighted) autocorrelation as it will be the tempo
			float aMax = 0.0f;
			int tempopd = 0;
			//float[] acVals = new float[maxlag];
			for (int i = 0; i < maxlag; ++i)
			{
				float acVal = (float)Math.Sqrt(auco.Autoco(i));
				if (acVal > aMax)
				{
					aMax = acVal;
					tempopd = i;
				}
				// store in array backwards, so it displays right-to-left, in line with traces
				acVals[maxlag - 1 - i] = acVal;
			}

			/* calculate DP-ish function to update the best-score function */
			float smax = -999999;
			int smaxix = 0;
			// weight can be varied dynamically with the mouse
			alph = 100 * gThresh;
			// consider all possible preceding beat times from 0.5 to 2.0 x current tempo period
			for (int i = tempopd / 2; i < Math.Min(colmax, 2 * tempopd); ++i)
			{
				// objective function - this beat's cost + score to last beat + transition penalty
				float score = onset + scorefun[(now - i + colmax) % colmax] - alph * (float)Math.Pow(Math.Log((float)i / (float)tempopd), 2);
				// keep track of the best-scoring predecesor
				if (score > smax)
				{
					smax = score;
					smaxix = i;
				}
			}

			scorefun[now] = smax;
			// keep the smallest value in the score fn window as zero, by subtracing the min val
			float smin = scorefun[0];
			for (int i = 0; i < colmax; ++i)
				if (scorefun[i] < smin)
					smin = scorefun[i];
			for (int i = 0; i < colmax; ++i)
				scorefun[i] -= smin;

			/* find the largest value in the score fn window, to decide if we emit a blip */
			smax = scorefun[0];
			smaxix = 0;
			for (int i = 0; i < colmax; ++i)
			{
				if (scorefun[i] > smax)
				{
					smax = scorefun[i];
					smaxix = i;
				}
			}

			// dobeat array records where we actally place beats
			dobeat[now] = 0;  // default is no beat this frame
			++sinceLast;
			// if current value is largest in the array, probably means we're on a beat
			if (smaxix == now)
			{
				//tapTempo();
				// make sure the most recent beat wasn't too recently
				if (sinceLast > tempopd / 4)
				{
					onBeat.Invoke();
					blipDelay[0] = 1;
					// record that we did actually mark a beat this frame
					dobeat[now] = 1;
					// reset counter of frames since last beat
					sinceLast = 0;
				}
			}

			/* update column index (for ring buffer) */
			if (++now == colmax)
				now = 0;

			//Debug.Log(System.Math.Round(60 / (tempopd * framePeriod)) + " bpm");
			//Debug.Log(System.Math.Round(auco.avgBpm()) + " bpm");
		}
	}

	public float Bandwidth()
	{
		return (2f / (float)bufferSize) * (samplingRate / 2f);
	}

	public int FreqToIndex(int freq)
	{
		// special case: freq is lower than the bandwidth of spectrum[0]
		if (freq < Bandwidth() / 2)
			return 0;
		// special case: freq is within the bandwidth of spectrum[512]
		if (freq > samplingRate / 2 - Bandwidth() / 2)
			return (bufferSize / 2);
		// all other cases
		float fraction = (float)freq / (float)samplingRate;
		int i = (int)Math.Round(bufferSize * fraction);
		//Debug.Log("frequency: " + freq + ", index: " + i);
		return i;
	}

	public void ComputeAverages(float[] data)
	{
		for (int i = 0; i < 12; i++)
		{
			float avg = 0;
			int lowFreq;
			if (i == 0)
				lowFreq = 0;
			else
				lowFreq = (int)((samplingRate / 2) / (float)Math.Pow(2, 12 - i));
			int hiFreq = (int)((samplingRate / 2) / (float)Math.Pow(2, 11 - i));
			int lowBound = FreqToIndex(lowFreq);
			int hiBound = FreqToIndex(hiFreq);
			for (int j = lowBound; j <= hiBound; j++)
			{
				//Debug.Log("lowbound: " + lowBound + ", highbound: " + hiBound);
				avg += data[j];
			}
			// line has been changed since discussion in the comments
			// avg /= (hiBound - lowBound);
			avg /= (hiBound - lowBound + 1);
			averages[i] = avg;
		}
	}

	[System.Serializable]
	public class OnBeatEventHandler : UnityEvent
    {

	}

	[System.Serializable]
	public class OnSpectrumEventHandler : UnityEvent<float[]>
    {

	}

	// class to compute an array of online autocorrelators
	private class AutoCorrelator
	{
		private readonly int del_length;
		private readonly float decay;
		private readonly float[] delays;
		private readonly float[] outputs;
		private int indx;

		private readonly float[] bpms;
		private readonly float[] rweight;
		private readonly float wmidbpm = 120f;
		private readonly float woctavewidth;

		public AutoCorrelator(int len, float alpha, float framePeriod, float bandwidth)
		{
			woctavewidth = bandwidth;
			decay = alpha;
			del_length = len;
			delays = new float[del_length];
			outputs = new float[del_length];
			indx = 0;

			// calculate a log-lag gaussian weighting function, to prefer tempi around 120 bpm
			bpms = new float[del_length];
			rweight = new float[del_length];
			for (int i = 0; i < del_length; ++i)
			{
				bpms[i] = 60.0f / (framePeriod * (float)i);
				//Debug.Log(bpms[i]);
				// weighting is Gaussian on log-BPM axis, centered at wmidbpm, SD = woctavewidth octaves
				rweight[i] = (float)Math.Exp(-0.5f * Math.Pow(Math.Log(bpms[i] / wmidbpm) / Math.Log(2.0f) / woctavewidth, 2.0f));
			}
		}

		public void NewValue(float val)
		{

			delays[indx] = val;

			// update running autocorrelator values
			for (int i = 0; i < del_length; ++i)
			{
				int delix = (indx - i + del_length) % del_length;
				outputs[i] += (1 - decay) * (delays[indx] * delays[delix] - outputs[i]);
			}

			if (++indx == del_length)
				indx = 0;
		}

		// read back the current autocorrelator value at a particular lag
		public float Autoco(int del)
		{
			float blah = rweight[del] * outputs[del];
			return blah;
		}

		public float AverageBPM()
		{
			float sum = 0;
			for (int i = 0; i < bpms.Length; ++i)
			{
				sum += bpms[i];
			}
			return sum / del_length;
		}
	}
}


