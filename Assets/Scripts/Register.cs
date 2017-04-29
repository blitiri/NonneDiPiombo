using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Register of <see cref="MonoBehaviour"/>. Key is <see cref="GameObject.GetInstanceID"/>
/// </summary>
public class Register
{
	/// <summary>
	/// The register instance (singleton)
	/// </summary>
	public static Register instance = null;
	/// <summary>
	/// The register.
	/// </summary>
	private Hashtable register = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="Register"/> class.
	/// </summary>
	public Register ()
	{
		instance = this;
		register = new Hashtable ();
	}

	/// <summary>
	/// Register a <see cref="MonoBehaviour"/> script associated with a specific key (<see cref="GameObject.GetInstanceID"/>).
	/// </summary>
	/// <param name="key"><see cref="MonoBehaviour"/> key (<see cref="GameObject.GetInstanceID"/>).</param>
	/// <param name="script"><see cref="MonoBehaviour"/> script.</param>
	public void Put (int key, MonoBehaviour script)
	{
		register.Add (key, script);
	}

	/// <summary>
	/// Get the <see cref="MonoBehaviour"/> script associated with key (<see cref="GameObject.GetInstanceID"/>).
	/// </summary>
	/// <param name="key"><see cref="MonoBehaviour"/> key (<see cref="GameObject.GetInstanceID"/>).</param>
	public MonoBehaviour Get (int key)
	{
		return register [key];
	}

	/// <summary>
	/// Remove the <see cref="MonoBehaviour"/> script associated with key (<see cref="GameObject.GetInstanceID"/>).
	/// </summary>
	/// <param name="key"><see cref="MonoBehaviour"/> key (<see cref="GameObject.GetInstanceID"/>).</param>
	public void Remove (int key)
	{
		register.Remove (key);
	}
}

