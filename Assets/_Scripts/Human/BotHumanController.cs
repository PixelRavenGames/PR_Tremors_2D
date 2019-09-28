using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHumanController : IHumanController {

	protected int controllerNum = 0;
	protected Human human;

	private bool needMove = false;
	private float moveMagnitude = 0;
	private bool needJump = false;

	private enum BotControllerState { IDLE, WANDER, RUN }
	private BotControllerState state = BotControllerState.IDLE;

	private float needMovemnentUpdate = 3f;
	private float targetMoveMagnitude = 0;

	private BotSettings settings;

	public BotHumanController(int controllerNum, Human human, BotSettings settings) {
		this.controllerNum = controllerNum;
		this.human = human;
		this.settings = settings;
	}

	public bool GetJumpButton() {
		return needJump;
	}

	public float GetMoveMagnitude() {
		return moveMagnitude;
	}

	public bool ShouldMove() {
		return needMove;
	}

	public void Update() {

		switch (state) {
			case BotControllerState.IDLE:
				targetMoveMagnitude = 0;

				if (GetDistanceFromMeteor() < settings.minimumIdleDistance || Random.Range(0, 100f) <= settings.idleEndChance) { state = BotControllerState.WANDER; }

				break;
			case BotControllerState.WANDER:
				needMovemnentUpdate -= Time.deltaTime;
				if (needMovemnentUpdate <= 0) {
					targetMoveMagnitude = Random.Range(0, 2) == 0 ? -1 : 1;
					needMovemnentUpdate += settings.wanderUpdateDelay;
				}

				if (GetDistanceFromMeteor() > settings.minimumIdleDistance && Random.Range(0, 100f) <= settings.wanderEndChance) { state = BotControllerState.IDLE; }
				if (GetDistanceFromMeteor() < settings.runDistance) { state = BotControllerState.RUN; }

				if (moveMagnitude > settings.wanderJumpMinimumSpeed) {
					if (!needJump) needJump = Random.Range(0, 100) == 0;
					else {
						needJump = Random.Range(0, 100) <= settings.wanderContinueJumpChance;
					}
				}

				break;
			case BotControllerState.RUN:
				targetMoveMagnitude = 1 - Mathf.Clamp01(GetDistanceFromMeteor());
				targetMoveMagnitude = Mathf.Sign(GetDirectionToMeteor().x);

				needJump = Mathf.Abs(GetDirectionToMeteor().y) > settings.runDistanceToJump && Random.Range(0, 100f) <= settings.runJumpChance;

				if (GetDistanceFromMeteor() > settings.runDistance) { state = BotControllerState.WANDER; }

				break;
		}

		moveMagnitude = Mathf.Lerp(moveMagnitude, targetMoveMagnitude, Time.deltaTime * 2);
		needMove = moveMagnitude != 0;
	}

	private float GetDistanceFromMeteor() {
		Vector2 closest = Vector2.zero;
		float closestDistance = float.MaxValue;
		foreach (Vector2 pos in PlayerManager.instance.GetMeteorPositions()) {
			float newDist = Vector2.Distance(pos, human.transform.position);
			if (newDist < closestDistance) {
				closest = pos;
			}
		}

		return closestDistance;
	}

	private Vector2 GetDirectionToMeteor() {
		Vector2 closest = Vector2.zero;
		float closestDistance = float.MaxValue;
		foreach (Vector2 pos in PlayerManager.instance.GetMeteorPositions()) {
			float newDist = Vector2.Distance(pos, human.transform.position);
			if (newDist < closestDistance) {
				closest = pos;
			}
		}

		return (Vector2)human.transform.position - closest;
	}

}
