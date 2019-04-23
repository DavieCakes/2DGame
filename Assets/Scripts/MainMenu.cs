using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, audioMenu, helpMenu;

    private void Start()
    {
        audioMenu.SetActive(false);
        helpMenu.SetActive(false);
        mainMenu.transform.GetChild(0).GetChild(0).GetComponent<Button>().Select();
    }

    public void Play()
    {
        SceneManager.LoadScene("Beta");
    }

    public void Audio()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void Help()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void Back()
    {
        audioMenu.SetActive(false);
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
