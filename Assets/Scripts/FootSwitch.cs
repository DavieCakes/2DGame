using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSwitch : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject parent;
    public Material[] mats;
    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!activated)
            {
                Instantiate(doors[0], doors[1].transform.position, doors[1].transform.rotation, parent.transform);
                Destroy(doors[1]);
                activated = true;
            }
            GetComponent<Animator>().SetBool("On", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("On", false);
        }
    }
}
