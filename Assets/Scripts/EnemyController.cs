using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Items;
using Builders;
using Creatures;

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
    public Creature enemyModel;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        enemyModel = new Creature(maxHealth, agility, defense, attack, enemyName);
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
        enemyModel.TakeDamage(d);
        slider.value = enemyModel.abilities.Health.Value;
        return (enemyModel.isDead());
    }

    public int GetHealth() { return enemyModel.abilities.Health.Value; }

    public string GetName() { return enemyModel.name; }
}
