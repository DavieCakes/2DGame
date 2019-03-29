using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3f;
    public bool pause;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (!pause)
        {
            float mHor = Input.GetAxis("Horizontal");
            float mVer = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(mHor, 0, mVer);

            rb.velocity = move * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
