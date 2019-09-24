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

				if (GetDistanceFromWorm() < settings.minimumIdleDistance || Random.Range(0, 100f) <= settings.idleEndChance) { state = BotControllerState.WANDER; }

				break;
			case BotControllerState.WANDER:
				needMovemnentUpdate -= Time.deltaTime;
				if (needMovemnentUpdate <= 0) {
					targetMoveMagnitude = Random.Range(0, 1) == 0 ? -1 : 1;
					needMovemnentUpdate += settings.wanderUpdateDelay;
				}

				if (GetDistanceFromWorm() > settings.minimumIdleDistance && Random.Range(0, 100f) <= settings.wanderEndChance) { state = BotControllerState.IDLE; }
				if (GetDistanceFromWorm() < settings.runDistance) { state = BotControllerState.RUN; }

				if (moveMagnitude > settings.wanderJumpMinimumSpeed) {
					if (!needJump) needJump = Random.Range(0, 100) == 0;
					else {
						needJump = Random.Range(0, 100) <= settings.wanderContinueJumpChance;
					}
				}

				break;
			case BotControllerState.RUN:
				targetMoveMagnitude = 1 - Mathf.Clamp01(GetDistanceFromWorm());
				targetMoveMagnitude = Mathf.Sign(GetDirectionToWorm().x);

				needJump = Mathf.Abs(GetDirectionToWorm().y) > settings.runDistanceToJump && Random.Range(0, 100f) <= settings.runJumpChance;

				if (GetDistanceFromWorm() > settings.runDistance) { state = BotControllerState.WANDER; }

				break;
		}

		moveMagnitude = Mathf.Lerp(moveMagnitude, targetMoveMagnitude, Time.deltaTime * 2);
		needMove = moveMagnitude != 0;
	}

	private float GetDistanceFromWorm() {
		return Vector2.Distance(human.transform.position, PlayerManager.instance.GetWormPosition());
	}

	private Vector2 GetDirectionToWorm() {
		return (Vector2)human.transform.position - PlayerManager.instance.GetWormPosition();
	}

}
