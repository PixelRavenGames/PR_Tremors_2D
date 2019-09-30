using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunIndicator : MonoBehaviour {

	public SpriteMask mask;
	public SpriteRenderer forground;
	public SpriteRenderer background;

	public float speed = 1f;

	public bool shouldShow = false;

	void Update() {

		if (shouldShow != mask.enabled) {
			mask.enabled = shouldShow;
		}

		forground.transform.localPosition += (Vector3) Vector2.right * Time.deltaTime * speed;
		if (forground.transform.localPosition.x > 0.25f) forground.transform.localPosition = Vector2.zero;

		background.transform.localPosition += (Vector3) Vector2.left * Time.deltaTime * speed;
		if (background.transform.localPosition.x < -0.25f) background.transform.localPosition = Vector2.zero;
	}
}
