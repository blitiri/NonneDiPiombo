using System;

/// <summary>
/// Interface of a damageable object.
/// </summary>
public interface IDamageableObject
{
	/// <summary>
	/// Adds the damage.
	/// </summary>
	/// <param name="damage">Damage to add.</param>
	/// <param name="damager">Who has produced damage.</param>
	void AddDamage (int damage, string damager);
}
