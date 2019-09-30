using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {

	public bool randomizeStart = true;
	public bool randomizeSpeed = false;
	[ConditionalHide("randomizeSpeed", true, true)]
	public float speed = 1f;
	[ConditionalHide("randomizeSpeed", true, false)]
	public float minSpeed = 1f;
	[ConditionalHide("randomizeSpeed", true, false)]
	public float maxSpeed = 1f;

	private Vector2 startPos;
	private Vector2 endPos;

	private float timer = 0f;

	void Start() {
		startPos = transform.position;
		endPos = new Vector2(-transform.position.x, transform.position.y);

		speed = Random.Range(minSpeed, maxSpeed);
		if (randomizeStart) timer = Random.Range(0, 1);
	}

	void Update() {

		timer += Time.deltaTime * speed;

		transform.position = Vector2.Lerp(startPos, endPos, timer);

		if (timer > 1) {
			timer = 0;
			if (randomizeSpeed) {
				speed = Random.Range(minSpeed, maxSpeed);
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawLine(transform.position, new Vector3(-transform.position.x, transform.position.y, transform.position.z));
	}
}
