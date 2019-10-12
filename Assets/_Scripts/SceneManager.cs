using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManager : MonoBehaviour {

	private static readonly Dictionary<Scenes, int> scenes = new Dictionary<Scenes, int>() {
		{ Scenes.MAIN_MENU, -1 },
		{ Scenes.LOBBY, 0 },
		{ Scenes.GAME, 1 },
		{ Scenes.WIN_SCREEN, -1 },
		{ Scenes.LOADING_MENU, -1 }
	};

	public static void LoadSceneStatic(int scene) {
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}

	public static void LoadSceneStatic(Scenes scene) {
		LoadSceneStatic(scenes[scene]);
	}

	public void LoadScene(Scenes scene) {
		LoadSceneStatic(scene);
	}

	public void LoadScene(int scene) {
		LoadSceneStatic(scene);
	}

}

[System.Serializable]
public enum Scenes {
	MAIN_MENU,
	LOBBY,
	GAME,
	WIN_SCREEN,
	LOADING_MENU
}
