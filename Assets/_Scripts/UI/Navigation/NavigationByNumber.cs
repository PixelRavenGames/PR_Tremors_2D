using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationByNumber : MonoBehaviour {

	public bool upPathRequiresNumbers = false;
	[ConditionalHide("upPathRequiresNumbers", true, false)]
	public PathByNumber pathsOnUp;
	[ConditionalHide("upPathRequiresNumbers", true, true)]
	public Selectable pathOnUp;

	public bool leftPathRequiresNumbers = false;
	[ConditionalHide("leftPathRequiresNumbers", true, false)]
	public PathByNumber pathsOnLeft;
	[ConditionalHide("leftPathRequiresNumbers", true, true)]
	public Selectable pathOnLeft;

	public bool rightPathRequiresNumbers = false;
	[ConditionalHide("rightPathRequiresNumbers", true, false)]
	public PathByNumber pathsOnRight;
	[ConditionalHide("rightPathRequiresNumbers", true, true)]
	public Selectable pathOnRight;

	public bool downPathRequiresNumbers = false;
	[ConditionalHide("downPathRequiresNumbers", true, false)]
	public PathByNumber pathsOnDown;
	[ConditionalHide("downPathRequiresNumbers", true, true)]
	public Selectable pathOnDown;

	public Selectable SelectOnUp(int controller) {
		return upPathRequiresNumbers ? pathsOnUp[controller] : pathOnUp;
	}

	public Selectable SelectOnDown(int controller) {
		return downPathRequiresNumbers ? pathsOnDown[controller] : pathOnDown;
	}

	public Selectable SelectOnLeft(int controller) {
		return leftPathRequiresNumbers ? pathsOnLeft[controller] : pathOnLeft;
	}

	public Selectable SelectOnRight(int controller) {
		return rightPathRequiresNumbers ? pathsOnRight[controller] : pathOnRight;
	}

}

[System.Serializable]
public class PathByNumber {
	// I AM AWARE ARRAYS EXIST -- This is for formatting in the inspector
	[SerializeField]
	private Selectable joystick1;
	[SerializeField]
	private Selectable joystick2;
	[SerializeField]
	private Selectable joystick3;
	[SerializeField]
	private Selectable joystick4;
	[SerializeField]
	private Selectable joystick5;
	[SerializeField]
	private Selectable joystick6;
	[SerializeField]
	private Selectable joystick7;
	[SerializeField]
	private Selectable joystick8;

	public Selectable this[int index] {
		get {
			switch(index) {
				case 1:
					return joystick1;
				case 2:
					return joystick2;
				case 3:
					return joystick3;
				case 4:
					return joystick4;
				case 5:
					return joystick5;
				case 6:
					return joystick6;
				case 7:
					return joystick7;
				case 8:
					return joystick8;
				default:
					return null;
			}
		}
		set {
			switch (index) {
				case 1:
					joystick1 = value;
					break;
				case 2:
					joystick2 = value;
					break;
				case 3:
					joystick3 = value;
					break;
				case 4:
					joystick4 = value;
					break;
				case 5:
					joystick5 = value;
					break;
				case 6:
					joystick6 = value;
					break;
				case 7:
					joystick7 = value;
					break;
				case 8:
					joystick8 = value;
					break;
			}
		}
	}

}
