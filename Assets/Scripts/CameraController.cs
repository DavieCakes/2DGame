using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb2d;
    private Transform tr;
    private Vector3 pos;
    private Vector3 old;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    private Vector2 inputVector;
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        pos = tr.position;
    }

    void FixedUpdate() {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // <- Not Used for a Camera -> //
        // sets speed of animator, linking input to animation/sprite movement
        // anim.SetFloat("Speed", Mathf.Abs(inputVelocity)); // use pos float to set speed regardless of direction
        if(inputVector.magnitude*rb2d.velocity.magnitude < maxSpeed) { // 
            rb2d.AddForce(moveForce * inputVector); // add force vector equal to Vin*moveForce: [-1, 1]*365, 
            // Vector * Scalar => Vector
            // Vector2.right == lam(x, y) || Vector(1, 0)
        }
        // if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
        //     rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        // }
    }
}
