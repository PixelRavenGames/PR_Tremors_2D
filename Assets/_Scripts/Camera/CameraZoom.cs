using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

	private void OnDrawGizmos() {

		Camera cam = GetComponent<Camera>();

		Vector2 outPointTL = cam.ViewportToWorldPoint(new Vector2(differentMargins ? screenMargins.left : screenMargin, differentMargins ? screenMargins.up : screenMargin));
		Vector2 outPointBR = cam.ViewportToWorldPoint(new Vector2(1 - (differentMargins ? screenMargins.right : screenMargin), 1 - (differentMargins ? screenMargins.down : screenMargin)));

		Vector2 inPointTL = cam.ViewportToWorldPoint(new Vector2(2 * (differentMargins ? screenMargins.left : screenMargin), 2 * (differentMargins ? screenMargins.up : screenMargin)));
		Vector2 inPointBR = cam.ViewportToWorldPoint(new Vector2(1 - (2 * (differentMargins ? screenMargins.right : screenMargin)), 1 - (2 * (differentMargins ? screenMargins.down : screenMargin))));

		Gizmos.DrawLine(outPointTL, new Vector2(outPointTL.x, outPointBR.y));
		Gizmos.DrawLine(new Vector2(outPointTL.x, outPointBR.y), outPointBR);
		Gizmos.DrawLine(outPointTL, new Vector2(outPointBR.x, outPointTL.y));
		Gizmos.DrawLine(new Vector2(outPointBR.x, outPointTL.y), outPointBR);

		Gizmos.DrawLine(inPointTL, new Vector2(inPointTL.x, inPointBR.y));
		Gizmos.DrawLine(new Vector2(inPointTL.x, inPointBR.y), inPointBR);
		Gizmos.DrawLine(inPointTL, new Vector2(inPointBR.x, inPointTL.y));
		Gizmos.DrawLine(new Vector2(inPointBR.x, inPointTL.y), inPointBR);
	}

}

[System.Serializable]
public class Margins {
	public float up = 0f;
	public float down = 0f;
	public float left = 0f;
	public float right = 0f;
}
