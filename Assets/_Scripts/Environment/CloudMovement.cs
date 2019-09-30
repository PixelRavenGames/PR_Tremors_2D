using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {

	public bool randomizeSpeed = false;
	[ConditionalHide("randomizeSpeed", true, true)]
	public float speed = 1f;
	[ConditionalHide("randomizeSpeed", true, false)]
	public float minSpeed = 1f;
	[ConditionalHide("randomizeSpeed", true, false)]
	public float maxSpeed = 1f;

	private Vector3 startPos;
	private Vector3 endPos;

	void Start() {
		startPos = transform.position;
		endPos = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
	}

	void Update() {

	}

	private void OnDrawGizmos() {
		Gizmos.DrawLine(transform.position, new Vector3(-transform.position.x, transform.position.y, transform.position.z));
	}
}
