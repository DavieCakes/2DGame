using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Items;
using Inventories;

public class InventoryUI : MonoBehaviour
{
    // to reference inventory and equipment
    PlayerController player;

    Dictionary<EquipSlot,GameObject> UI_EquipmentIcons = new Dictionary<EquipSlot, GameObject>();
    Transform UI_EquipmentPanel;
    Transform UI_InventoryPanel;
    Inventory IU_InventoryCopy;

    public GameObject Prefab_InventoryItemDisplay;
    private List<GameObject> InventoryItemDisplays = new List<GameObject>();


    void Start()
    {
        
        Transform trans = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Log(player.ToString());
        
        UI_EquipmentPanel = trans.Find("EquipmentPanel");
        UI_InventoryPanel = trans.Find("InventoryPanel");

        UI_EquipmentIcons.Add(EquipSlot.HELMET, GameObject.Find("HelmetIcon"));
        UI_EquipmentIcons.Add(EquipSlot.ARMOR, GameObject.Find("ArmorIcon"));
        UI_EquipmentIcons.Add(EquipSlot.WEAPON, GameObject.Find("WeaponIcon"));
        UI_EquipmentIcons.Add(EquipSlot.BOOTS, GameObject.Find("BootsIcon"));

        UpdateEquipmentPanel();
        UpdateInventoryPanel();
        Debug.Log("Testing");
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
        foreach (GameObject display in this.InventoryItemDisplays) {
            // InventoryItemDisplays.Remove(display);
            GameObject.Destroy(display);
        }
        InventoryItemDisplays.Clear();
    }

    /**
    ToolTips:
        [Tooltip(item.ToString())]
        int health = 0;

     */

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateEquipmentPanel() {
        // if (player.playerModel.currentlyEquipped.ContainsKey(EquipSlot.HELMET)) {
        //     Debug.Log("helm");
        //     ((this.UI_EquipmentIcons[EquipSlot.HELMET]).GetComponent<Image>()).color =  Color.black;
        //     }
        // if (player.playerModel.currentlyEquipped.ContainsKey(EquipSlot.ARMOR)) {
        //     Debug.Log("armor");
        //     }
        // if (player.playerModel.currentlyEquipped.ContainsKey(EquipSlot.WEAPON)) {
        //     Debug.Log("Weapon");
        //     }
        // if (player.playerModel.currentlyEquipped.ContainsKey(EquipSlot.BOOTS)) {
        //     Debug.Log("boots");
        //     }
        foreach (KeyValuePair<EquipSlot, GameObject> UI_icon in this.UI_EquipmentIcons) {
            ((this.UI_EquipmentIcons[UI_icon.Key]).GetComponent<Image>()).color = Color.white;
        }

        foreach (KeyValuePair<EquipSlot, Equipment> entry in player.playerModel.currentlyEquipped) {
            ((this.UI_EquipmentIcons[entry.Key]).GetComponent<Image>()).color = Color.black;
        }
    }

    void UpdateInventoryPanel() {
        foreach (Equipment equipment in player.playerModel.inventory.equipmentInventory) {
            GameObject tempDisplay = Instantiate(this.Prefab_InventoryItemDisplay);
            // Text tempText = tempDisplay.GetComponentInChildren<Text>();
            Debug.Log("Updating Inventory Item Display");
            (tempDisplay.GetComponentInChildren<Text>()).text = equipment.ToString();
            tempDisplay.transform.SetParent(UI_InventoryPanel.transform);
            tempDisplay.GetComponent<Button>().onClick.AddListener(() => Debug.Log("Inventory Item Clicked!"));
            this.InventoryItemDisplays.Add (tempDisplay);
        }
    }
}
