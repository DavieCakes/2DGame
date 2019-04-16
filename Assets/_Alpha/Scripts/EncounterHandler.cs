using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterHandler : MonoBehaviour
{
    PlayerController pc;
    GameController gc;
    GameObject enemy;
    GameObject[] enemies;
    EnemyController ec;
    Animator anim;
    Its[] drops;

    public Canvas transition;
    public GameObject encounterUI, overWorldUI;
    public Text txtBox;
    public Button[] btnArray;

    private IEnumerator encounter;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        foreach (Button btn in btnArray)
            btn.interactable = false;
        encounterUI.SetActive(false);
    }

    public void StartEncounter(Encounters enc)
    {
        drops = enc.drops;
        enemies = enc.enemies;
        encounter = Encounter();
        StartCoroutine(encounter);
    }

    IEnumerator Encounter()
    {
        pc.pause = true;
        gc.InEncounter();
        overWorldUI.SetActive(false);

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(2f, true));
            yield return new WaitForSeconds(.5f);
        }

        encounterUI.SetActive(true);
        enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], encounterUI.transform);
        ec = enemy.GetComponent<EnemyController>();
        anim = enemy.transform.GetChild(0).GetComponent<Animator>();

        txtBox.text = ec.GetName() + " is attacking!";
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("TrigGesture");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        btnArray[0].Select();
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
        gc.InEncounter();
        encounterUI.SetActive(false);
        overWorldUI.SetActive(true);
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
        foreach (Button btn in btnArray)
        {
            btn.interactable = false;
        }
        StartCoroutine(Attack());
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
                btnArray[0].Select();
            }
        }
        else
        {
            anim.SetTrigger("TrigAttack");
            txtBox.text = ec.GetName() + " attacked!";
            yield return new WaitForSeconds(1.5f);
            if (pc.TakeDamage(ec.Attack()))
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
        StartCoroutine(Item());
    }

    IEnumerator Item()
    {
        Debug.Log("Items() was called!");
        if (pc.UseItem() != null)
        {

        }
        else
        {
            txtBox.text = "You have no items!";
        }
        btnArray[1].Select();
        yield return null;
    }

    public void BtnRun()
    {
        foreach (Button btn in btnArray)
            btn.interactable = false;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        txtBox.text += pc.GetName() + " ran away!";
        StopCoroutine(encounter);
        yield return new WaitForSeconds(1.5f);
        Destroy(enemy);
        pc.pause = false;
        gc.InEncounter();
        encounterUI.SetActive(false);
        yield return null;
    }
}
