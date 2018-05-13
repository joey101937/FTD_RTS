using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//thing that units are spawned on, checks to see if there is space avail to spawn
public class SpawnPad : MonoBehaviour {
    public bool isOpen = true;
    public GameObject productionFacility = null; //should be set in inspector to related production building

    private void Awake()
    {
        productionFacility.GetComponent<FactoryUnit>().setSpawnPad(this);
    }

    private void OnTriggerStay(Collider collision)
    {
        BaseUnit b = collision.gameObject.GetComponent<BaseUnit>();
        if (b!= null)
        {
            if (b == productionFacility) return;
            isOpen = false;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        isOpen = true;
    }
}
