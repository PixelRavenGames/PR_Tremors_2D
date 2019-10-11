using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Human Sprites", menuName = "Human/Sprites", order = 1)]
public class HumanSpriteSelection : ScriptableObject {

	[SerializeField]
	private HumanSpriteSet[] male;
	[SerializeField]
	private HumanSpriteSet[] female;

	public Sprite GetSprite(bool maleFlg, int index, bool crouching = false) {
		HumanSpriteSet set = GetSpriteSet(maleFlg, index);
		return (crouching ? set.crouching : set.main);
	}

	public HumanSpriteSet GetSpriteSet(bool maleFlg, int index) {
		return maleFlg ? male[index] : female[index];
	}

	public int GetSpriteCount(bool maleFlg) {
		return maleFlg ? male.Length : female.Length;
	}

}

[System.Serializable]
public class HumanSpriteSet {
	public Sprite main;
	public Sprite crouching;
}
