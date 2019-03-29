using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        Movement m = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if (m == null)
            Debug.Log("Did not find movement on player!");
    }

    // Update is called once per frame
    void OnTriggerStay()
    {
        
    }
}
