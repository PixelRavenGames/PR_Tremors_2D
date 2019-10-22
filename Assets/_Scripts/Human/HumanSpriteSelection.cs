using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Human Sprites", menuName = "Human/Sprites", order = 1)]
public class HumanSpriteSelection : ScriptableObject {

	[SerializeField]
	private HumanSpriteSet[] sprites;

	public Sprite GetSprite(int index, bool crouching = false) {
		HumanSpriteSet set = GetSpriteSet(index);
		return (crouching ? set.crouching : set.main);
	}

	public HumanSpriteSet GetSpriteSet(int index) {
		return sprites[index];
	}

	public int GetSpriteCount() {
		return sprites.Length;
	}

}

[System.Serializable]
public class HumanSpriteSet {
	public Sprite main;
	public Sprite crouching;
}
