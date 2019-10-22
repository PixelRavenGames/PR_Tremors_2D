using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : Selectable {

	public int controllerNumber;
	public Image preview;
	public HumanSpriteSelection sprites;

	public Image background;
	public Text readyText;

	private int index;

	private Vector2 prevInput;

	private bool lateStartRan = false;

	void Update() {
		if (!lateStartRan) {
			LateStart();
			lateStartRan = true;
		}

		CheckInput();
	}

	private void LateStart() {
		index = 0;

		if (!JoystickExists(controllerNumber)) {
			Randomize();
		}

		UpdateChange();

		readyText.enabled = false;
	}

	#region Interface
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
	#endregion


	private void CheckInput() {
		Vector2 input = new Vector2(Input.GetAxis($"Joystick{controllerNumber}X"), 0);

		if (IsInputRight(input)) Next();
		if (IsInputLeft(input)) Prev();

		prevInput = input;
	}

	private bool IsInputLeft(Vector2 input) {
		return (input.x < -0.5f) && !(prevInput.x < -0.5f);
	}

	private bool IsInputRight(Vector2 input) {
		return (input.x > 0.5f) && !(prevInput.x > 0.5f);
	}

	private void UpdateChange() {
		preview.sprite = sprites.GetSprite(index);

		PlayerPrefs.SetInt($"PLAYER{controllerNumber}_SPRITE_INDEX", index);

	}

	private bool JoystickExists(int joystick) {
		joystick -= 1;
		string[] joysticks = Input.GetJoystickNames();
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
