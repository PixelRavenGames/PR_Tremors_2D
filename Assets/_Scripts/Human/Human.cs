using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Human : MonoBehaviour {

	[Header("Controller")]
	public int controllerNum = 0;

	private IHumanController control;

	[Header("Settings")]
	[SerializeField]
	private float jumpHeight = 3;
	[SerializeField]
	private float maxJumpHeld = 1f;
	[SerializeField]
	private float fallModifier = 1f;
	[SerializeField]
	private float speed = 1f;

	// TODO [Header("Events")]

	private float jumpHeldTime = 0;
	private bool jumpWasPressed = false;

	private bool alive = true;

	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();

		// TMP
		ChangeController(new PlayerHumanController(controllerNum));
		// ---
	}

	void Update() {
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallModifier - 1) * Time.deltaTime;
		}

		control.Update();
		if (control.ShouldMove()) Move(control.GetMoveMagnitude());

		if (control.GetJumpButton()) {
			if (jumpHeldTime < maxJumpHeld) {
				jumpHeldTime += Time.deltaTime;
				Jump(jumpHeldTime);
			}
		} else {
			jumpHeldTime = 0;
		}

		//if (jumpHeldTime > 0) {
		//	if (IsJumpUp()) {
		//		jumpHeldTime = 0;
		//	} else {
		//		jumpHeldTime += Time.deltaTime;
		//		if (jumpHeldTime >= maxJumpHeld) jumpHeldTime = 0;
		//		Jump(jumpHeldTime);
		//	}
		//} else if (IsJumpDown()) {
		//	jumpHeldTime += Time.deltaTime;
		//}
		
	}

	public void Move(float magnitude) {
		rb.velocity = new Vector2(magnitude * speed * Time.deltaTime, rb.velocity.y);
	}

	public void Jump(float magnitude) {
		if (jumpHeight <= 0) return;

		float percentage = Mathf.Clamp01(magnitude / maxJumpHeld);
		float targetHeight = (percentage * jumpHeight * 0.5f);

		float velocity = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * targetHeight); 

		rb.velocity = new Vector2(rb.velocity.x, (float.IsNaN(velocity) ? 0 : velocity));
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
		return (control = controller) != null;
	}

	public void Kill() {
		alive = false;
	}

	public bool IsAlive() {
		return alive;
	}

}
