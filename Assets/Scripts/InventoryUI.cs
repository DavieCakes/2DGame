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
    Dictionary<EquipSlot, GameObject> UI_EquipmentSlotPanels = new Dictionary<EquipSlot, GameObject>();
    GameObject UI_PotionsRemainingLabel;
    Transform UI_EquipmentPanel;
    Transform UI_InventoryPanel;
    Inventory IU_InventoryCopy;
    public GameObject Prefab_InventoryItemDisplay;
    private List<GameObject> InventoryItemDisplays = new List<GameObject>();

    void Awake()
    {

        Transform trans = GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // Debug.Log("Setting Model");
        // Debug.Log(playerController.playerModel.ToString());

        UI_EquipmentPanel = trans.Find("EquipmentPanel");
        UI_InventoryPanel = trans.Find("InventoryPanel");
        // Debug.Log(UI_InventoryPanel.name);

        UI_EquipmentSlotPanels.Add(EquipSlot.HELMET, GameObject.Find("HelmetPanel"));
        UI_EquipmentSlotPanels.Add(EquipSlot.ARMOR, GameObject.Find("ArmorPanel"));
        UI_EquipmentSlotPanels.Add(EquipSlot.WEAPON, GameObject.Find("WeaponPanel"));
        UI_EquipmentSlotPanels.Add(EquipSlot.BOOTS, GameObject.Find("BootsPanel"));
        Debug.Log(UI_EquipmentSlotPanels.Count);
        foreach ( KeyValuePair<EquipSlot, GameObject> entry in UI_EquipmentSlotPanels) {
            Debug.Log(entry.Key.ToString());
            entry.Value.GetComponent<Button>().onClick.AddListener(() => EquipmentSlotItemClicked(entry.Key));
        }



        GameObject.Find("PotionsIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/potion");
        this.UI_PotionsRemainingLabel = GameObject.Find("PotionsRemainingLabel");
        GameObject.Find("PotionPanel").GetComponent<Button>().onClick.AddListener(() => PotionItemClicked());

        // UpdateEquipmentPanel();
        // UpdateInventoryPanel();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        foreach (KeyValuePair<EquipSlot, GameObject> UI_icon in this.UI_EquipmentSlotPanels)
        {
            ((this.UI_EquipmentSlotPanels[UI_icon.Key]).transform.Find("IconContainer").Find("Icon").GetComponent<Image>()).color = Color.gray;
            ((this.UI_EquipmentSlotPanels[UI_icon.Key]).transform.Find("IconContainer").Find("Icon").GetComponent<Image>()).sprite = null;
        }

        foreach (KeyValuePair<EquipSlot, Equipment> equipped in playerController.playerModel.currentlyEquipped)
        {
            ((this.UI_EquipmentSlotPanels[equipped.Key]).transform.Find("IconContainer").Find("Icon").GetComponent<Image>()).color = Color.white;
            ((this.UI_EquipmentSlotPanels[equipped.Key]).transform.Find("IconContainer").Find("Icon").GetComponent<Image>()).sprite = Resources.Load<Sprite>("Icons/" + equipped.Value.name);
        }

        this.UI_PotionsRemainingLabel.GetComponent<Text>().text = this.playerController.playerModel.inventory.potions.ToString();
    }

    void UpdateInventoryPanel()
    {
        foreach (Equipment equipment in playerController.playerModel.inventory.EquipmentInventory)
        {
            GameObject tempDisplay = Instantiate(this.Prefab_InventoryItemDisplay);
            // Text tempText = tempDisplay.GetComponentInChildren<Text>();
            Debug.Log("Updating Inventory Item Display");
            (tempDisplay.GetComponentInChildren<Text>()).text = equipment.ToString();
            (tempDisplay.transform.Find("Icon")).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + equipment.name);

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
        playerController.UpdateUI();
    }

    /*
        On Click event for currently equipped item panel in the equipment panel
     */
    private void EquipmentSlotItemClicked(EquipSlot slot)
    {
        Debug.Log("Equipment Slot Clicked: " + slot.ToString());
        this.playerController.playerModel.Unequip(slot);
        playerController.UpdateUI();
        UpdateEquipmentPanel();
        ClearInventoryUI();
        UpdateInventoryPanel();
    }

    /*
        On click event for the potions
     */
    private void PotionItemClicked()
    {
        Debug.Log("Potions Clicked");
        if (playerController.UsePotion())
        { 
            UpdateEquipmentPanel();
        }
    }
}
