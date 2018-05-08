using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idletarget : MonoBehaviour {
    //this is an empty used to point the gun when idle
    public GameObject turret;
    private void Awake()
    {
        turret.GetComponent<Turret>().idleTarget = this;
    }
}
