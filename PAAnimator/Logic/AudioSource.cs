using NAudio.Wave;
using NAudio.Vorbis;
using System;

namespace PAAnimator.Logic
{
    public class AudioSource : IDisposable
    {
        public float Volume
        {
            get => output.Volume;
            set => output.Volume = value;
        }

        public bool IsPlaying => output.PlaybackState == PlaybackState.Playing;

        private VorbisWaveReader reader;
        private WaveChannel32 waveChannel;
        private WasapiOut output = new WasapiOut();

        public AudioSource(string path)
        {
            reader = new VorbisWaveReader(path);
            waveChannel = new WaveChannel32(reader);
            output.Init(waveChannel);
        }

        public void Play()
        {
            output.Play();
        }

        public void Stop()
        {
            output.Stop();
        }

        public void Seek(float position)
        {
            waveChannel.Seek((long)position * waveChannel.WaveFormat.AverageBytesPerSecond, System.IO.SeekOrigin.Begin);
        }

        public float GetPosition()
            => (float)waveChannel.CurrentTime.TotalSeconds;

        public float GetLength() 
            => (float)waveChannel.TotalTime.TotalSeconds;

        public void Dispose()
        {
            output.Dispose();
            waveChannel.Dispose();
            reader.Dispose();
        }
    }
}
