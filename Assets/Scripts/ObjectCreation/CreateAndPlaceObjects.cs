using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAndPlaceObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    GameObject particle;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown (0))
        {
            
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); // creates a cube
            // Position defaults to all 0's
            Debug.Log(cube.transform.position);
            cube.transform.position = new Vector3(0f, 2f, -7f);
            cube.AddComponent<Light>();
            cube.GetComponent<Light>().type = LightType.Point;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray)) {
                Instantiate(particle, transform.position, transform.rotation);
            }


        }

        if (Input.GetMouseButtonDown(1)) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(0f, 3f, 0f);

            sphere.AddComponent<Camera>();

        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(1f, 1f, 1f);
            sphere.AddComponent<Rigidbody>();
        }
    }
}
