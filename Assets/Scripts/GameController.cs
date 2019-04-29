using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverUI, pauseUI, overWorldUI, inventoryUI;
    public GameObject player;
    private PlayerController pc;
    bool inEncounter;
    AudioSource aud;
    public AudioClip[] clips;
    
    // Start is called before the first frame update
    void Awake()
    {
        gameOverUI.SetActive(false);
        pc = player.GetComponent<PlayerController>();
        pauseUI.SetActive(false);
        inventoryUI.SetActive(false);
        aud = GetComponent<AudioSource>();
        if (clips.Length > 0)
        {
            aud.clip = clips[0];
            aud.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!inEncounter && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        // ADDED, opens Inventory UI from key 'i'
        if (!inEncounter && Input.GetKeyDown(KeyCode.I)) 
        {
            this.inventoryUI.SetActive(!this.inventoryUI.activeSelf);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over called");
        gameOverUI.SetActive(true);
        gameOverUI.transform.GetChild(1).GetComponent<Text>().text = "Game Over!";
        StartCoroutine(GameOverLoop());
    }

    IEnumerator GameOverLoop()
    {
        Debug.Log("Player Died!");
        gameOverUI.SetActive(true);
        while (true)
        {
            if (Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;
        }
    }

    public void Pause()
    {
        if (!inEncounter)
        {
            pc.pause = !pc.pause;
            pauseUI.SetActive(pc.pause);
            pauseUI.transform.GetChild(1).GetChild(1).GetComponent<Button>().Select();
            overWorldUI.SetActive(!pc.pause);
            aud.pitch = (pc.pause) ? .6f : 1f;
        }
    }

    public void InEncounter()
    {
        inEncounter = !inEncounter;
        if (clips.Length > 1)
        {
            aud.clip = (inEncounter ? clips[1] : clips[0]);
            aud.pitch = (inEncounter ? .85f : 1f);
            aud.Play();
        }
    }
    
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
