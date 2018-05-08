using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisonUnit : BaseUnit {

	// Use this for initialization
    
	public override void init () {
        unitName = "Bison light Tank";
        maxHP = 100;
        HP = maxHP;
        selected = false;
        classification = Classification.Tank;
        myPathfinder = gameObject.GetComponent<Pathfinder>();
        team = 1;
        print("bison init done");
        this.myPathfinder = transform.parent.gameObject.GetComponent<Pathfinder>();
    }

}
