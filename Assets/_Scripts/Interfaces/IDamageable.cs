using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

	void Damage(Vector2 damageSource, float damageMultiplier = 1);
	void Damage(float rawDamage);

}
