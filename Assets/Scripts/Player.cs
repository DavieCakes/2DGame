using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Creatures;

public class Player : MonoBehaviour
{
    public Camera cam;
    public Creature playerModel;
    public UnityEngine.AI.NavMeshAgent agent;

    public Dictionary <string, string> playerData;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        playerModel = new Creature();
        playerModel.spells.Add("Fireball");
        playerData = new Dictionary<string, string>();
        playerData.Add("spell", playerModel.spells[0]);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject()) { // detect if pointer is over a ui element
            if (Input.GetMouseButton(0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit))
                {
                    // MOVE AGENT
                    agent.SetDestination(hit.point);

                }
            }
        }
    }
}

