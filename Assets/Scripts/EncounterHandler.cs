using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Items;
using Builders;

public class EncounterHandler : MonoBehaviour
{
    Encounters enc;
    PlayerController pc;
    GameController gc;
    GameObject enemy;
    GameObject[] enemies;
    EnemyController ec;
    Animator anim;
    List<Item> drops = new List<Item>();
    public bool boss = false;

    public Canvas transition;
    public GameObject encounterUI, overWorldUI, gameOverUI;
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
        this.enc = enc;
        
        foreach (var item in drops)
            Debug.Log(item.ToString());
        enemies = enc.enemies;
        encounter = Encounter();
        StartCoroutine(encounter);
    }

    public void StartEncounter(MimicBoss mb, int i)
    {
        boss = true;
        this.enc = mb;
        enemies = mb.enemies;
        encounter = BossFight();
        StartCoroutine(encounter);
    }

    IEnumerator Encounter()
    {
        Debug.Log("Encounter Started");
        pc.pause = true;
        gc.InEncounter();
        // overWorldUI.SetActive(false);

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(2f, true));
            yield return new WaitForSeconds(.5f);
        }

        SetUp();
        
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("TrigGesture");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        btnArray[0].Select();
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
        RegWin();
    }

    IEnumerator BossFight()
    {
        Debug.Log("Boss Fight Started");
        pc.pause = true;
        //overWorldUI.SetActive(false);

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(1f, true));
            yield return new WaitForSeconds(1f);
        }

        SetUp();

        txtBox.text = ec.GetName() + " is attacking!";
        anim.SetTrigger("Fight");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        btnArray[0].Select();
        while (ec.GetHealth() > 0)
        {
            yield return null;
        }
        txtBox.text += "\n" + ec.GetName() + " was defeated!";
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Destroy(enemy);
        yield return new WaitForSeconds(1f);
        gameOverUI.SetActive(true);
        while (true)
        {
            if (Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;
        }
    }

    private void SetUp()
    {
        encounterUI.SetActive(true);
        enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], encounterUI.transform);
        ec = enemy.GetComponent<EnemyController>();
        anim = enemy.transform.GetChild(0).GetComponent<Animator>();

        txtBox.text = ec.GetName() + " is attacking!";
    }

    private void RegWin()
    {
        enc.clear();
        enc = null;
        pc.pause = false;
        gc.InEncounter();
        encounterUI.SetActive(false);
        overWorldUI.SetActive(true);
    }

    private void Drops()
    {
        drops.AddRange(Builder.BuildRandomItemDrop());
        txtBox.text = "";
        foreach (Item s in this.drops)
        {
            txtBox.text += pc.GetName() + " received " + s.GetDisplayName() + ".\n";
            pc.ReceiveDrop(s);
        }
        if (enc.key) {
            txtBox.text += pc.GetName() + " recieved a key!\n";
            pc.RecieveKey();
        }
        // foreach (string s in ec.drops)
        // {
        //     Debug.Log(s);
        //     Item temp = Builder.BuildEquipment(s);
        //     txtBox.text += pc.GetName() + " received " + temp.GetItemName() + ".\n";
        //     pc.ReceiveDrop(temp);
        // }
        drops.Clear();
    }

    public void BtnAttack()
    {
        foreach (Button btn in btnArray)
            btn.interactable = false;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        if (pc.playerModel.abilities.Agility.Value > ec.agility || (pc.playerModel.abilities.Agility.Value == ec.agility && Random.Range(0, 2) == 0))
        {
            txtBox.text = pc.GetName() + " attacked!";
            if (ec.TakeDamage(pc.playerModel.abilities.Attack.Value))
            {
                anim.SetTrigger(boss ? "Attacking" : "TrigAttack");
                yield return new WaitForSeconds(1.5f);
                txtBox.text += "\n" + ec.GetName() + " attacked!";
                if(!pc.TakeDamage(ec.Attack()))
                {
                    gc.GameOver();
                }
                foreach (Button btn in btnArray)
                    btn.interactable = true;
            }
        }
        else
        {
            anim.SetTrigger(boss ? "Attacking" : "TrigAttack");
            txtBox.text = ec.GetName() + " attacked!";
            if (pc.TakeDamage(ec.Attack()))
            {
                yield return new WaitForSeconds(1.5f);
                txtBox.text += "\n" + pc.GetName() + " attacked!";
                if(ec.TakeDamage(pc.playerModel.abilities.Attack.Value))
                    foreach (Button btn in btnArray)
                        btn.interactable = true;
            }
            else
            {
                gc.GameOver();
            }
        }
        btnArray[0].Select();
    }

    public void BtnItem()
    {
        StartCoroutine(Item());
    }

    IEnumerator Item()
    {
        Debug.Log("Items() was called!");
        if (pc.UsePotion())
        {
            txtBox.text += "\nYou used a potion!";
        }
        else
        {
            txtBox.text += "\nYou have no potions!\n";
        }
        btnArray[1].Select();
        yield return null;
    }

    public void BtnRun()
    {
        if (!boss)
        {
            foreach (Button btn in btnArray)
                btn.interactable = false;
            StartCoroutine(Run());
        }
        else
            txtBox.text = "As you try to run,\nthe Mimic chases you!\nYou cannot get away!";
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
