using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Abilities
{

    public int bHealth = 20,
        maxHealth,
        bAttack = 1,
        bDefense = 1,
        bAgility = 1;

    public int health,
        attack,
        defense,
        agility;

    public void SetUp()
    {
        maxHealth = bHealth;
        health = bHealth;
        attack = bAttack;
        defense = bDefense;
        agility = bAgility;
    }
}

public class PlayerController : MonoBehaviour
{
    public Equipment[] equip = new Equipment[4];

    public Abilities abilities;

    public string playerName = "Player";
    public int keys = 0, gold = 0;
    public float moveSpeed = 2.5f;
    bool moving = false;
    public bool pause;
    private Rigidbody rb;
    GameController gc;
    ItemsList items;
    Animator anim;
    int cDir = 0, pDir = 0;

    public Text keyCount, goldCount;
    public Slider health;

    private void Start()
    {
        abilities.SetUp();
        foreach (Equipment e in equip)
        {
            if(e != null)
                e.AbilitiesBoost(this);
        }
        UpdateUI();
        items = new ItemsList();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = transform.GetChild(1).GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float mHor = 0f;
        float mVer = 0f;
        if (!pause)
        {
            mHor = Input.GetAxisRaw("Horizontal");
            mVer = Input.GetAxisRaw("Vertical");
            
            Vector3 move = new Vector3(mHor, 0, mVer);

            rb.velocity = move * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        moving = rb.velocity != Vector3.zero;
        
        if (moving)
        {
            switch (Input.GetAxisRaw("Vertical"))
            {
                case 1:
                    if (Input.GetAxisRaw("Horizontal") == 0)
                        cDir = 1;
                    break;
                case -1:
                    if (Input.GetAxisRaw("Horizontal") == 0)
                        cDir = 3;
                    break;
                case 0:
                    switch (Input.GetAxisRaw("Horizontal"))
                    {
                        case 1:
                            cDir = 2;
                            break;
                        case -1:
                            cDir = 4;
                            break;
                    }
                    break;
            }
        }
        else
            cDir = 0;

        if(cDir != pDir)
        {
            pDir = cDir;
            anim.SetInteger("Direction", cDir);
            anim.SetTrigger("Change");
        }
    }

    public bool IsMoving() { return moving; }

    public bool UseKey()
    {
        if (keys > 0)
        {
            keys--;
            UpdateUI();
            return true;
        }
        UpdateUI();
        return false;
    }

    public bool TakeDamage(int damage)
    {
        abilities.health -= damage;
        UpdateUI();
        if (abilities.health <= 0)
        {
            abilities.health = 0;
            gc.GameOver();
            return false;
        }
        return true;
    }

    public string GetName() { return playerName; }

    public Its UseItem()
    {
        if(items.Length == 0)
            return null;
        return items.Remove(0);
    }

    public void ReceiveDrop(Its item)
    {
        if (item.Equals("Key"))
            keys++;
        else
        {
            items.Append(item);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        health.maxValue = abilities.maxHealth;
        health.value = abilities.health;
        keyCount.text = "KEYS: " + keys.ToString();
        goldCount.text = "GOLD: " + gold.ToString();
    }
}

class ItemsList
{
    class Node
    {
        public Node next;
        public Its data;

        public Node(Its data)
        {
            this.data = data;
        }
    }

    Node head;
    public int Length = 0;

    public void Append(Its data)
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
    
    public Its Remove(int index)
    {
        if (index >= Length)
            return null;
        Node cur = head;
        for (int i = 0; i < index; i++) cur = cur.next;
        return cur.data;
    }
}