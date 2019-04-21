using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Items;
using Inventories;

public class InventoryUI : MonoBehaviour
{
    // to reference inventory and equipment
    PlayerController playerController;
    Dictionary<EquipSlot, GameObject> UI_EquipmentIcons = new Dictionary<EquipSlot, GameObject>();
    GameObject UI_PotionsRemainingLabel;
    Transform UI_EquipmentPanel;
    Transform UI_InventoryPanel;
    Inventory IU_InventoryCopy;
    public GameObject Prefab_InventoryItemDisplay;
    private List<GameObject> InventoryItemDisplays = new List<GameObject>();

    void Start()
    {

        Transform trans = GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Log(playerController.playerModel.ToString());

        UI_EquipmentPanel = trans.Find("EquipmentPanel");
        UI_InventoryPanel = trans.Find("InventoryPanel");

        UI_EquipmentIcons.Add(EquipSlot.HELMET, GameObject.Find("HelmetIcon"));
        UI_EquipmentIcons.Add(EquipSlot.ARMOR, GameObject.Find("ArmorIcon"));
        UI_EquipmentIcons.Add(EquipSlot.WEAPON, GameObject.Find("WeaponIcon"));
        UI_EquipmentIcons.Add(EquipSlot.BOOTS, GameObject.Find("BootsIcon"));

        GameObject.Find("PotionsIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/potion");
        this.UI_PotionsRemainingLabel = GameObject.Find("PotionsRemainingLabel");
        GameObject.Find("PotionPanel").GetComponent<Button>().onClick.AddListener(() => PotionItemClicked());

        UpdateEquipmentPanel();
        UpdateInventoryPanel();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        UpdateEquipmentPanel();
        UpdateInventoryPanel();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        ClearInventoryUI();
    }

    private void ClearInventoryUI()
    {
        foreach (GameObject display in this.InventoryItemDisplays)
        {
            GameObject.Destroy(display);
        }
        InventoryItemDisplays.Clear();
    }

    /**
    ToolTips:
        [Tooltip(item.ToString())]
        int health = 0;

     */


    void UpdateEquipmentPanel()
    {
        foreach (KeyValuePair<EquipSlot, GameObject> UI_icon in this.UI_EquipmentIcons)
        {
            ((this.UI_EquipmentIcons[UI_icon.Key]).GetComponent<Image>()).color = Color.gray;
        }

        Debug.Log(playerController.PlayerName);
        foreach (KeyValuePair<EquipSlot, Equipment> entry in playerController.playerModel.currentlyEquipped)
        {
            ((this.UI_EquipmentIcons[entry.Key]).GetComponent<Image>()).color = Color.white;
            ((this.UI_EquipmentIcons[entry.Key]).GetComponent<Image>()).sprite = Resources.Load<Sprite>("Icons/" + entry.Value.iconName);
        }

        this.UI_PotionsRemainingLabel.GetComponent<Text>().text = this.playerController.playerModel.inventory.potions.ToString();

        Debug.Log(playerController.playerModel.ToString());
    }

    void UpdateInventoryPanel()
    {
        foreach (Equipment equipment in playerController.playerModel.inventory.equipmentInventory)
        {
            GameObject tempDisplay = Instantiate(this.Prefab_InventoryItemDisplay);
            // Text tempText = tempDisplay.GetComponentInChildren<Text>();
            Debug.Log("Updating Inventory Item Display");
            (tempDisplay.GetComponentInChildren<Text>()).text = equipment.ToString();
            (tempDisplay.transform.Find("Icon")).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + equipment.iconName);
            tempDisplay.transform.SetParent(UI_InventoryPanel.transform);
            tempDisplay.GetComponent<Button>().onClick.AddListener(() => InventoryItemClicked());
            this.InventoryItemDisplays.Add(tempDisplay);
        }
    }

    /*
        On Click event for items in the player's inventory (not currently equipped)
     */
    private void InventoryItemClicked()
    {
        Debug.Log("Inventory Item clicked!");
        playerController.playerModel.EquipFromInventory(EventSystem.current.currentSelectedGameObject.transform.Find("Icon").GetComponent<Image>().sprite.name);
        UpdateEquipmentPanel();
        ClearInventoryUI();
        UpdateInventoryPanel();
    }

    /*
        On Click event for currently equipped item panel in the equipment panel
     */
    private void EquipmentSlotItemClicked()
    {

    }

    /*
        On click event for the potions
     */
    private void PotionItemClicked()
    {
        // Health potion gets used up to full health, and only if not at full health already
        if (this.playerController.playerModel.TryHealthPotion())
        { 
            UpdateEquipmentPanel(); 
            this.playerController.UpdateUI();
        }
    }
}
