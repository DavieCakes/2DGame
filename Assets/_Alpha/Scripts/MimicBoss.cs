using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MimicBoss : Encounters
{
    public Canvas transition;
    GameController gc;
    Animator anim;
    bool trig = false;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        delay = 0f; waitTime = 10f;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Awaken());
            }

            if(delay < waitTime)
                delay += Time.deltaTime;
            else if(!trig)
            {
                trig = true;
                StartCoroutine(Anim());
            }
        }
    }

    IEnumerator Anim()
    {
        anim.SetTrigger("Peak");
        yield return new WaitForSeconds(1f);
        delay = 0;
        trig = false;
    }

    IEnumerator Awaken()
    {
        pc.pause = true;
        gc.InEncounter();

        anim.SetTrigger("Awakened");
        yield return new WaitForSeconds(2.5f);
        base.handler.StartEncounter(this,0);
    }
}
