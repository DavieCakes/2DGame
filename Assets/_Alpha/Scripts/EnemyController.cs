using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Items;
using Builders;

public class EnemyController : MonoBehaviour
{
    public string enemyName = "Enemy";
    public int maxHealth = 10,
        attack = 1,
        defense = 1,
        agility = 1;
    private int health;
    public Slider slider;
    // public List<string> gauranteedDrops;
    public List<string> drops;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        // drops = Builder.BuildRandomItemDrop();
        // foreach (string drop in gauranteedDrops) {
        //     drops.Add(Builder.BuildItem(drop));
        // }
    }

    // Update is called once per frame
    public int Attack()
    {
        return attack;
    }

    public bool TakeDamage(int d)
    {
        health -= d;
        slider.value = health;
        return (health > 0);
    }

    public int GetHealth() { return health; }

    public string GetName() { return enemyName; }
}
