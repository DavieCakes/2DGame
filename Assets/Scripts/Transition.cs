using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public bool test;

    public IEnumerator Up(float speed, bool dest)
    {
        test = dest;
        this.transform.localScale = Vector3.zero;
        float time = 0f;
        while (time <= 1)
        {
            this.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        if (dest)
            Destroy(transform.parent.gameObject);
        else
            StartCoroutine(Down(speed));
    }

    public IEnumerator Down(float speed)
    {
        this.transform.localScale = Vector3.one;
        float time = 0f;
        while (time <= 1)
        {
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
