using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private enum UIAudioSourceType
    {
        Loop,
        OneShot
    }

    private Dictionary<string, AudioClip> audioDict;

    private AudioSource loopSoundSrc;
    private AudioSource oneShotSoundSrc;

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

    private void PlayAudioClip(string clipName, UIAudioSourceType targetSrc)
    {
        if (targetSrc == UIAudioSourceType.Loop)
        {
            if (audioDict == null || !audioDict.ContainsKey(clipName))
                return;

            if (loopSoundSrc == null)
            {
                loopSoundSrc = gameObject.AddComponent<AudioSource>();
                loopSoundSrc.loop = true;
                oneShotSoundSrc.playOnAwake = false;
            }

            loopSoundSrc.clip = audioDict[clipName];
            loopSoundSrc.Play();
        }
        else if (targetSrc == UIAudioSourceType.OneShot)
        {
            if (audioDict == null || !audioDict.ContainsKey(clipName))
                return;

            if (oneShotSoundSrc == null)
            {
                oneShotSoundSrc = gameObject.AddComponent<AudioSource>();
                oneShotSoundSrc.loop = false;
                oneShotSoundSrc.playOnAwake = false;
            }

            oneShotSoundSrc.clip = audioDict[clipName];
            oneShotSoundSrc.Play();
        }
    }

    private void StopUIAudioClip(UIAudioSourceType targetSrc)
    {
        if (targetSrc == UIAudioSourceType.Loop)
        {
            if (loopSoundSrc != null)
                loopSoundSrc.Stop();
        }
    }

    public void PlayLoopSound(string soundName)
    {
        PlayAudioClip(soundName, UIAudioSourceType.Loop);
    }

    public void StopLoopSound(string soundName)
    {
        if (loopSoundSrc != null)
            loopSoundSrc.Stop();
    }

    public void PlayOneShotSound(string soundName)
    {
        PlayAudioClip(soundName, UIAudioSourceType.OneShot);
    }
}
