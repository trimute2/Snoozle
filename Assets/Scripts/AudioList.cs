using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    public List<AudioClip> soundFXs;

    private void Start()
    {
        AudioManager.Instance.Initialize(soundFXs);
    }
}
