﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public delegate void AudioAction(Sound sound);
	
	#region Private Variables

	[SerializeField] private Sound[] sounds;

	private readonly Dictionary<string, Sound> _soundsDict = new Dictionary<string, Sound>();
	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Awake()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
	        sounds[i].Init();
	        _soundsDict.Add(sounds[i].Name, sounds[i]);
        }
        
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Tries to play a sound, won't play the sound if it's already playing (so there is no reset).
    /// If the sound was found, it will return it.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The sound if it was found.</returns>
    public Sound Play(string name)
    {
	    Sound value;
	    bool success = _soundsDict.TryGetValue(name, out value);
	    if (!success) return null;
	    value.UpdateSource();
	    if (value.Source.isPlaying) return value;
	    value.Play();
	    return value;
    }

    /// <summary>
    /// Will force the sound to play even though it's already playing. If it's playing it will reset.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sound ForcePlay(string name)
    {
	    Sound value;
	    bool success = _soundsDict.TryGetValue(name, out value);
	    if (!success) return null;
	    value.UpdateSource();
	    if (value.Source.isPlaying)
	    {
		    value.Source.time = 0.0f;
	    }
	    value.Play();
	    return value;
    }

    /// <summary>
    /// Stops a sound
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sound Stop(string name)
    {
	    Sound value;
	    bool success = _soundsDict.TryGetValue(name, out value);
	    if (!success) return null;
	    value.Stop();
	    return value;
    } 

    /// <summary>
    /// Custom action.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Sound CustomAction(string name, AudioAction action)
    {
	    Sound value;
	    bool success = _soundsDict.TryGetValue(name, out value);
	    if (!success) return null;
	    action(value);
	    return value;
    }

    /// <summary>
    /// Calls the callback <param name="action"></param> before playing the sound, so you can change the
    /// attributes of a sound if you want.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Sound ChangeAndPlay(string name, AudioAction action)
    {
	    Sound value;
	    bool success = _soundsDict.TryGetValue(name, out value);
	    if (!success) return null;
	    action(value);
	    print(value.volume);
		value.UpdateSource();
		if (value.Source.isPlaying) return value;
		value.Play();
	    return value;
    }

    /// <summary>
    /// Removes a sound
    /// </summary>
    /// <param name="soundName"></param>
    /// <returns></returns>
    public bool DeleteSound(string soundName)
    {
	    for (int i = 0; i < sounds.Length; ++i)
	    {
		    if (soundName != sounds[i].Name) continue;
		    sounds[i] = null;
	    }
	    return _soundsDict.Remove(soundName);
    }
    
    #endregion

    #region Private Methods

    #endregion
}