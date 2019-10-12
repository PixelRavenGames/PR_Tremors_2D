using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLaunchingScript : MonoBehaviour, IDamageable {

	public float explosionForce = 8;
	public float damageThreshold = 0.75f;
	Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void Damage(Vector2 damageSource, float damageMultiplier = 1) {
		float distance = Vector2.Distance(transform.position, damageSource);

		float damage = 1 - Mathf.Pow(distance / damageMultiplier, 2);

		Damage(damage);
	}

	public void Damage(float rawDamage) {
		if (rawDamage > damageThreshold) rb.velocity = Vector2.up * explosionForce;
	}
}
