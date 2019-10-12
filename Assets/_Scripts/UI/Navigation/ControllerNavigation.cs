using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerNavigation : MonoBehaviour {

	public Selectable[] startSelection;

	private Selectable[] selected;

	private Vector2[] prevInput;
	private bool[] prevPressed;

	void Start() {
		selected = new Selectable[startSelection.Length];
		for (int i = 0; i < startSelection.Length; i++) Select(startSelection[i], i);

		prevInput = new Vector2[startSelection.Length];
		for (int i = 0; i < prevInput.Length; i++) prevInput[i] = Vector2.zero;

		prevPressed = new bool[startSelection.Length];
		for (int i = 0; i < prevPressed.Length; i++) prevPressed[i] = false;
	}

	void Update() {
		Navigate();
		CheckPress();
	}

	public void Select(Selectable selectable, int controller) {
		Selectable last = selected[controller];
		selected[controller] = selectable;

		UpdateDisplay(last, selectable);
	}

	private void CheckPress() {
		for(int i = 0; i < selected.Length; i++) {
			bool pressed = Input.GetAxis($"Joystick{i + 1}button0") > 0;
			if (pressed && !prevPressed[i]) { Press(i); }
			prevPressed[i] = pressed;
		}
	}

	public void Press(int controller) {
		Selectable s = selected[controller];
		if (s is Button) {
			((Button) s).onClick.Invoke();
		}
	}

	public Selectable NavigateUp(Selectable start, int controller) {
		Selectable s = start.navigation.selectOnUp;

		NavigationByNumber nav = start.GetComponent<NavigationByNumber>();
		if (nav) s = nav.SelectOnUp(controller);

		if (!s) return start;
		if (s.interactable) return s;

		Selectable newS = NavigateUp(s, controller);
		return newS == s ? start : newS;
	}

	public Selectable NavigateDown(Selectable start, int controller) {
		Selectable s = start.navigation.selectOnDown;

		NavigationByNumber nav = start.GetComponent<NavigationByNumber>();
		if (nav) s = nav.SelectOnDown(controller);
		
		if (!s) return start;
		if (s.interactable) return s;

		Selectable newS = NavigateDown(s, controller);
		return newS == s ? start : newS;
	}

	public Selectable NavigateLeft(Selectable start, int controller) {
		Selectable s = start.navigation.selectOnLeft;

		NavigationByNumber nav = start.GetComponent<NavigationByNumber>();
		if (nav) s = nav.SelectOnLeft(controller);

		if (!s) return start;
		if (s.interactable) return s;

		Selectable newS = NavigateLeft(s, controller);
		return newS == s ? start : newS;
	}

	public Selectable NavigateRight(Selectable start, int controller) {
		Selectable s = start.navigation.selectOnRight;

		NavigationByNumber nav = start.GetComponent<NavigationByNumber>();
		if (nav) s = nav.SelectOnRight(controller);

		if (!s) return start;
		if (s.interactable) return s;

		Selectable newS = NavigateRight(s, controller);
		return newS == s ? start : newS;
	}

	private void Navigate() {
		for (int i = 0; i < selected.Length; i++) {
			Vector2 input = new Vector2(Input.GetAxis($"Joystick{i + 1}X"), Input.GetAxis($"Joystick{i + 1}Y"));

			if (IsInputDown(input, i)) Select(NavigateDown(selected[i], i + 1), i);
			if (IsInputUp(input, i)) Select(NavigateUp(selected[i], i + 1), i);
			if (IsInputRight(input, i)) Select(NavigateRight(selected[i], i + 1), i);
			if (IsInputLeft(input, i)) Select(NavigateLeft(selected[i], i + 1), i);

			prevInput[i] = input;
		}
	}

	private void UpdateDisplay(Selectable last, Selectable next) {
		if (last) last.OnDeselect(new BaseEventData(EventSystem.current));
		if (next) next.OnSelect(new BaseEventData(EventSystem.current));
	}

	private bool IsInputUp(Vector2 input, int controller) {
		return (input.y < -0.5f) && !(prevInput[controller].y < -0.5f);
	}

	private bool IsInputDown(Vector2 input, int controller) {
		return (input.y > 0.5f) && !(prevInput[controller].y > 0.5f);
	}

	private bool IsInputLeft(Vector2 input, int controller) {
		return (input.x < -0.5f) && !(prevInput[controller].x < -0.5f);
	}

	private bool IsInputRight(Vector2 input, int controller) {
		return (input.x > 0.5f) && !(prevInput[controller].x > 0.5f);
	}

}
