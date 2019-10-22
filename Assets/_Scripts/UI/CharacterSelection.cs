using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

	public int controllerNumber;
	public Image preview;
	public HumanSpriteSelection sprites;

	public Image background;
	public Text readyText;

	private int index;

	private bool lateStartRan = false;

	void Start() {
		index = 0;

		if (!JoystickExists(controllerNumber)) {
			Randomize();
		}

		UpdateChange();

		readyText.enabled = false;
	}

	public void Next() {
		index++;
		index %= sprites.GetSpriteCount();

		UpdateChange();
	}

	public void Prev() {
		index--;
		if (index < 0) index += sprites.GetSpriteCount();

		UpdateChange();
	}

	public void Randomize() {
		index = Random.Range(0, sprites.GetSpriteCount());

		UpdateChange();
	}

	private void UpdateChange() {
		preview.sprite = sprites.GetSprite(index);

		PlayerPrefs.SetInt($"PLAYER{controllerNumber}_SPRITE_INDEX", index);

	}

	private bool JoystickExists(int joystick) {
		joystick -= 1;
		string[] joysticks = Input.GetJoystickNames();
		Debug.Log(joystick < joysticks.Length && !string.IsNullOrEmpty(joysticks[joystick]));
		return joystick < joysticks.Length && !string.IsNullOrEmpty(joysticks[joystick]);
	}

	public void OnReady(int controller) {
		if (controller == controllerNumber) OnReady();
	}

	private void OnReady() {
		background.color = new Color(background.color.r - 0.1f, background.color.g - 0.1f, background.color.b - 0.1f);
		readyText.enabled = true;
	}

	public void OnUnReady(int controller) {
		if (controller == controllerNumber) OnUnReady();
	}

	private void OnUnReady() {
		background.color = new Color(background.color.r + 0.1f, background.color.g + 0.1f, background.color.b + 0.1f);
		readyText.enabled = false;
	}

}
