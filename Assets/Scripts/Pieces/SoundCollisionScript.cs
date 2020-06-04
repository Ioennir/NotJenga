using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionScript : MonoBehaviour
{
    private AudioController _controller;

    public void Start()
    {
<<<<<<< HEAD
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

=======
        _controller = FindObjectOfType<AudioController>();
    }

    private void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!_controller) return;
        if (collision.gameObject.CompareTag("Selectable"))
        {
            float volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 50);
            _controller.ChangeAndPlay("JengaSound", sound1 =>
            {
                sound1.volume = volume;
                sound1.transform.position = transform.position;
            });
>>>>>>> edfee153ddea4586b206d637b44b0638dd370335
        }
    }
   
}
