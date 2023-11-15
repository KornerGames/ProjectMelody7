// -----------------------------------------------------------------------------------------
// using classes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -----------------------------------------------------------------------------------------
// player movement class
public class PlayerMovement : MonoBehaviour
{
    // static public members
    public static PlayerMovement instance;

    // -----------------------------------------------------------------------------------------
    // public members
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    // -----------------------------------------------------------------------------------------
    // private members
    private Vector2 movement;

    // -----------------------------------------------------------------------------------------
    // awake method to initialisation
    void Awake()
    {
        instance = this;
    }

    // -----------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // update members to restrict movement to 8 directions
        movement.x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        movement.y = Mathf.Round(Input.GetAxisRaw("Vertical"));

        // normalize the movement vector to prevent diagonal movement being faster than horizontal/vertical movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
    }

    // -----------------------------------------------------------------------------------------
    // fixed update methode
    void FixedUpdate()
    {
        // move the rigidbody using the restricted movement vector and the move speed
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // -----------------------------------------------------------------------------------------
 
}
