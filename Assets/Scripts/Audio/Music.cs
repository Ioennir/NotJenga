using UnityEngine;

public class Music : MonoBehaviour
{
	#region Private Variables
    private Sound _sound;
	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Start()
    {
	    _sound = GetComponent<Sound>();
	    _sound.Init();
    }

	private void Update()
    {
	    if (!_sound.Playing)
	    {
		    _sound.loop = true;
		    _sound.UpdateSource();
		    _sound.Play();
	    }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}
