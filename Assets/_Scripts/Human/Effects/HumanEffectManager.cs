using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEffectManager : MonoBehaviour {

	public Human target;
	private Dictionary<string, HumanEffectInstance> effects;

	public void Awake() {
		effects = new Dictionary<string, HumanEffectInstance>();

		target.onHurt.AddListener(OnDamageTaken);
	}

	public void AddEffect(HumanEffectInstance instance) {
		string effectID = instance.GetEffect().id;

		if (effects.ContainsKey(effectID)) {
			effects[effectID].AddDuration(instance.GetDuration());
		} else {
			effects.Add(effectID, instance);
			instance.OnStartEffect(target);
		}
	}

	public void AddEffect(HumanEffect effect, float duration) {
		AddEffect(new HumanEffectInstance(effect, duration));
	}

	public void RemoveEffect(string id) {
		effects[id].OnEndEffect(target);
		effects.Remove(id);
	}

	public void OnDamageTaken(float damage) {
		foreach(HumanEffectInstance effect in effects.Values) {
			effect.OnDamageTaken(target, damage);
		}
	}

	private void Update() {

		List<string> toRemove = new List<string>();

		foreach(HumanEffectInstance effect in effects.Values) {
			effect.Update();

			if (effect.IsExpired) {
				toRemove.Add(effect.GetEffect().id);
			}
		}

		foreach (string id in toRemove) {
			RemoveEffect(id);
		}
	}

}
