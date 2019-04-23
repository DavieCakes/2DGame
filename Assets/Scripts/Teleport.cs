using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public Transform destination;
    public Canvas transition;
    
    public float speed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(transition != null)
            {
                StartCoroutine(Transition(other));
            }
            else
                other.transform.position = destination.position;
        }
    }

    IEnumerator Transition(Collider other)
    {
        PlayerController m = other.gameObject.GetComponent<PlayerController>();
        m.pause = true;
        StartCoroutine(Instantiate(transition).transform.GetChild(0).GetComponent<Transition>().Up(speed, false));
        yield return new WaitForSeconds(1f / speed);
        other.transform.position = destination.position;
        yield return new WaitForSeconds(1f / speed);
        m.pause = false;
    }
}
