using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, helpMenu;

    private void Start()
    {
        helpMenu.SetActive(false);
        mainMenu.transform.GetChild(0).GetChild(0).GetComponent<Button>().Select();
    }

    public void Play()
    {
        SceneManager.LoadScene("Beta");
    }

    public void Help()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void Back()
    {
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
