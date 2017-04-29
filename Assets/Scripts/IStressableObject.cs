using System;

/// <summary>
/// Interface of a stressable object.
/// </summary>
public interface IStressableObject
{
	/// <summary>
	/// Adds the stress.
	/// </summary>
	/// <param name="stress">Stress to add.</param>
	void AddStress (float stress);
}
