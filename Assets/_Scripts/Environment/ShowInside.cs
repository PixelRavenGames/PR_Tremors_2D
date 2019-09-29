using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInside : MonoBehaviour {

	public float transitionTime = 0.1f;
	public float transparency = 0.25f;

	private bool showInside = false;

	private Color startColour;
	private Color endColour;

	private SpriteRenderer sr;

	private float timer = 0;

	[SerializeField]
	private bool arePlayersInside = false;

	void Start() {
		sr = GetComponent<SpriteRenderer>();

		startColour = sr.color;
		endColour = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - transparency);
	}

	void Update() {
		timer += Time.deltaTime * (1 / transitionTime) * (showInside ? 1 : -1);
		timer = Mathf.Clamp01(timer);

		if (arePlayersInside && !showInside) Show();
		if (!arePlayersInside && showInside) Hide();

		sr.color = Color.Lerp(startColour, endColour, timer);
	}

	public void Show() {
		showInside = true;
	}

	public void Hide() {
		showInside = false;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.GetComponent<Human>()) {
			arePlayersInside = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		arePlayersInside = false;
	}
}
