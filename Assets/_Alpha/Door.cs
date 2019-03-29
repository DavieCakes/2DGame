using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum Type
    {
        Regular,
        Locked,
        Trapped,
        Secret,
        Portcullis
    }

    public Type type = Type.Regular;
    public GameObject arch;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            GameObject temp = Instantiate(arch, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
