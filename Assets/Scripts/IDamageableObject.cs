using System;

/// <summary>
/// Interface of a damageable object.
/// </summary>
public interface IDamageableObject
{
	/// <summary>
	/// Adds the damage.
	/// </summary>
	/// <param name="demage">Demage to add.</param>
	void AddDamage (int demage);
}
