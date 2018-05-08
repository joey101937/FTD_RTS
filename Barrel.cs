using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {
   public  Weapon weapon = null;
	// Use this for initialization
	void Awake () {
        weapon = gameObject.GetComponent<Weapon>();
	}
	
}
