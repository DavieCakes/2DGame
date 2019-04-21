using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Items;
using Creatures;
using Builders;

// [System.Serializable]
// public class Abilities
// {

//     public int bHealth = 20,
//         maxHealth,
//         bAttack = 1,
//         bDefense = 1,
//         bAgility = 1;

//     public int health,
//         attack,
//         defense,
//         agility;

    //     public int boostedHealth, boostedAttack, boostedDefense, boostedAgility;

    //     public void SetUp()
    //     {
    //         maxHealth = bHealth;
    //         health = bHealth;
    //         attack = bAttack;
    //         defense = bDefense;
    //         agility = bAgility;
    //     }
// }
public class PlayerController : MonoBehaviour
{

    /* Player Starting Values */
    public string PlayerName = "Player Name";
    public List<string> StartingEquipment = new List<string>();
    public int StartingHealth = 20;
    public int StartingAgility = 1;

    public int StartingDefense = 1;

    public int StartingAttack = 1;
    public int StartingKeys = 0, StartingGold = 0;
    public Creature playerModel;

    /* Controller Init Variables */

    public float moveSpeed = 2.5f;
    bool moving = false;
    public bool pause;
    
    private Rigidbody rb;
    GameController gc;
    // ItemsList items;
    Animator anim;
    int cDir = 0, pDir = 0;

    public Text keyCount, goldCount;
    public Slider health;


    void Start()
    {
        InitPlayerModel();
        UpdateUI();
        // items = new ItemsList();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = transform.GetChild(1).GetComponent<Animator>();
    }

    private void InitPlayerModel()
    {
        Debug.Log("test");
        playerModel = new Creature(StartingHealth, StartingAgility, StartingDefense, StartingAttack, PlayerName);
        playerModel.inventory.AddGold(StartingGold);
        playerModel.inventory.AddKeys(StartingKeys);
        foreach (string itemName in StartingEquipment)
        {
            playerModel.Equip(Builder.BuildEquipment(itemName));
        }
        Debug.Log("Adding Equipment");
        List<Equipment> items = Builder.BuildAllEquipment();
        Debug.Log(items.Count);
        foreach (Equipment item in items) {
            Debug.Log("test");
            playerModel.Equip(item);
        }
        playerModel.inventory.AddPotions(20);
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

            if (moving)
            {
                switch (mVer)
                {
                    case 1:
                        if (mHor == 0 || anim.GetInteger("Direction") == 0)
                            cDir = 1;
                        break;
                    case -1:
                        if (mHor == 0 || anim.GetInteger("Direction") == 0)
                            cDir = 3;
                        break;
                    case 0:
                        switch (mHor)
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

            if (cDir != pDir)
            {
                pDir = cDir;
                anim.SetInteger("Direction", cDir);
                anim.SetTrigger("Change");
            }
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
        if (playerModel.inventory.keys > 0)
        {
            playerModel.inventory.RemoveKey();
            UpdateUI();
            return true;
        }
        UpdateUI();
        return false;
    }

    public bool TakeDamage(int damage)
    {
        playerModel.TakeDamage(damage);
        // abilities.health -= damage;
        UpdateUI();
        // if (abilities.health <= 0)
        if (playerModel.isDead())
        {
            //     abilities.health = 0;
            gc.GameOver();
            return false;
        }
        return true;
    }

    public string GetName() { return playerModel.name; }

    public Item UseItem()
    {
        // if(items.Length == 0)
        //     return null;
        // return items.Remove(0);
        return null;
    }

    public void ReceiveDrop(Item item)
    {
        playerModel.PickUp(item);
        UpdateUI();
    }

    public void UpdateUI()
    {
        health.maxValue = playerModel.abilities.Health.maxHealth;
        health.value = playerModel.abilities.Health.Value;
        keyCount.text = "KEYS: " + playerModel.inventory.keys.ToString();
        goldCount.text = "GOLD: " + playerModel.inventory.gold.ToString();
    }
}

// class ItemsList : List<Item> {

// }

// class ItemsList : List<Item>
// {
//     class Node
//     {
//         public Node next;
//         public Its data;

//         public Node(Its data)
//         {
//             this.data = data;
//         }
//     }

//     Node head;
//     public int Length = 0;

//     public void Append(Item data)
//     {
//         Node newNode = new Node(data);
//         Length++;
//         if (head == null)
//         {
//             head = newNode;
//         }
//         else
//         {
//             Node cur = head;
//             while (cur.next != null) cur = cur.next;
//             cur.next = newNode;
//         }
//     }

//     public Item Remove(int index)
//     {
//         if (index >= Length)
//             return null;
//         Node cur = head;
//         for (int i = 0; i < index; i++) cur = cur.next;
//         return cur.data;
//     }
// }