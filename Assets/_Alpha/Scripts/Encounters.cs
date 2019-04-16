using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounters : MonoBehaviour
{
    public EncounterHandler handler;

    public GameObject[] enemies;
    PlayerController pc;
    public Its[] drops;
    public int encounterRate = 200;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (pc == null)
            Debug.Log("Did not find PlayerController on player!");
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pc.IsMoving() && Random.Range(0, encounterRate) == 0)
            {
                handler.StartEncounter(this);
            }
        }
    }

    public void clear()
    {
        drops = new Its[0];
    }
}
