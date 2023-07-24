using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection
{
	None = 0,
	UP,
	DOWN,
	LEFT,
	RIGHT,
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DashControl))]
public class PlayerMovement : MonoBehaviour {

	#region Variables
	public bool IsMoving => this.movementInput != Vector2.zero;
	public Rigidbody2D RigidBody { get; private set; }
	public Vector2 MovementInput => this.movementInput;
	public Vector2 PrevMovementInput { get; private set; }
	public bool CanStir { get; set; }
	public MovementDirection CurrentDirection { get; private set; }
	private Vector2 movementInput;
	private Animator animator;
	[SerializeField]
	private float movementSpeed = 25f;
	private SpriteRenderer renderer;
	private DashControl dashControl;
	#endregion

	#region Constants
	private const string AXIS_HORIZONTAL = "Horizontal";
	private const string AXIS_VERTICAL = "Vertical";
	private const string ANIM_DIRECTION = "Direction";
	#endregion

	private void Start() {
		this.RigidBody = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.renderer = GetComponent<SpriteRenderer>();
		this.dashControl = GetComponent<DashControl>();
		this.CanStir = true;
	}

	private void Update() {
		this.HandleInput();
		this.AnimationValueUpdate();
	}

	private void FixedUpdate() {
		//Pretty descriptive xd
		movementInput.Normalize();
		//Apply to the rigidbody the movement
		RigidBody.velocity = movementInput * movementSpeed;
		//Update last input
		if (this.movementInput != Vector2.zero)
		{
			this.PrevMovementInput = movementInput;
		}
	}

	private void HandleInput()
	{
		if (!this.CanStir) return;
        //Get the input from X and Y axis
        movementInput.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        movementInput.y = Input.GetAxisRaw(AXIS_VERTICAL);
        this.CurrentDirection = this.CalculateDominantDirection();
    }

	private void AnimationValueUpdate()
	{
		//Set the variables for the animations
		bool shouldFlipX = movementInput.x > 0f && movementInput.y == 0f;
		bool shouldFlipY = movementInput.y < 0f;

        animator.SetInteger(ANIM_DIRECTION, (int)this.CurrentDirection);
		this.renderer.flipX = this.CurrentDirection == MovementDirection.RIGHT;
	}

	private MovementDirection CalculateDominantDirection()
	{
		//If the player's input X > 0 Right, X < 0 left only if Y == 0
		bool movingHorizontally = this.movementInput.x != 0f;
		MovementDirection result = MovementDirection.None;
		//Handle horizontal movement
		result = this.movementInput.x switch {
			> 0f => MovementDirection.RIGHT,
			< 0f => MovementDirection.LEFT,
			_ => result
		};
		//Handle vertical movement
		result = this.movementInput.y switch
		{
			> 0f when !movingHorizontally => MovementDirection.UP,
			< 0f when !movingHorizontally => MovementDirection.DOWN,
			_ => result
		};
		return result;
	}

}
