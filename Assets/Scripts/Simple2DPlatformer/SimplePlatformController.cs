using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatformController : MonoBehaviour
{

    private bool facingRight = true;
    private bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck; // line cast down to check if grouneded
    // Start is called before the first frame update

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    
    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Start()
    {
        // get and store references to componenets
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); // bit right shoft mask only to ground layer, no other layers

        // when jump pressed and on the ground
        if (Input.GetButtonDown("Jump") && grounded) {
            jump = true;
        }
    }

    // physics loops occur in fixed update, separate from Update which is depended on frame updates
    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal"); // GetAxis returns vals [-1, 1]

        // sets speed of animator, linking input to animation/sprite movement
        anim.SetFloat("Speed", Mathf.Abs(h)); // use pos float to set speed regardless of direction
        if(h*rb2d.velocity.x < maxSpeed) { // 
            rb2d.AddForce(Vector2.right * h * moveForce); // add force vector equal to h*moveeForce: [-1, 1]*365, 
            // Vector * Scalar => Vector
            // Vector2.right == lam(x, y) || Vector(1, 0)
        }
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        // flip sprite based on which direction moving 
        if(h > 0 && !facingRight) {
            flip();
        } else if (h < 0 && facingRight) {
            flip();
        }

        if (jump) {
            // add burst of vertical force
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false; // can't double jump
        }
    }
}
