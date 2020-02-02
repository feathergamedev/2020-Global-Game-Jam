using System.Collections.Generic;
using Repair.Infrastructures.Core;
using Repair.Infrastructures.Settings;
using UnityEngine;

namespace Repair.Infrastructures.Audios
{
    internal class AudioManager : Singleton<AudioManager>
    {
        private class AudioSourceStatus
        {
            public AudioSource Source;
        }

        private class MusicSourceStatus : AudioSourceStatus
        {
            public MusicType Type = MusicType.Mute;
        }

        private class SoundSourceStatus : AudioSourceStatus
        {
            public SoundType Type = SoundType.Mute;
        }

        private const int MaxSound = 10;
        private MusicSourceStatus musicSource = new MusicSourceStatus();
        private List<SoundSourceStatus> soundSources = new List<SoundSourceStatus>(MaxSound);

        private Dictionary<string, AudioClip> musics = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            TryCreateSource(musicSource, 0.8f);
            for (var i = 0; i < MaxSound; ++i)
            {
                soundSources.Add(new SoundSourceStatus());
            }

            foreach (var soundSource in soundSources)
            {
                TryCreateSource(soundSource, 0.4f);
            }

            LoadAllMusics();
            LoadAllSounds();

            void TryCreateSource(AudioSourceStatus audioSource, float volume)
            {
                if (audioSource.Source == null)
                {
                    audioSource.Source = gameObject.AddComponent<AudioSource>();
                    audioSource.Source.volume = volume;
                }
            }

            void LoadAllMusics()
            {
                var clips = Resources.LoadAll<AudioClip>("Audios/Musics");
                foreach (var clip in clips)
                {
                    musics.Add(clip.name, clip);
                    Debug.Log($"Add music: {clip.name}");
                }
            }

            void LoadAllSounds()
            {
                var clips = Resources.LoadAll<AudioClip>("Audios/Sounds");
                foreach (var clip in clips)
                {
                    sounds.Add(clip.name, clip);
                    Debug.Log($"Add sound: {clip.name}");
                }
            }
        }

        internal void Initialize()
        {
            // NOTE: Dummy
        }

        internal void PlayMusic(MusicType audioName)
        {
            if (audioName == MusicType.Mute)
            {
                musicSource.Type = audioName;
                musicSource.Source.Stop();
                return;
            }

            if (musics.TryGetValue(audioName.ToString(), out AudioClip clip))
            {
                Debug.Log($"Play Music {audioName}!");
                if (musicSource.Type == audioName && musicSource.Source.isPlaying)
                {
                    return;
                }

                musicSource.Type = audioName;
                musicSource.Source.clip = clip;
                musicSource.Source.loop = true;
                musicSource.Source.Play();
            }
            else
            {
                Debug.LogWarning($"Music {audioName} not found!");
            }
        }

        internal void PlaySound(SoundType audioName, int channel)
        {
            if (audioName == SoundType.Mute)
            {
                foreach (var soundSource in soundSources)
                {
                    soundSource.Type = SoundType.Mute;
                    soundSource.Source.Stop();
                }
                return;
            }

            if (sounds.TryGetValue(audioName.ToString(), out AudioClip clip))
            {
                if (channel >= MaxSound)
                {
                    return;
                }

                var soundSource = soundSources[channel];
                if (soundSource == null)
                {
                    return;
                }

                Debug.Log($"Play Sound {audioName}!");
                if (soundSource.Type == audioName && soundSource.Source.isPlaying)
                {
                    soundSource.Source.Stop();
                }

                soundSource.Type = audioName;
                soundSource.Source.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"Sound {audioName} not found!");
            }
        }
    }
}
