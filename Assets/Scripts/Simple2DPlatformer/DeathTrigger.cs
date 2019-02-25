using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    // public GameObject sensor;


    // private Transform playerPos;
    // private Transform sensorPos;
    // Start is called before the first frame update
    void Start()
    {
        // playerPos = gameObject.GetComponent<Transform>();
        // sensorPos = sensor.GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Fall Sensor triggered on tag: " + other.tag);
        if (other.gameObject.CompareTag("Player")) 
        {
            // Debug.Log("Death Triggered Character");
            SceneManager.LoadScene(1);
        } 
        else 
        {
            Destroy(other);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
