using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	#region Variables
	private Rigidbody2D rigidBody;
	[SerializeField]
	private float movementSpeed = 25f;
	private Vector2 movementInput;
	#endregion

	#region Constants
	private const string AXIS_HORIZONTAL = "Horizontal";
	private const string AXIS_VERTICAL = "Vertical";
	#endregion

	private void Start() {
		this.rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		//Get the input from X and Y axis
		movementInput.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
		movementInput.y = Input.GetAxisRaw(AXIS_VERTICAL);
	}

	private void FixedUpdate() {
		//Pretty descriptive xd
		movementInput.Normalize();

		//Apply to the rigidbody the movement
		rigidBody.velocity = movementInput * movementSpeed;
	}
}
