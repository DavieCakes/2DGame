using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimicBoss : MonoBehaviour
{
    public Canvas transition, encounterUI;

    GameObject enemy;
    EnemyController ec;
    GameController gc;
    Animator anim;
    float delayCount = 0f, delay = 10f;
    bool trig = false;
    public Text txtBox;
    public Button[] btnArray;
    public GameObject mimic;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E))
                StartCoroutine(BossFight(other.GetComponent<PlayerController>()));

            if(delayCount < delay)
                delayCount += Time.deltaTime;
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
        delayCount = 0;
        trig = false;
    }

    IEnumerator BossFight(PlayerController pc)
    {
        pc.pause = true;
        gc.InEncounter();

        anim.SetTrigger("Awakened");
        yield return new WaitForSeconds(2f);

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(1f, true));
            yield return new WaitForSeconds(1f);
        }

        encounterUI.enabled = true;
        enemy = Instantiate(mimic, encounterUI.transform);
        ec = enemy.GetComponent<EnemyController>();
        anim = enemy.transform.GetChild(0).GetComponent<Animator>();

        txtBox.text = ec.GetName() + " is attacking!";
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("TrigGesture");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        while (ec.GetHealth() > 0)
        {
            yield return null;
        }
        txtBox.text += "\n" + ec.GetName() + " was defeated!";
        anim.SetTrigger("TrigDeath");
        yield return new WaitForSeconds(2f);
        Destroy(enemy);
        yield return new WaitForSeconds(3f);
        pc.pause = false;
        gc.InEncounter();
        encounterUI.enabled = false;
    }
}
