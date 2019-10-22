using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class FadeUI : MonoBehaviour {

	public float timeScale = 1;
	public bool shouldShow = false;

	private MaskableGraphic graphic;

	private Color showColour = Color.white;
	private Color hideColour = new Color(1, 1, 1, 0);

	private float currTime;

	void Start() {
		graphic = GetComponent<MaskableGraphic>();
		graphic.color = shouldShow ? showColour : hideColour;
		currTime = shouldShow ? 1 : 0;
	}

	void Update() {
		currTime += Time.deltaTime * timeScale * (shouldShow ? 1 : -1);
		currTime = Mathf.Clamp01(currTime);
		graphic.color = Color.Lerp(hideColour, showColour, currTime);
	}
}
