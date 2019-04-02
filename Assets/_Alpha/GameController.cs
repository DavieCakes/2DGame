using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    private PlayerController pc;
    bool inEncounter;

    public Canvas PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        PauseUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GameOver()
    {

    }

    public void Pause()
    {
        if (!inEncounter)
        {
            pc.pause = !pc.pause;
            PauseUI.enabled = pc.pause;
        }
    }

    public void InEncounter() { inEncounter = !inEncounter; }
    
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
