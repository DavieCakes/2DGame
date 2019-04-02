using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName = "Player";
    public int keys = 0, health = 20;
    public float speed = 3f;
    public int attack = 1;
    bool moving;
    public bool pause;
    private Rigidbody rb;
    GameController gc;
    ItemsList items;

    private void Start()
    {
        items = new ItemsList();
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
        if (keys > 0)
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

    public string UseItem()
    {
        if(items.Length == 0)
            return "";
        return "Item";
    }

    public void ReceiveDrop(string item)
    {
        if (item.Equals("Key"))
            keys++;
        else
        {
            items.Append(item);
        }
    }
}

class ItemsList
{
    class Node
    {
        public Node next;
        public string data;

        public Node(string data)
        {
            this.data = data;
        }
    }

    Node head;
    public int Length = 0;

    public void Append(string data)
    {
        Node newNode = new Node(data);
        Length++;
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node cur = head;
            while (cur.next != null) cur = cur.next;
            cur.next = newNode;
        }
    }
    
    public string Remove(int index)
    {
        if (index >= Length)
            return "";
        Node cur = head;
        for (int i = 0; i < index; i++) cur = cur.next;
        return cur.data;
    }
}