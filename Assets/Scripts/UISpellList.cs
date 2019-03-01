using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Creatures;

public class UISpellList : MonoBehaviour
{
    public GameObject player;
    private Creature playerModel;
    public List<GameObject> btns;
    public GameObject parent;
    public GameObject SpellButton;
    
    // Start is called before the first frame update
    void Start()
    {
        playerModel = player.GetComponent<Player>().playerModel;
        foreach (string spell in playerModel.spells) {
            GameObject btn = Instantiate(SpellButton) as GameObject;
            btn.transform.SetParent(parent.transform, false);
            btn.GetComponentInChildren<Text>().text = spell;
            btns.Add(btn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
