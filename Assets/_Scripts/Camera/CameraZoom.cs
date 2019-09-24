using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	[Header("Settings")]
	[SerializeField]
	private float minZoom = 1f;
	[SerializeField]
	private float maxZoom = 1f;
	[SerializeField]
	private float smoothFactor = 1f;
	[SerializeField]
	private bool differentMargins = false;
	[ConditionalHide("differentMargins", true, true)]
	[SerializeField]
	private float screenMargin = 0f;
	[ConditionalHide("differentMargins", true, false)]
	[SerializeField]
	private Margins screenMargins;


	void Update() {
		if (!IsAllOnScreen()) {
			ZoomOut();
		} else if (IsAllOnScreen(2f)) {
			ZoomIn();
		}
	}

	public bool IsAllOnScreen(float marginMultiplier = 1) {
		bool ret = true;

		foreach (Vector2 position in PlayerManager.instance.GetPlayerPositions()) {

			Vector2 screenPos = Camera.main.WorldToViewportPoint(position);

			if (screenPos.x < ((differentMargins ? screenMargins.left : screenMargin) * marginMultiplier)) ret = false;
			if (screenPos.x > 1 - ((differentMargins ? screenMargins.right : screenMargin) * marginMultiplier)) ret = false;
			if (screenPos.y < ((differentMargins ? screenMargins.down : screenMargin) * marginMultiplier)) ret = false;
			if (screenPos.y > 1 - ((differentMargins ? screenMargins.up : screenMargin) * marginMultiplier)) ret = false;

		}

		return ret;
	}

	public bool ZoomOut() {
		bool ret = false;
		float newZoom = Mathf.MoveTowards(Camera.main.orthographicSize, maxZoom, Time.deltaTime * smoothFactor);
		if (newZoom > maxZoom) {
			newZoom = maxZoom;
			ret = true;
		}
		Camera.main.orthographicSize = newZoom;
		return ret;
	}

	public bool ZoomIn() {
		bool ret = false;
		float newZoom = Mathf.MoveTowards(Camera.main.orthographicSize, minZoom, Time.deltaTime * smoothFactor);
		if (newZoom < minZoom) {
			newZoom = minZoom;
			ret = true;
		}
		Camera.main.orthographicSize = newZoom;
		return ret;
	}

}

[System.Serializable]
public class Margins {
	public float up = 0f;
	public float down = 0f;
	public float left = 0f;
	public float right = 0f;
}
