using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName = "Player";
    int keys = 0, health = 20;
    public float speed = 3f;
    public int attack = 1;
    bool moving;
    public bool pause;
    private Rigidbody rb;
    GameController gc;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
        moving = rb.velocity != Vector3.zero;
    }

    public bool IsMoving() { return moving; }

    public bool UseKey()
    {
        if(keys > 0)
        {
            keys--;
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            gc.GameOver();
        }
    }

    public string GetName() { return playerName; }
}
