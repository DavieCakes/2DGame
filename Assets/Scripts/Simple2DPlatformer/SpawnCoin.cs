using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    public Transform[] coinSpawns;
    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();    
    }

    void Spawn() {
        for (int i = 0; i < coinSpawns.Length; i ++) {
            int coinFlip = Random.Range( 0, 2); // range ecluxed maximum
            if (coinFlip > 0) {
                Instantiate(coin, coinSpawns[i].position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
