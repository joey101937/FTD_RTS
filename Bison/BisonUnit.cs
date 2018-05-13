using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisonUnit : BaseUnit {

	// Use this for initialization
    
	public override void init () {
        setUnitName("Bison Light Tank");
        maxHP = 100;
        HP = maxHP;
        selected = false;
        setClassification(Classification.Tank);
        print("bison init done");
        setMyPathfinder(transform.parent.gameObject.GetComponent<Pathfinder>());
    }

    public override Vector3 getHitOffset()
    {
        int i = Random.Range(1, 11);
        float offset = 1 + (i * .1f);
        return new Vector3(offset, offset,offset);

    }
}
