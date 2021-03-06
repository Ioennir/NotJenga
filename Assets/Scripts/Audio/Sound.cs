﻿using UnityEngine;

/// <summary>
/// Sound represents a sound emitter controller inside a gameObject
/// This means that you won't have to deal directly with AudioSource (the scripts handle that for you)
/// and there is a single class to deal with the sounds (The AudioController)
///
/// How to use:
/// 1. Create an AudioController gameObject.
/// 2. Create a Sound component in the desired GameObject (in which you want to emit sounds)
/// 3. Add it to the AudioController
/// 4. When you want to play a sound (or do another action) you must get the AudioController, pass the name of the sound
///    and play it. Or you can use the sound reference directly if you already have it.
/// 5. Example: FindObject <AudioController>().Play("cool_sound_01"); (There are more functions in the AudioController, you can read them to see how they work)
/// 6. There is no need to use the AudioController when you have the Sound in your gameObject and you want to play it in your gameObject;
/// 	but sometimes you need to play a general sound without having the specified Sound reference.
/// 7. Example: if (!sound.Source.isPlaying) sound.Play();
/// 8. If you really don't want bugs and things to work well without knowing how to use Unity Sound API, I recommend using the methods in AudioController instead.
/// </summary>
public class Sound : MonoBehaviour
{
	#region Private Variables

	private static int nOfSoundsPlaying = 0;
	private bool _playing = false;
	
	[SerializeField] private string name;
	
	[SerializeField]
	private AudioClip audioClip;
	
	private AudioSource _source;
	
	#endregion

	#region Public Variables

	public float volume = 50.0f;
	public bool loop = false;
	public float pitch = 1f;

	/// <summary>
	/// Min Distance To Play maximum volume
	/// </summary>
	public float minDistance = 15;
	
	// MaxDistance ( I don't think this changes much )
	public float maxDistance = 30;
	#endregion

	#region Properties

	public string Name => name;

	public AudioClip AudioClip => audioClip;

	/// <summary>
	/// Unity AudioSource (here you can change more advance things)
	/// If you wanna change the loop, the volume, the minDistance or the pitch you can change it here before playing.
	/// </summary>
	public AudioSource Source => _source;

	public bool Playing => _playing;
	
	#endregion

	#region MonoBehaviour

    private void Start()
    {
        
    }

	private void Update()
    {
	    if (_playing && !Source.isPlaying)
	    {
		    nOfSoundsPlaying--;
		    _playing = false;
	    }
    }

    #endregion

    #region Public Methods

    public void Init()
    {
	    _source = gameObject.GetComponent<AudioSource>();
	    if (!_source)
		    // Create an AudioSource
			_source = gameObject.AddComponent<AudioSource>();
	    // Set 3D sound
	    _source.spatialBlend = 1;
	    // Doppler level
	    _source.dopplerLevel = 0;
	    // Spread in the sound (150 is the mid level so it sounds good)
	    _source.spread = 150;
	    _source.minDistance = minDistance;
	    _source.maxDistance= maxDistance;
	    UpdateSource();
    }

    
    /// <summary>
    /// Update source applies audioClip, minDistance, maxDistance volume, loop and pitch changes.
    /// </summary>
    public void UpdateSource()
    {
	    if (!_source.clip)
			_source.clip = audioClip;
	    _source.volume = volume;
	    _source.loop = loop;
	    _source.pitch = pitch;
	    _source.minDistance = minDistance;
	    _source.maxDistance = maxDistance;
    }

    /// <summary>
    /// Plays the sound
    /// </summary>
    public void Play()
    {
	    _playing = true;
	    nOfSoundsPlaying++;
	    _source.Play();
    }

    public void Stop()
    {
	    _source.Stop();
    }
    
    #endregion

    #region Private Methods

    #endregion
}
