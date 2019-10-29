using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLaunchingScript : MonoBehaviour, IDamageable {

	[Header("Settings")]
	public float explosionForce = 8;
	public float damageThreshold = 0.75f;
	public int hitsToDestroy = int.MaxValue;
	[Header("Particles")]
	public bool particleEffectOnHit = false;
	[ConditionalHide("particleEffectOnHit", true, false)]
	public PropParticlesContainer particles;
	[Header("Sprite")]
	public bool damageSprite = false;
	[ConditionalHide("damageSprite", true, false)]
	public DestroyablePropSprite sprites;

	Rigidbody2D rb;
	SpriteRenderer sr;

	private int hits = 0;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	public void Damage(Vector2 damageSource, float damageMultiplier = 1) {
		float distance = Vector2.Distance(transform.position, damageSource);

		float damage = 1 - Mathf.Pow(distance / damageMultiplier, 2);

		Damage(damage);
	}

	public void Damage(float rawDamage) {
		if (rawDamage > damageThreshold) {
			rb.velocity = Vector2.up * explosionForce;
			if (particleEffectOnHit) particles.PlayParticles(transform);

			if (damageSprite) {
				sr.sprite = sprites.GetNextSprite();
			}

			hits++;
			if (hits > hitsToDestroy) {
				Destroy(gameObject);
			}
		}

	}
}

[System.Serializable]
public class PropParticlesContainer {
	public bool oneTimeOnly = true;
	public PropParticles[] particles;

	private bool consumed = false;

	public void PlayParticles(Transform transform) {
		if (!consumed) {
			foreach (PropParticles p in particles) {
				p.PlayParticles(transform);
			}

			if (oneTimeOnly) consumed = true;
		}
	}

}

[System.Serializable]
public class PropParticles {
	public GameObject particles;
	public Vector2 offset;
	public bool shouldParent = false;

	public void PlayParticles(Transform transform) {

		Vector2 newPosition = transform.localPosition + (transform.InverseTransformVector(offset));

		GameObject particlesObj = null;
		if (shouldParent) {
			particlesObj = GameObject.Instantiate(particles, newPosition, Quaternion.identity, transform);
		} else {
			particlesObj = GameObject.Instantiate(particles, newPosition, Quaternion.identity);
		}
		particlesObj.transform.localScale = Vector3.one * 0.3f;
	}
}

[System.Serializable]
public class DestroyablePropSprite {

	[Range(0, 1)]
	public float chanceOfBreaking = 1;

	public Sprite[] sprites;

	private int currSprite = 0;
	public Sprite GetNextSprite() {
		if (Random.Range(0f, 1f) < chanceOfBreaking) {
			currSprite = Mathf.Min(sprites.Length - 1, currSprite + 1);
		}

		return sprites[currSprite];

	}

}
