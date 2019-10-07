using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEffectInstance {

	private HumanEffect effect;
	private float duration = 0;

	public bool IsExpired {
		get {
			return duration <= 0;
		}
	}

	public HumanEffectInstance(HumanEffect effect, float duration) {
		this.effect = effect;
		this.duration = duration;
	}

	public void Update() {
		duration -= Time.deltaTime;
	}

	public void OnStartEffect(Human human) {
		effect.OnStartEffect(human);
	}

	public void OnEndEffect(Human human) {
		effect.OnEndEffect(human);
	}

	public void OnDamageTaken(Human human, float damage) {
		effect.OnDamageTaken(human, damage);
	}

	public void AddDuration(float duration) {
		this.duration += duration;
	}

	public float GetDuration() {
		return duration;
	}

	public HumanEffect GetEffect() {
		return effect;
	}

}
