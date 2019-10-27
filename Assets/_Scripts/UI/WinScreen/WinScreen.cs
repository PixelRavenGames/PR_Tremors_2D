using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	[Header("Managers")]
	public ScoreManager scoreManager;

	[Header("References")]
	public Image realWinnerImg;
	public Text realWinnerTxt;

	public Text awards;

	public Image[] scoreWinnersImg;
	public Text[] scoreWinnersTxt;

	public Button retry;


	[Header("Settings")]
	public Vector2 offscreenDisplacement;
	public float enterTime = 1;

	[Header("Format")]
	public string realWinnerString = "Winner - Player {0}";
	public string scoreString = "Player {0} - {1}";
	public string awardString = "{0} - Player {1}";

	private Vector2 startPos;

	private bool shouldShow = false;

	#region SubClass
	private class PlayerScore {
		public int controller;
		public Human player;
		public int score;
	}
	#endregion

	void Start() {

		PlayerManager.instance.onPlayerWin.AddListener(OnWin);

		startPos = transform.position;
		transform.position = startPos + offscreenDisplacement;
	}

	void Update() {
		if (shouldShow) transform.position = Vector2.Lerp(transform.position, startPos, Time.unscaledDeltaTime * (1/enterTime));
	}

	public void OnWin(int winner) {
		if (shouldShow) return;
		Human[] players = PlayerManager.instance.humans;

		if (winner > 0) {
			realWinnerImg.sprite = players[winner - 1].GetSprite();
			realWinnerTxt.text = string.Format(realWinnerString, winner);
		}

		PlayerScore[] playerScores = new PlayerScore[players.Length];

		float[] scores = scoreManager.GetScores();
		for (int i = 0; i < players.Length; i++) {
			playerScores[i] = new PlayerScore {
				controller = i + 1,
				score = (int)(scores[i] * 10),
				player = players[i]
			};
		}

		playerScores = SortPlayerScores(ref playerScores);
		for (int i = playerScores.Length - 1; i >= 0; i--) {
			scoreWinnersImg[i].sprite = playerScores[i].player.GetSprite();
			scoreWinnersTxt[i].text = string.Format(scoreString, playerScores[i].controller, playerScores[i].score);
		}

		Time.timeScale = 0.5f;
		shouldShow = true;

		EventSystem.current.SetSelectedGameObject(retry.gameObject);
	}

	private PlayerScore[] SortPlayerScores(ref PlayerScore[] scores) {
		for (int p = 0; p < scores.Length - 1; p++) {
			for (int i = scores.Length - 1; i > 0; i--) {
				if (scores[i].score > scores[i - 1].score) {
					(scores[i], scores[i - 1]) = (scores[i - 1], scores[i]);
				}
			}
		}

		return scores;
	}

}
