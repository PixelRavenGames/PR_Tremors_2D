using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bot Settings", menuName = "Bots/Bot Settings", order = 1)]
public class BotSettings : ScriptableObject {
	[Header("Idle")]
	[Tooltip("The minimum distance the bot must be from the worm to idle")]
	public float minimumIdleDistance = 5f;
	[Tooltip("The chance to wander every frame that bot is idling")]
	public float idleEndChance = 30f;

	[Header("Wander")]
	[Tooltip("The amount of seconds until bots change wandering paths")]
	public float wanderUpdateDelay = 3f;
	[Tooltip("The chance to stop wandering and switch to either idle or run")]
	public float wanderEndChance = 30f;
	[Tooltip("The minimum speed needed to jump while wandering")]
	public float wanderJumpMinimumSpeed = 0.7f;
	[Tooltip("The chance to continue holding a jump")]
	public float wanderContinueJumpChance = 90f;

	[Header("Running")]
	[Tooltip("The maximum distance from the worm to run")]
	public float runDistance = 1f;
	[Tooltip("The distance from the worm verticly to force a jump while running")]
	public float runDistanceToJump = 0.25f;
	[Tooltip("The the chance to allow the bot to hold the jump")]
	public float runJumpChance = 60f;

	[Header("Movement")]
	[Tooltip("The minimum movement needed to move")]
	public float movementDeadzone = 0f;
}
