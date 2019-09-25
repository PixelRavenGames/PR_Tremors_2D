using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour {

	[Header("References")]
	public SpriteRenderer indicator;
	public SpriteRenderer arrow;

	[Header("Animation Settings")]
	public float animTime = 0.25f;
	public Vector3 offset = Vector2.up;
	public float changeAcceleration = 2.5f;

	[Header("Indicator Settings")]
	public Sprite[] playerNumberSprites;
	public Color[] playerNumberColours;

	private float animTimer = 0;
	private bool moveDirection = true;

	private int controllerNumber = -1;
	private bool isConnected = false;

	private SpriteRenderer tmpChangeSprite = null;
	private float timeScale = 1f;

	private enum IndicatorAnimationState { BOBBING, CHANGING, NONE }
	private IndicatorAnimationState state = IndicatorAnimationState.BOBBING;

	void Update() {
		if (controllerNumber <= 0) return;
		UpdateAnimation();
	}

	public void UpdateValues(int controllerNumber, bool isConnected) {
		if (this.controllerNumber != controllerNumber || this.isConnected != isConnected) {

			this.controllerNumber = controllerNumber;
			this.isConnected = isConnected;

			ChangeState(IndicatorAnimationState.CHANGING);
		}

	}

	private void UpdateAnimation() {
		switch (state) {
			case IndicatorAnimationState.BOBBING:
				animTimer -= Time.deltaTime;
				if (animTimer <= 0) {
					animTimer += animTime;
					indicator.transform.localPosition = (moveDirection ? offset : -offset);
					moveDirection = !moveDirection;
				}
				break;
			case IndicatorAnimationState.CHANGING:
				animTimer += Time.deltaTime * timeScale;
				float percentage = animTimer / animTime;

				indicator.transform.localPosition = Vector2.Lerp(Vector2.up, Vector2.zero, percentage);

				if (percentage >= 1f) {

					indicator.transform.localPosition = Vector2.zero;

					if (tmpChangeSprite) {
						Destroy(tmpChangeSprite.gameObject);
						tmpChangeSprite = null;
					}

					arrow.color = playerNumberColours[controllerNumber];

					ChangeState(IndicatorAnimationState.BOBBING);
					animTimer = 0;
					moveDirection = true;
				}

				timeScale += changeAcceleration;
				break;
		}
	}

	private void ChangeState(IndicatorAnimationState state) {

		if (this.state == IndicatorAnimationState.CHANGING) {
			if (tmpChangeSprite) {
				Destroy(tmpChangeSprite.gameObject);
				tmpChangeSprite = null;
			}
		}

		this.state = state;

		if (this.state == IndicatorAnimationState.CHANGING) {
			timeScale = 1f;

			GameObject go = Instantiate(indicator.gameObject, indicator.transform.position, Quaternion.identity, transform);
			tmpChangeSprite = go.GetComponent<SpriteRenderer>();

			indicator.sprite = playerNumberSprites[isConnected ? controllerNumber : 0];
			indicator.color = playerNumberColours[controllerNumber];

			indicator.transform.localPosition = Vector2.up;
		}
	}

}
