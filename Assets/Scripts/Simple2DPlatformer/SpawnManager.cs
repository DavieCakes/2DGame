using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int maxPlatforms = 20;
    public GameObject platform;

    public GameObject[] list;

    // how far platforms spawn as distance between
    public float horizontalMin = 6.5f;
    public float horizontalMax = 14f;
    public float verticalMin = -6f;
    public float verticalMax = 6f;

    private Vector2 originPos; // anchor position used for how far away to spawn next platform

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position; // first set to start position
        Spawn();
    }

    void Spawn() {
        for (int i = 0; i < maxPlatforms; i++) {

            // random pos based off offset from original position by vector between min and max parameters
            // scalar + vector = vector
            Vector2 randomPosition = originPos + new Vector2 (Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax));

            // Quaternion.identity == no rotation
            // Quaternions, based of complex numbers, used internally by Unity to represent all rotations
            Instantiate(platform, randomPosition, Quaternion.identity); 

            originPos = randomPosition; // reset new pos, each new platform spawn offset from last.
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
