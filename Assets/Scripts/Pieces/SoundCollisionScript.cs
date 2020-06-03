using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionScript : MonoBehaviour
{
    private Sound sound;
    private AudioController controller;
    //private Sound soundcontroller;
    public void Start()
    {
        controller = GameObject.Find("AudioController").GetComponent<AudioController>();
        sound = this.GetComponent<Sound>();
        sound.Init();
    }
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            float volume = Mathf.Clamp01(other.relativeVelocity.magnitude / 20);
            sound.volume = volume;
            sound.UpdateSource();
            sound.Play();

        }
    }
   
}
