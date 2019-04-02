using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController m = other.gameObject.GetComponent<PlayerController>();
            other.transform.position = destination.position;
        }
    }
}
