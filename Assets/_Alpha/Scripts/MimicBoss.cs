using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MimicBoss : MonoBehaviour
{
    public Canvas transition, encounterUI;

    GameObject enemy;
    EnemyController ec;
    GameController gc;
    PlayerController pc;
    Animator anim;
    float delayCount = 0f, delay = 10f;
    bool trig = false, inEn = false;
    public Text txtBox;
    public Button[] btnArray;
    public GameObject mimic;
    public Canvas GameOverUI;

    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(!inEn && other.CompareTag("Player"))
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
        inEn = true;
        pc.pause = true;
        gc.InEncounter();

        anim.SetTrigger("Awakened");
        yield return new WaitForSeconds(2.5f);

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
        anim.SetTrigger("Fight");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        while (ec.GetHealth() > 0)
        {
            yield return null;
        }
        txtBox.text += "\n" + ec.GetName() + " was defeated!";
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Destroy(enemy);

        GameOverUI.enabled = true;
        while(true)
        {
            if (Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;
        }
    }

    public void BtnAttack()
    {
        if (inEn)
        {
            foreach (Button btn in btnArray)
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
                anim.SetTrigger("Attacking");
                yield return new WaitForSeconds(1.5f);
                txtBox.text += "\n" + ec.GetName() + " attacked!";
                pc.TakeDamage(ec.Attack());
                foreach (Button btn in btnArray)
                    btn.interactable = true;
            }
        }
        else
        {
            anim.SetTrigger("Attacking");
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
        if(inEn)
            StartCoroutine(Item());
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
        if (inEn)
            txtBox.text = "As you try to run,\nthe Mimic chases you!\nYou cannot get away!";
    }
}
