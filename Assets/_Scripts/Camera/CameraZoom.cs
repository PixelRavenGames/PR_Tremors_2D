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

	void Start() {

	}

	void Update() {

	}

	public bool IsAllOnScreen() {
		return false;
	}

	public bool ZoomOut() {
		float newZoom = Mathf.MoveTowards(Camera.main.orthographicSize, maxZoom, Time.deltaTime * smoothFactor);
		if (newZoom > maxZoom) {
			newZoom = maxZoom;
			return true;
		}
		return false;
	}

	public bool ZoomIn() {
		float newZoom = Mathf.MoveTowards(Camera.main.orthographicSize, minZoom, Time.deltaTime * smoothFactor);
		if (newZoom > minZoom) {
			newZoom = minZoom;
			return true;
		}
		return false;
	}

}

[System.Serializable]
public class Margins {
	public float up = 0f;
	public float down = 0f;
	public float left = 0f;
	public float right = 0f;
}
