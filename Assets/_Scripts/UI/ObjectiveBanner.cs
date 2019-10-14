using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBanner : MonoBehaviour {

	public float duration = 5f;

	public AnimationCurve shakeOverTime;
	public AnimationCurve rotationOverTime;
	public AnimationCurve gravityOverTime;

	private Vector3 mainPosition;

	private float time = 0;

	void Start() {
		Destroy(gameObject, duration);
		mainPosition = transform.position;
	}

	void Update() {
		time += Time.deltaTime;

		transform.rotation = Quaternion.Euler(0, 0, rotationOverTime.Evaluate(time));
		mainPosition = (Vector2) mainPosition + Physics2D.gravity * Vector2.up * gravityOverTime.Evaluate(time);
		transform.position = (Vector2)mainPosition + (Random.insideUnitCircle * shakeOverTime.Evaluate(time));

	}
}
