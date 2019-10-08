using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Human Sprites", menuName = "Human/Sprites", order = 1)]
public class HumanSpriteSelection : ScriptableObject {

	[SerializeField]
	private Sprite[] male;
	[SerializeField]
	private Sprite[] female;

	public Sprite GetSprite(bool maleFlg, int index) {
		return maleFlg ? male[index] : female[index];
	}

	public int GetSpriteCount(bool maleFlg) {
		return maleFlg ? male.Length : female.Length;
	}

}
