using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsButton : MonoBehaviour
{
    Button btn;
    public GameObject spellsPanel;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleSpellsPanel);
    }

    void ToggleSpellsPanel() {
            spellsPanel.SetActive(!spellsPanel.activeSelf);
    }
}
