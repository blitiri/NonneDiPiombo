using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio manager.
/// </summary>
public class AudioManager : MonoBehaviour
{
	/// <summary>
	/// The audio source.
	/// </summary>
	public AudioSource audioSource;

	/// <summary>
	/// Start the script.
	/// </summary>
	void Start ()
	{
		if (audioSource != null) {
			audioSource.volume = Configuration.instance.GetSoundVolume ();
		}
	}
}
