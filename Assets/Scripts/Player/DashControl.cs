using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

public class DashControl : MonoBehaviour
{
	public float dashSpeed;

	public float dashCooldown;

	public float dashDuration;

	public PlayerMovement movRef;

	public SpartanTimer DashDurationTimer => dashDurationTimer;

	private SpartanTimer dashTimer;
	private SpartanTimer dashDurationTimer;

	//This enum is just for readability
	public enum ExternalDashResults
	{
		INTERNAL = 0,
		EXTERNAL_SUCCESS = 1,
		EXTERNAL_FAILURE = -1
	};

	private void Start()
	{
		movRef = GetComponent<PlayerMovement>();
		//Instantiate 2 timers, one for the cooldown, and one for the dashing time
		dashTimer = new SpartanTimer(TimeMode.Framed);
		dashDurationTimer = new SpartanTimer(TimeMode.Fixed);
	}

	private void Update()
	{
		HandleInput();
	}

	private void FixedUpdate()
	{
		if (dashDurationTimer.Started)
			Dash();
	}

	private void HandleInput()
	{
		if (SentDashSignal() && AbleToDash(this.dashTimer, this.dashDurationTimer, this.dashCooldown))
			dashDurationTimer.Start();
	}

	private bool SentDashSignal()
	{
		return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown(GamepadButton.B.ToString());
	}

	private void Dash()
	{
		this.Dash(this.dashSpeed, this.dashDuration, this.dashCooldown, ref this.dashTimer, ref this.dashDurationTimer);
	}

	public bool Dash(float dashSpeed, float dashDuration, float cooldown, ref SpartanTimer cooldownTimer, ref SpartanTimer durationTimer)
	{
		ExternalDashResults dashTriggerState = HandleExternalTriggers(cooldownTimer, ref durationTimer, cooldown);
		Debug.Log($"Dash result: {dashTriggerState}");
		//The trigger was external, but, we were not able to dash (see AbleToDash func)
		if (dashTriggerState == ExternalDashResults.EXTERNAL_FAILURE) return false;

		float currTime = durationTimer.GetCurrentTime(TimeScaleMode.Seconds);
		if (currTime >= dashDuration)
		{
			durationTimer.Stop();
			//this.dashedAirbone = !movRef.Grounded;
			cooldownTimer.Reset();
			this.movRef.CanStir = true;
			return false;
		}
		Vector3 forceDirection = new Vector3(this.movRef.PrevMovementInput.x, 0f, this.movRef.PrevMovementInput.y);
		const float DASH_BOOST_FACTOR = 10f;
		movRef.RigidBody.AddForce(forceDirection * dashSpeed * DASH_BOOST_FACTOR, ForceMode2D.Force);
		this.movRef.CanStir = false;
		return true;
	}

	public bool AbleToDash(SpartanTimer cooldownTimer, SpartanTimer durationTimer, float cooldown)
	{
		if (!cooldownTimer.Started) return true;
		float timePassed = cooldownTimer.GetCurrentTime(TimeScaleMode.Seconds);
		Debug.Log($"Cooldown: {timePassed}");
		return (timePassed >= cooldown && !durationTimer.Started);
	}

	private ExternalDashResults HandleExternalTriggers(SpartanTimer cooldownTimer, ref SpartanTimer durationTimer, float cooldown)
	{
		//So, if the dash timer has not been started
		if (!durationTimer.Started)
		{
			//We verify if we can dash or not
			if (!this.AbleToDash(cooldownTimer, durationTimer, cooldown))
				return ExternalDashResults.EXTERNAL_FAILURE;

			//We start the dash timer
			durationTimer.Start();
			return ExternalDashResults.EXTERNAL_SUCCESS;
		}
		return ExternalDashResults.INTERNAL;
	}
}
