using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioSourceType
{
    Weapon,
    Movement,
    Effect
}

public class AudioManager : Singleton<AudioManager>
{

    private Dictionary<string, AudioClip> audioDict;

    private AudioSource movementSound1;
    private AudioSource movementSound2;
    private AudioSource weaponSound;
    private AudioSource soundEffectSrc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(List<AudioClip> audioClips)
    {
        if (audioDict == null)
        {
            audioDict = new Dictionary<string, AudioClip>();

            // Add UI audio clips to manager
            foreach (var clip in audioClips)
            {
                audioDict.Add(clip.name, clip);
            }
        }
    }

    private void PlayAudioClip(string clipName, AudioSourceType targetSrc)
    {
        if (targetSrc == AudioSourceType.Movement)
        {
            if (audioDict == null || !audioDict.ContainsKey(clipName))
                return;

            if (movementSound1 == null)
            {
                movementSound1 = gameObject.AddComponent<AudioSource>();
                movementSound1.loop = false;
                movementSound1.playOnAwake = false;
            }

            if (clipName != "walk")
            {
                if (movementSound2 == null)
                {
                    movementSound2 = gameObject.AddComponent<AudioSource>();
                    movementSound2.loop = false;
                    movementSound2.playOnAwake = false;
                }

                movementSound2.clip = audioDict[clipName];
                movementSound2.Play();
            }
            else
            {
                movementSound1.clip = audioDict[clipName];
                movementSound1.Play();
            }

        }
        else if (targetSrc == AudioSourceType.Weapon)
        {
            if (audioDict == null || !audioDict.ContainsKey(clipName))
                return;

            if (weaponSound == null)
            {
                weaponSound = gameObject.AddComponent<AudioSource>();
                weaponSound.loop = false;
                weaponSound.playOnAwake = false;
            }

            if (weaponSound.isPlaying) 
                return;

            weaponSound.clip = audioDict[clipName];
            weaponSound.Play();
        }
        else if (targetSrc == AudioSourceType.Effect)
        {
            if (audioDict == null || !audioDict.ContainsKey(clipName))
                return;

            if (soundEffectSrc == null)
            {
                soundEffectSrc = gameObject.AddComponent<AudioSource>();
                soundEffectSrc.loop = false;
                soundEffectSrc.playOnAwake = false;
            }

            if (soundEffectSrc.isPlaying)
                return;

            soundEffectSrc.clip = audioDict[clipName];
            soundEffectSrc.Play();
        }
    }

    public void PlayOneShotSound(string soundName, AudioSourceType type)
    {
        PlayAudioClip(soundName, type);
    }
}
