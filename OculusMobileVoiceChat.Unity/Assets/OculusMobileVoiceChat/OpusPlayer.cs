using System;
using UnityEngine;
using UnityOpus;

namespace Nikaera.OculusMobileVoiceChat
{
    [RequireComponent(typeof(AudioSource))]
    public class OpusPlayer : MonoBehaviour
    {
        const NumChannels channels = NumChannels.Mono;
        const SamplingFrequency frequency = SamplingFrequency.Frequency_48000;
        const int audioClipLength = 1024 * 6;
        int head = 0;
        float[] audioClipData;

        MicrophoneEncoder encoder;
        Decoder decoder;
        readonly float[] pcmBuffer = new float[Decoder.maximumPacketDuration * (int)channels];
        AudioSource audioSource;

        void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = AudioClip.Create("OpusPlayer", audioClipLength, (int)channels, (int)frequency, false);
            audioSource.loop = true;

            decoder = new Decoder(
                SamplingFrequency.Frequency_48000,
                NumChannels.Mono);
        }

        void OnDisable()
        {
            decoder.Dispose();
            decoder = null;

            audioSource.Stop();
        }

        public void PlayAudio(OpusData opusData)
        {
            if (opusData == null) return;

            var pcmLength = decoder.Decode(opusData.Bytes, opusData.EncodedLength, pcmBuffer);

            if (audioClipData == null || audioClipData.Length != pcmLength)
            {
                audioClipData = new float[pcmLength];
            }
            Array.Copy(pcmBuffer, audioClipData, pcmLength);
            audioSource.clip.SetData(audioClipData, head);
            head += pcmLength;

            if (!audioSource.isPlaying && head > audioClipLength / 2)
            {
                audioSource.Play();
            }
            head %= audioClipLength;
        }

        public float GetRMS()
        {
            if (audioClipData == null)
                return 0;

            float sum = 0.0f;
            foreach (var sample in audioClipData)
            {
                sum += sample * sample;
            }
            return Mathf.Sqrt(sum / audioClipData.Length);
        }
    }
}
