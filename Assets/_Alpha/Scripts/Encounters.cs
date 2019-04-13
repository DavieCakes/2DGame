using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounters : MonoBehaviour
{
    public GameObject[] enemies;
    bool active, inEn = false;
    PlayerController pc;
    GameController gc;
    EnemyController ec;
    GameObject enemy;
    Animator anim;
    public Button[] btnArray;
    public Its[] drops;

    public Canvas encounterUI, transition;
    public Text txtBox;
    public int encounterRate = 200;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button btn in btnArray)
            btn.interactable = false;
        encounterUI.enabled = false;
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (pc == null)
            Debug.Log("Did not find PlayerController on player!");
        active = enemies.Length > 0;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            if (pc.IsMoving() && Random.Range(0, encounterRate) == 0)
            {
                StartCoroutine(Encounter());
            }
        }
    }

    IEnumerator Encounter()
    {
        pc.pause = true;
        inEn = true;
        gc.InEncounter();

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(1f, true));
            yield return new WaitForSeconds(1f);
        }

        encounterUI.enabled = true;
        enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], encounterUI.transform);
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
        Drops();
        Destroy(enemy);
        yield return new WaitForSeconds(3f);
        pc.pause = false;
        inEn = false;
        active = false;
        gc.InEncounter();
        encounterUI.enabled = false;
    }

    private void Drops()
    {
        txtBox.text = "";
        foreach (Its s in this.drops)
        {
            txtBox.text += pc.GetName() + " received " + s.GetItemName() + ".\n";
            pc.ReceiveDrop(s);
        }
        foreach (Its s in ec.drops)
        {
            txtBox.text += pc.GetName() + " received " + s.GetItemName() + ".\n";
            pc.ReceiveDrop(s);
        }
    }

    public void BtnAttack()
    {
        if (inEn)
        {
            foreach(Button btn in btnArray)
            {
                btn.interactable = false;
            }
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        if (pc.abilities.agility > ec.agility || (pc.abilities.agility == ec.agility && Random.Range(0, 2) == 0))
        {
            txtBox.text = pc.GetName() + " attacked!";
            if (ec.TakeDamage(pc.abilities.attack))
            {
                anim.SetTrigger("TrigAttack");
                yield return new WaitForSeconds(1.5f);
                txtBox.text += "\n" + ec.GetName() + " attacked!";
                pc.TakeDamage(ec.Attack());
                foreach (Button btn in btnArray)
                    btn.interactable = true;
            }
        }
        else
        {
            anim.SetTrigger("TrigAttack");
            txtBox.text = ec.GetName() + " attacked!";
            yield return new WaitForSeconds(1.5f);
            if(pc.TakeDamage(ec.Attack()))
            {
                txtBox.text += "\n" + pc.GetName() + " attacked!";
                ec.TakeDamage(pc.abilities.attack);
                foreach (Button btn in btnArray)
                    btn.interactable = true;
            }
        }
    }

    public void BtnItem()
    {
        if(inEn)
        {
            StartCoroutine(Item());
        }
    }

    IEnumerator Item()
    {
        Debug.Log("Items() was called!");
        if (pc.UseItem() == null)
        {

        }
        else
        {
            txtBox.text = "You have no items!";
        }
        yield return null;
    }

    public void BtnRun()
    {
        if(inEn)
        {
            StartCoroutine(Run());
        }
    }

    IEnumerator Run()
    {
        txtBox.text += pc.GetName() + " ran away!";
        Destroy(enemy);
        pc.pause = false;
        inEn = false;
        gc.InEncounter();
        encounterUI.enabled = false;
        yield return new WaitForSeconds(1.5f);
    }
}
