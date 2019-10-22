using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(TrailRenderer))]
public class Human : MonoBehaviour, IDamageable {

	#region Inspector
	[Header("Controller")]
	public int controllerNum = 0;

	[Header("Settings")]
	[SerializeField]
	private int maxJumpCount = 2;
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
	[SerializeField]
	private float deathYLevel = -5.5f;

	[Header("References")]
	public PlayerIndicator indicator;
	public StunIndicator stunIndicator;
	public GameObject deathEffect;

	[Header("Sprites")]
	public HumanSpriteSelection spritesReference;

	[Header("Events")]
	public OnHurtEvent onHurt = new OnHurtEvent();
	public OnDeathEvent onDeath = new OnDeathEvent();
	#endregion

	#region Private Variables
	// Control Object. This object controls what is inputted and when
	private IHumanController control;
	// Is a player controlling?
	private bool controllerConnected = false;

	// Was the jump input pressed last frame?
	private bool jumpWasPressed = false;
	// Can you normal jump?
	private bool canJump = false;
	// Can you wall jump?
	private bool canWallJump = false;
	// How many times have you jumped since last on ground?
	private int jumpCount = 0;

	// Invulnuability Flag
	private bool invulnerable = false;

	// Was dash button pressed last frame?
	private bool dashWasPressed = false;

	// Is crouch button pressed?
	private bool isCrouching = false;

	// Alive Flag
	private bool alive = true;

	// Remaining Stun Time
	private float stunTime = 0;

	// Enum for wall collision
	private enum WallSide { NONE, LEFT, RIGHT }
	// Which side are we touching a wall
	private WallSide attachedToWall = WallSide.NONE;

	// References
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private TrailRenderer tr;

	// Cooldown for dash
	private float dashTimer = 0;

	// Sprite Set For Player
	private HumanSpriteSet sprite;
	#endregion

	#region Unity Events
	void Start() {
		// Get References
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		tr = GetComponent<TrailRenderer>();

		// Set the Sprite
		int offset = PlayerPrefs.GetInt($"PLAYER{controllerNum}_SPRITE_INDEX", 1);
		sprite = spritesReference.GetSpriteSet(offset);

		sr.sprite = sprite.main;

		// Disable Trail
		tr.emitting = false;
	}

	void Update() {
		// Abort if no control object is found
		if (control == null) return;

		// Disable Wall Jump
		canWallJump = false;
		// Check if able to wall jump
		canWallJump = (attachedToWall == WallSide.LEFT) || (attachedToWall == WallSide.RIGHT);

		// Check if moving down
		if (rb.velocity.y < 0) {
			// Increase falling speed by multiplier of gravity
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallModifier - 1) * Time.deltaTime;
		}

		// If moving in the X
		if (Mathf.Abs(rb.velocity.x) > 0) {
			// Decrease velocity by 30%
			rb.velocity = new Vector2(rb.velocity.x * 0.7f, rb.velocity.y);
		}

		// Check if not stunned
		if (stunTime <= 0f) {
			// Update based on control object
			GetControlInput();
		} else {
			// Decrease stun time
			stunTime -= Time.deltaTime;
			// Reset dash cooldown
			dashTimer = 0;
		}		

		// Reset wall detection
		attachedToWall = WallSide.NONE;
		// Update sprite display
		UpdateDisplay();

