using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject target = null;
    public int damage = 0;
    bool hasLaunched = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hasLaunched)
        {
            transform.LookAt(target.transform.position);
            gameObject.transform.Translate(0, 0, .25f);
        }
	}

    /*
     * launches the projectile at target
     */
    public void launch()
    {
        if (target == null)
        {
            return;
        }
        hasLaunched = true;
    }
}
