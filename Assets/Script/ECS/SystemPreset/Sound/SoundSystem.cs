using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Donuts
{
    public class SpawnSoundComponent : IComponent
    {
        public Entity owner { get; set; }
        public AudioClip spawnSound;
    }



    public partial class GameEvent
    {
        public Action<float> onChangeSoundFXVolume;
        public Action<float> onChangeBGMVolume;
        public Action<float> onChangeAmbientSoundVolume;
    }

    public class SoundFXPool
    {
        public float lastPlayedTime = 0;
        public List<AudioSource> sources = new List<AudioSource>();
    }

    public class SoundSystem : AGameSystem
    {
        private AudioSource bgmSound;
        private AudioSource ambientSound;
        private AudioSource soundFXModel;

        private Dictionary<int, SoundFXPool> soundFXPools = new Dictionary<int, SoundFXPool>();

        private float soundFXVolume = 1.0f;
        private float ambientSoundVolume = 1.0f;
        private float bgmSoundVolume = 1.0f;
        private float minSoundFrameInterval = 0.1f;

        public SoundSystem(AudioSource _bgm, AudioSource _ambient, AudioSource _soundFX)
        {
            bgmSound = _bgm;
            ambientSound = _ambient;
            soundFXModel = _soundFX;

        }

        public override void SetupEvents()
        {

            gameEvent.onSpawnedEntity += OnSpawnedEntity;
            gameEvent.onChangeSoundFXVolume += OnChangedSoundFXVolume;
            gameEvent.onChangeBGMVolume += OnChangedBGMVolume;
            gameEvent.onChangeAmbientSoundVolume += OnChangeAmbientSoundVolume;
        }

        private void OnSpawnedEntity(Entity entity)
        {
            SpawnSoundComponent soundComponent = entity.GetComponent<SpawnSoundComponent>();
            if (soundComponent != null)
            {
                PlaySoundOneShot(soundComponent.spawnSound, entity.transform.position);
            }
        }

        private void PlaySoundOneShot(AudioClip clip, Vector3 position)
        {
            int hash = clip.GetHashCode();
            if (soundFXPools.ContainsKey(hash))
            {
                SoundFXPool pool = soundFXPools[hash];
                if ((Time.timeSinceLevelLoad - pool.lastPlayedTime) < minSoundFrameInterval)
                {
                    return;
                }
                pool.lastPlayedTime = Time.timeSinceLevelLoad;
                int count = pool.sources.Count;
                for (int i = 0; i < count; i++)
                {
                    if (pool.sources[i].gameObject.activeSelf == false)
                    {
                        SetAudioSourceVolulme(pool.sources[i], soundFXVolume);
                        pool.sources[i].gameObject.SetActive(true);
                        pool.sources[i].transform.position = position;
                        pool.sources[i].PlayOneShot(clip);
                        return;
                    }
                }
                AudioSource source = GameObject.Instantiate<AudioSource>(soundFXModel);
                source.transform.position = position;
                SetAudioSourceVolulme(source, soundFXVolume);
                source.PlayOneShot(clip);

                pool.sources.Add(source);
                return;
            }

            SoundFXPool newPool = new SoundFXPool();
            AudioSource newSource = GameObject.Instantiate<AudioSource>(soundFXModel);
            SetAudioSourceVolulme(newSource, soundFXVolume);
            newPool.sources.Add(newSource);
            newPool.lastPlayedTime = Time.timeSinceLevelLoad;
            soundFXPools.Add(hash, newPool);

        }

        private void SetAudioSourceVolulme(AudioSource source, float volume)
        {
            source.volume = volume;//min max and others
        }

        private void OnChangedBGMVolume(float volume)
        {
            bgmSoundVolume = volume;
            SetAudioSourceVolulme(bgmSound, bgmSoundVolume);
        }

        private void OnChangedSoundFXVolume(float volume)
        {
            soundFXVolume = volume;
        }

        private void OnChangeAmbientSoundVolume(float volume)
        {
            ambientSoundVolume = volume;
            SetAudioSourceVolulme(ambientSound, ambientSoundVolume);
        }
    }
}