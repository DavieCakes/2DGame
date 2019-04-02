using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public string enemyName = "Enemy";
    public int maxHealth = 10, damage = 1;
    private int health;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    // Update is called once per frame
    public int Attack()
    {
        return damage;
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
