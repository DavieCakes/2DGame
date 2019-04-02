using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public enum Type
    {
        Arch,
        Regular,
        Locked,
        Trapped,
        Secret,
        Portcullis
    }

    public Type type = Type.Regular;
    public GameObject arch;
    public Transform parent;
    public GameObject damageMsg;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                switch (type)
                {
                    case Type.Regular:
                        Regular();
                        break;
                    case Type.Locked:
                        Locked(other.gameObject.GetComponent<PlayerController>());
                        break;
                    case Type.Trapped:
                        Trapped(other.gameObject.GetComponent<PlayerController>());
                        break;
                    case Type.Secret:
                        Secret();
                        break;
                }
            }
            else if(Input.GetKey(KeyCode.F))
            {
                TrapAvoid();
            }
        }
    }

    private void Regular()
    {

        GameObject temp = Instantiate(arch, transform.position, transform.rotation, parent);
        Destroy(gameObject);
    }

    private void Locked(PlayerController pc)
    {
        if (pc.UseKey())
        {
            GameObject temp = Instantiate(arch, transform.position, transform.rotation, parent);
            Destroy(gameObject);
        }
    }

    private void Trapped(PlayerController pc)
    {

        GameObject temp = Instantiate(arch, transform.position, transform.rotation, parent);
        Destroy(gameObject);
    }

    private void TrapAvoid()
    {

    }

    private void Secret()
    {

        GameObject temp = Instantiate(arch, transform.position, transform.rotation, parent);
        Destroy(gameObject);
    }
}
