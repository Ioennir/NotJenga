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
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Selectable"))
        {
            float volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 20);
            sound.volume = volume;
            controller.Play("JengaSound");
        }
    }
    
}
