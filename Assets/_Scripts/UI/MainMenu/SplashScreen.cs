using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	public CanvasGroup mainMenu;
	public CanvasGroup splashScreen;

	public float timeScale = 1;

	private bool gotoMainMenu = false;
	private float time = 0;

	void Start() {
		splashScreen.alpha = 1;
		mainMenu.alpha = 0;

		splashScreen.interactable = true;
		mainMenu.interactable = false;
	}

	void Update() {
		if (!splashScreen.interactable && Input.anyKeyDown) {
			gotoMainMenu = true;
		}
		
		if (gotoMainMenu) {
			time += Time.deltaTime * timeScale;

			splashScreen.alpha = time;
			mainMenu.alpha = 1 - time;

			if (time >= 1) {
				mainMenu.interactable = true;
				splashScreen.interactable = false;

				gotoMainMenu = false;
			}

		}

	}
}
