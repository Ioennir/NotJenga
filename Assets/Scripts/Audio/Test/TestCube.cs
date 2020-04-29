using UnityEngine;

/// <summary>
/// Example on how the controller can work
/// </summary>
public class TestCube : MonoBehaviour
{
	#region Private Variables

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

	private AudioController controller;

	private float increase = 0.0f;
	
    private void Start()
    {
	    // Get the controller
	    controller = FindObjectOfType<AudioController>();
    }

    /// <summary>
    /// Increase the volume until 0.5f (mid level)
    /// </summary>
	private void Update()
	{
		increase += Time.deltaTime / 10;
		if (increase > 0.5f) return;
		print(controller.ChangeAndPlay("sound_test_01", sound => sound.volume = increase));
	}

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}
