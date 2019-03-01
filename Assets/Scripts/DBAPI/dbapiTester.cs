using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dbapiTester : MonoBehaviour
{
    
    private Data data;
    // Start is called before the first frame update
    void Awake()
    {
        data = new Data();
        data.LogRow("*", "items", "name" , "sword");
        Debug.Log("out");
    }

}
