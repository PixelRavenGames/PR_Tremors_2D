using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

	public int controllerNumber;
	public Image preview;
	public HumanSpriteSelection sprites;

	private int index;
	private bool male;

	private bool lateStartRan = false;

	void Start() {
		index = 0;
		male = true;

		if (!JoystickExists(controllerNumber)) {
			male = Random.Range(0, 2) == 0;
			Randomize();
		}

		UpdateChange();
	}

	public void Next() {
		index++;
		index %= sprites.GetSpriteCount(male);

		UpdateChange();
	}

	public void Prev() {
		index--;
		if (index < 0) index += sprites.GetSpriteCount(male);

		UpdateChange();
	}

	public void Randomize() {
		index = Random.Range(0, sprites.GetSpriteCount(male));

		UpdateChange();
	}

	public void SetMale() {
		male = true;
		index %= sprites.GetSpriteCount(male);

		UpdateChange();
	}

	public void SetFemale() {
		male = false;
		index %= sprites.GetSpriteCount(male);

		UpdateChange();
	}

	private void UpdateChange() {
		preview.sprite = sprites.GetSprite(male, index);

		PlayerPrefs.SetInt($"PLAYER{controllerNumber}_SPRITE_INDEX", index);
		PlayerPrefs.SetInt($"PLAYER{controllerNumber}_SPRITE_GENDER", male ? 1 : 0);

	}

	private bool JoystickExists(int joystick) {
		string[] joysticks = Input.GetJoystickNames();
		return joystick < joysticks.Length && !string.IsNullOrEmpty(joysticks[joystick]);
	}

}
