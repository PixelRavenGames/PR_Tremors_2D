using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Human : MonoBehaviour, IDamageable {

	[Header("Controller")]
	public int controllerNum = 0;

	private IHumanController control;
	private bool controllerConnected = false;

	[Header("Settings")]
	[SerializeField]
	private float jumpHeight = 3;
	[SerializeField]
	private float fallModifier = 1f;
	[SerializeField]
	private float speed = 1f;
	[SerializeField]
	private float dashSpeed = 1f;
	[SerializeField]
	private float dashCooldown = 0.5f;

	[Header("References")]
	public PlayerIndicator indicator;
	public StunIndicator stunIndicator;
	public GameObject deathEffect;

	[Header("Sprites")]
	public Sprite defaultSprite;
	public Sprite crouchSprite;

	// TODO [Header("Events")]

	private bool jumpWasPressed = false;
	private bool canJump = false;
	private bool canWallJump = false;

	private bool dashWasPressed = false;

	private bool isCrouching = false;

	private bool alive = true;

	private float stunTime = 0;
	private enum WallSide { NONE, LEFT, RIGHT }
	private WallSide attachedToWall = WallSide.NONE;

	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private TrailRenderer tr;

	private float dashTimer = 0;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		tr = GetComponent<TrailRenderer>();

		sr.sprite = defaultSprite;

		tr.emitting = false;
	}

	void Update() {
		if (control == null) return;

		canWallJump = (attachedToWall == WallSide.LEFT) || (attachedToWall == WallSide.RIGHT);

		if (Input.GetKeyDown(KeyCode.Space)) Kill();

		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallModifier - 1) * Time.deltaTime;
		}

		if (Mathf.Abs(rb.velocity.x) > 0) {
			rb.velocity = new Vector2(rb.velocity.x * 0.7f, rb.velocity.y);
		}

		stunIndicator.shouldShow = stunTime > 0f;
		if (stunTime <= 0f) {
			control.Update();
			if (control.ShouldMove()) Move(control.GetMoveMagnitude());

			bool isJumpDown = IsJumpDown();
			if (control.GetJumpButton()) {
				if (isJumpDown) {
					if (canWallJump) {
						Jump(3, attachedToWall == WallSide.LEFT ? -45 : 45);
					} else if (canJump) {
						Jump();
					}
				}
			} else if (rb.velocity.y > 0) {
				rb.velocity += Vector2.up * Physics2D.gravity.y * 3 * Time.deltaTime;
			}

			bool wasCrouching = isCrouching;
			isCrouching = !control.ShouldMove() && control.GetCrouchButton();

			if (!wasCrouching && isCrouching) sr.sprite = crouchSprite;
			if (wasCrouching && !isCrouching) sr.sprite = defaultSprite;

			bool dashButton = control.GetDashButton();
			if (!dashWasPressed && dashButton) Dash();
			dashWasPressed = dashButton;

			dashTimer -= Time.deltaTime;

		} else {
			stunTime -= Time.deltaTime;
			dashTimer = 0;
		}

		if (dashTimer <= dashCooldown - 0.2f) tr.emitting = false;

		attachedToWall = WallSide.NONE;
		sr.flipX = rb.velocity.x < 0;

	}

	public void Move(float magnitude) {
		float previousX = rb.velocity.x;

		Vector2 newVelocity = new Vector2(magnitude * speed, rb.velocity.y);

		if (Mathf.Sign(newVelocity.x) != Mathf.Sign(previousX)) {
			newVelocity = new Vector2(previousX + (newVelocity.x * 0.01f), newVelocity.y);
		} else {
			newVelocity = new Vector2(Mathf.Max(Mathf.Abs(previousX), Mathf.Abs(newVelocity.x)) * Mathf.Sign(newVelocity.x), newVelocity.y);
		}

		rb.velocity = newVelocity;
	}

	public void Jump(float magnitude = 1, float angle = 0) {
		if (jumpHeight <= 0) return;

		float targetHeight = jumpHeight * magnitude;

		float velocity = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * targetHeight);

		Vector2 jumpVector = Vector2.up * (float.IsNaN(velocity) ? 0 : velocity);

		if (angle != 0) {
			float rad = angle * Mathf.Deg2Rad;
			float cos = Mathf.Cos(rad);
			float sin = Mathf.Sin(rad);
			jumpVector = new Vector2((jumpVector.x * cos) - (jumpVector.y * sin), (jumpVector.y * cos) - (jumpVector.x * sin));
		}

		rb.velocity = new Vector2(rb.velocity.x + jumpVector.x, jumpVector.y);

		canJump = false;
		attachedToWall = WallSide.NONE;
		canWallJump = false;
	}

	public void Dash() {
		if (dashTimer > 0) return;

		rb.velocity += Vector2.right * control.GetMoveMagnitude() * dashSpeed;
		tr.emitting = true;

		dashTimer = dashCooldown;
	}

	private bool IsJumpDown() {
		bool wasDownFlg = jumpWasPressed;
		jumpWasPressed = control.GetJumpButton();
		return !wasDownFlg && jumpWasPressed;
	}

	private bool IsJumpUp() {
		bool wasDownFlg = jumpWasPressed;
		jumpWasPressed = control.GetJumpButton();
		return wasDownFlg && !jumpWasPressed;
	}

	public bool ChangeController(IHumanController controller) {
		control = controller;
		controllerConnected = control is PlayerHumanController;

		indicator?.UpdateValues(controllerNum, controllerConnected);

		return controllerConnected;
	}

	public void Kill() {
		if (!alive) return;
		alive = false;

		Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 5);

		Stun(2, 0);
		transform.position += (Vector3)Vector2.up * 10f;

		GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.75f, 1f, 0.5f);
	}

	public void Stun(float time, float random = 0.1f) {
		stunTime += (isCrouching ? 0.25f : 1) * time * Random.Range(1 - random, 1 + random);
	}

	public bool IsAlive() {
		return alive;
	}

	private void OnCollisionStay2D(Collision2D collision) {
		ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
		collision.GetContacts(contactPoints);

		foreach(ContactPoint2D cp in contactPoints) {
			Vector2 point = transform.InverseTransformPoint(cp.point);

			if (point.y <= -0.10f) {
				canJump = true;
			} else {
				if (point.x < -0.07f) { attachedToWall = WallSide.LEFT; }
				if (point.x > 0.07f) { attachedToWall = WallSide.RIGHT; }
			}

		}

	}

	private void OnCollisionExit2D(Collision2D collision) {
		attachedToWall = WallSide.NONE;
	}

	public void Damage(Vector2 damageSource, float damageMultiplier = 1) {
		float distance = Vector2.Distance(transform.position, damageSource);

		float damage = 1 - Mathf.Pow(distance / damageMultiplier, 2);

		if (damage > 0.75f) Kill();
		else Stun(damage * 2f);

	}

	public void Damage(float rawDamage) {
		if (rawDamage > 0.75f) Kill();
		else Stun(rawDamage * 2f);
	}
}
