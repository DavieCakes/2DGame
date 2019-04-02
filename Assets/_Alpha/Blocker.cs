using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    bool door;
    public GameObject doors;
    public GameObject[] blockers;

    private void Start()
    {
        door = doors != null;
    }

    private void Update()
    {
        if (door)
            if (doors == null)
                door = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!door && other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject b in blockers)
            {
                b.transform.position = new Vector3(b.transform.position.x, 2.9f, b.transform.position.z);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!door && other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject b in blockers)
            {
                b.transform.position = new Vector3(b.transform.position.x, 1.9f, b.transform.position.z);
            }
        }
    }
}
