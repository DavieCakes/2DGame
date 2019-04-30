using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Items;
using Builders;
using Creatures;

public class EncounterHandler : MonoBehaviour
{
    Encounters enc;
    PlayerController pc;
    GameController gc;
    GameObject enemy;
    GameObject[] enemies;
    EnemyController ec;
    Animator enemyAnim;
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
        enemyAnim.SetTrigger("TrigGesture");
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
        enemyAnim.SetTrigger("TrigDeath");
        yield return new WaitForSeconds(2f);
        Drops();
        Destroy(enemy);
        yield return new WaitForSeconds(3f);
        RegWin();
    }

    IEnumerator BossFight()
    {
        Debug.Log("Boss Fight Started");
        overWorldUI.SetActive(false);

        if (transition != null)
        {
            StartCoroutine(Instantiate(transition).transform.GetChild(0)
                .GetComponent<Transition>().Up(1f, true));
            yield return new WaitForSeconds(1f);
        }

        SetUp();

        txtBox.text = ec.GetName() + " is attacking!";
        enemyAnim.SetTrigger("Fight");
        yield return new WaitForSeconds(1f);
        foreach (Button btn in btnArray)
            btn.interactable = true;
        btnArray[0].Select();
        while (ec.GetHealth() > 0)
        {
            yield return null;
        }
        txtBox.text += "\n" + ec.GetName() + " was defeated!";
        enemyAnim.SetTrigger("Death");
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
        enemyAnim = enemy.transform.GetChild(0).GetComponent<Animator>();

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
        Creature c1 = (pc.playerModel.Initiative() > ec.enemyModel.Initiative()) ? pc.playerModel : ec.enemyModel;

        // Object reference equality **should** compare the object hashes with equality checks
        // i.e. true if pointing to same object in memory
        Creature c2 = (c1 == pc.playerModel) ? ec.enemyModel : pc.playerModel;

        if (c1 == ec.enemyModel) enemyAnim.SetTrigger(boss ? "Attacking" : "TrigAttack");
        yield return new WaitForSeconds(1.5f);
        txtBox.text = c1.name + " is attacking!";
        txtBox.text += (c1.Attack(c2)) ? "\nHit!" : "\nMissed!";
        txtBox.text = "\n" + c2.name + " is attacking!";
        txtBox.text += (c2.Attack(c1)) ? "\nHit!" : "\nMissed!";

        if (c2.isDead()) {
            if (c2 == pc.playerModel) {
                gc.GameOver();
            }
        }

        foreach (Button btn in btnArray) btn.interactable = true;


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
