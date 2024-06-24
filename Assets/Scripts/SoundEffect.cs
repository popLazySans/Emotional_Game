using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private AudioSource sound;
    [SerializeField] private AudioClip[] audioClip;
    private void Start()
    {
        sound = gameObject.GetComponent<AudioSource>();
    }
    public void playSound(int action)
    {
        sound.clip = audioClip[action];
        sound.Play();
    }
}