		// If too low, die
		if (transform.position.y < deathYLevel) Kill();
	}

	private void OnCollisionStay2D(Collision2D collision) {

		if (collision.collider.gameObject.layer != 12) return;

		// Get Points of Contact
		ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
		collision.GetContacts(contactPoints);

		// Iterate over contact points
		foreach (ContactPoint2D cp in contactPoints) {
			// Convert contact point to local space
			Vector2 point = transform.InverseTransformPoint(cp.point);

			// If point is low enough
			if (point.y <= -0.10f) {
				// Reset jump count
				jumpCount = 0;
				// Enable Jumping
				canJump = true;
			} else {
				// If this isn't a jump-through platform
				if (!collision.collider.GetComponent<PlatformEffector2D>()) {
					// Check direction and set wall detection
					if (point.x < -0.07f) { attachedToWall = WallSide.LEFT; }
					if (point.x > 0.07f) { attachedToWall = WallSide.RIGHT; }
				}
			}

		}

	}

	private void OnCollisionExit2D(Collision2D collision) {
		// Reset Wall Detection
		attachedToWall = WallSide.NONE;
		// Increment Jump Count
		jumpCount++;
		// Disable Wall Jump
		canWallJump = false;
	}
	#endregion

	#region Interface

	public bool ChangeController(IHumanController controller) {
		// Set controller
		control = controller;
		// Check if player controlled
		controllerConnected = control is PlayerHumanController;

		// Update indicator display
		indicator?.UpdateValues(controllerNum, controllerConnected);

		// return player controlled flag
		return controllerConnected;
	}

	public void Move(float magnitude) {
		// Store last movement in x
		float previousX = rb.velocity.x;

		// Get new movement
		Vector2 newVelocity = new Vector2(magnitude * speed, rb.velocity.y);

		// If moving in new direction
		if (Mathf.Sign(newVelocity.x) != Mathf.Sign(previousX)) {
			// Smooth turnaround
			newVelocity = new Vector2(previousX + (newVelocity.x * 0.01f), newVelocity.y);
		} else {
			// Continue at the higher speed
			newVelocity = new Vector2(Mathf.Max(Mathf.Abs(previousX), Mathf.Abs(newVelocity.x)) * Mathf.Sign(newVelocity.x), newVelocity.y);
		}

		// Set new movement
		rb.velocity = newVelocity;
	}

	public void Jump(float magnitude = 1, float angle = 0) {
		// Check for valid value
		if (jumpHeight <= 0) return;

		// Get a target hight based on magnitude
		float targetHeight = jumpHeight * magnitude;

		// Get velocity based on target height
		float velocity = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * targetHeight);

		// Create Vector
		Vector2 jumpVector = Vector2.up * (float.IsNaN(velocity) ? 0 : velocity);

		// If angle is specified
		if (angle != 0) {
			// Rotate vector
			float rad = angle * Mathf.Deg2Rad;
			float cos = Mathf.Cos(rad);
			float sin = Mathf.Sin(rad);
			jumpVector = new Vector2((jumpVector.x * cos) - (jumpVector.y * sin), (jumpVector.y * cos) - (jumpVector.x * sin));
		}

		// Apply velocity
		rb.velocity = new Vector2(rb.velocity.x + jumpVector.x, jumpVector.y);

		// Increment Jump Count
		jumpCount++;
		// If jump count reaches max disable jump
		if (jumpCount >= maxJumpCount) canJump = false;
		// Reset Wall Detection
		attachedToWall = WallSide.NONE;
		// Disable Wall Jump
		canWallJump = false;
	}

	public void Dash() {
		// If cooldown is not completed, stop
		if (dashTimer > 0) return;

		// Add Velocity based on movement
		rb.velocity += Vector2.right * control.GetMoveMagnitude() * dashSpeed;
		// Enable Trail
		tr.emitting = true;

		// Set Cooldown
		dashTimer = dashCooldown;
	}

	public void Kill() {
		// If already dead, stop
		if (!alive) return;
		// Run Death Event
		onDeath.Invoke();
		// Set Alive Flag
		alive = false;

		// Create Partcles, Schedule Destruction
		Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 5);

		Destroy(gameObject);

		//// Stun for 2 Seconds
		//Stun(2, 0);
		//// Move into the air
		//transform.position += (Vector3) Vector2.up * 5f;
		//
		//// Set to ghost colour;
		//GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.75f, 1f, 0.5f);
	}

	public void Stun(float time, float random = 0.1f) {
		stunTime += (isCrouching ? 0.25f : 1) * time * Random.Range(1 - random, 1 + random);
	}

	public void Damage(Vector2 damageSource, float damageMultiplier = 1) {
		float distance = Vector2.Distance(transform.position, damageSource);

		float damage = 1 - Mathf.Pow(distance / damageMultiplier, 2);

		Damage(damage);

	}

	public bool DropThrough() {
		bool ret = false;

		if (DropThrough(Vector2.zero)) ret = true;

		return ret;
	}

	public bool DropThrough(Vector2 offset) {
		RaycastHit2D hit = Physics2D.Raycast((Vector2) transform.position + offset + new Vector2(0, -0.17f), Vector2.down, 0.5f);

		Debug.DrawLine((Vector2) transform.position + offset + new Vector2(0, -0.1f), (Vector2) transform.position + offset + new Vector2(0, -0.17f) + (Vector2.down * 0.5f), Color.white);

		if (hit && hit.collider) Debug.Log(hit.collider.name);

		if (hit && hit.collider && hit.collider.GetComponent<PlatformEffector2D>()) {
			canJump = false;
			StartCoroutine(FullDropThrough(hit.collider));
			return true;
		}

		return false;
	}

	public void Damage(float rawDamage) {

		if (!alive || invulnerable) rawDamage = 0;

		onHurt.Invoke(rawDamage);

		if (rawDamage > 0.75f) Kill();
		else Stun(rawDamage * 2f);
	}
	#endregion

	#region Helper Methods
	private void UpdateDisplay() {
		stunIndicator.shouldShow = stunTime > 0f;

		if (dashTimer <= dashCooldown - 0.2f) tr.emitting = false;

		sr.flipX = rb.velocity.x < 0;
	}

	private void GetControlInput() {
		control.Update();

		UpdateMovement();

		UpdateCrouching();

		UpdateJumping();

		UpdateDashing();
	}

	private void UpdateMovement() {
		if (control.ShouldMove()) Move(control.GetMoveMagnitude());
	}

	private void UpdateCrouching() {
		bool wasCrouching = isCrouching;
		isCrouching = !control.ShouldMove() && control.GetCrouchButton();

		if (!wasCrouching && isCrouching) sr.sprite = sprite.crouching;
		if (wasCrouching && !isCrouching) sr.sprite = sprite.main;
	}

	private void UpdateJumping() {
		bool isJumpDown = IsJumpDown();
		if (isCrouching && control.GetJumpButton() && DropThrough()) { } else
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
	}

	private void UpdateDashing() {
		bool dashButton = control.GetDashButton();
		if (!dashWasPressed && dashButton) Dash();
		dashWasPressed = dashButton;

		dashTimer -= Time.deltaTime;
	}

	private bool IsJumpDown() {
		bool wasDownFlg = jumpWasPressed;
		jumpWasPressed = control.GetJumpButton();
		return !wasDownFlg && jumpWasPressed;
	}

	public IEnumerator FullDropThrough(Collider2D col) {

		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, true);

		yield return new WaitForSeconds(0.2f);

		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, false);

	}
	#endregion

	#region Getters/Setters
	public bool IsAlive() {
		return alive;
	}

	public int GetJumpCount() {
		return maxJumpCount;
	}

	public void SetJumpCount(int maxJumpCount) {
		this.maxJumpCount = maxJumpCount;
	}

	public float GetDashSpeed() {
		return dashSpeed;
	}

	public void SetDashSpeed(float dashSpeed) {
		this.dashSpeed = dashSpeed;
	}

	public void SetInvulnerable(bool invulnerable) {
		this.invulnerable = invulnerable;
	}
	#endregion

}

#region Events
[System.Serializable]
public class OnHurtEvent : UnityEvent<float> {}

[System.Serializable]
public class OnDeathEvent : UnityEvent { }
#endregion
