using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Its : MonoBehaviour
{
    public string itemName;
    
    public string GetItemName() { return itemName; }

    public bool Equals(string s)
    {
        return itemName.Equals(s);
            
    }
}
