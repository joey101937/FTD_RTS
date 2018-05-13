using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {
   private  Weapon weapon = null;
	// Use this for initialization
	void Awake () {
        weapon = gameObject.GetComponent<Weapon>();
	}

    public Weapon getWeapon()
    {
        return weapon;
    }
    
    public void setWeapon(Weapon w)
    {
        weapon = w;
    }
}
