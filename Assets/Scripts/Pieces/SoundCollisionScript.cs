using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionScript : MonoBehaviour
{
    private AudioController _controller;

    public void Start()
    {
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
            float volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10);
            _controller.ChangeAndPlay("JengaSound", sound1 =>
            {
                sound1.volume = volume;
                sound1.transform.position = transform.position;
            });
        }
    }
   
}
