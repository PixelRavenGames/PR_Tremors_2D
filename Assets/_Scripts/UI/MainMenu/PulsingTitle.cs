using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PulsingTitle : MonoBehaviour {

	public AnimationCurve scale;
	public float timeScale = 1;

	private RectTransform rectTransform;

	void Start() {
		rectTransform = GetComponent<RectTransform>();
	}

	void Update() {

		float time = (Time.time * timeScale) % 1;

		rectTransform.localScale = new Vector2(scale.Evaluate(time), scale.Evaluate(time));
	}
}
