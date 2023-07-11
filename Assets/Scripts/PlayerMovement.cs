using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public Rigidbody2D rigidBody;
    public float movementVelocity = 25f;
    Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //Get the input from X and Y axis
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //Pretty descriptive xd
        movementInput.Normalize();

        //Apply to the rigidbody the movement
        rigidBody.velocity = movementInput * movementVelocity;
    }
}
