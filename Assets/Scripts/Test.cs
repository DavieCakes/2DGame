using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;


public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Creature c = new Creature();
        Item i = new Item("sword", 1);
        StatModifier mod = new StatModifier(5f, StatModType.Flat, CreatureAttributeType.Dexterity, i);
        i.AddStatMod(mod);
        Debug.Log("Test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
