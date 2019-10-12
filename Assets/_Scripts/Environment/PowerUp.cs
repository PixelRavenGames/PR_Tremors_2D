using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public HumanEffect effect;
	public float duration;

	private void OnCollisionEnter2D(Collision2D collision) {
		HumanEffectManager humanEffects = collision.collider.GetComponent<HumanEffectManager>();
		if (humanEffects) {
			humanEffects.AddEffect(effect, duration);
			Destroy(gameObject);
		}
	}

}
