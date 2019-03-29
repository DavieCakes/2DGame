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
            Movement m = other.gameObject.GetComponent<Movement>();
            m.pause = true;
            StartCoroutine(MoveWait(m));
            other.transform.position = destination.position;
        }
    }

    IEnumerator MoveWait(Movement m)
    {
        yield return new WaitForSeconds(.25f);
        m.pause = false;
    }
}
