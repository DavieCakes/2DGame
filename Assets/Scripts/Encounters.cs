﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Items;

public class Encounters : MonoBehaviour
{
    public EncounterHandler handler;
    public GameObject[] enemies;
    protected PlayerController pc;
    public bool key = false;
    // public List<string> drops;

    public int encounterRate = 200;
    public float delay = 5f, waitTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (pc == null)
            Debug.Log("Did not find PlayerController on player!");
        if (enemies.Length == 0)
            encounterRate = 0;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pc.IsMoving())
            {
                if (delay >= waitTime)
                {
                    if (Random.Range(0, encounterRate) == 0)
                        handler.StartEncounter(this);
                }
                else
                    delay += Time.deltaTime;
            }
        }
    }

    public void clear()
    {
        this.key = false;
        delay = 0f;
    }
}
