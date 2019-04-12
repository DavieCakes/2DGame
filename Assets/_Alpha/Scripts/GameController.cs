using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    private PlayerController pc;
    bool inEncounter;
    AudioSource aud;
    public AudioClip[] clips;

    public Canvas PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        PauseUI.enabled = false;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over called");
    }

    public void Pause()
    {
        if (!inEncounter)
        {
            pc.pause = !pc.pause;
            PauseUI.enabled = pc.pause;
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
