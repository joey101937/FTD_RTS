using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    private BaseUnit host = null;
    private Barrel barrel = null;
    private Weapon weapon = null;
    BaseUnit foe = null;
    Collider[] nearbyCol;
    List<BaseUnit> nearbyEnemies;
    Quaternion idleRotation;
    Quaternion targetRotation;
    private Idletarget idleTarget;
    public bool debugging = false;

    public Idletarget getIdleTarget()
    {
        return idleTarget;
    }
    public void setIdleTarget(Idletarget it)
    {
        idleTarget = it;
    }

    public BaseUnit getHost()
    {
        return host;
    }
    public void setHost(BaseUnit b)
    {
        host = b;
    }
    public Barrel getBarrel()
    {
        return barrel;
    }
    public void setBarrel(Barrel b)
    {
        barrel = b;
    }
    public Weapon getWeapon()
    {
        return barrel.getWeapon();
    }
    public void setWeapon(Weapon w)
    {
        weapon = w;
    }

    // Use this for initialization
    void Start () {
        host = this.gameObject.transform.parent.GetComponent<BaseUnit>();
        barrel = this.gameObject.GetComponentInChildren<Barrel>();
        weapon = barrel.getWeapon() ;
        idleRotation = transform.rotation;
        targetRotation = idleRotation;
        host.setupWeapons();
        if (debugging) print("turned 45");
        transform.Rotate(new Vector3(0, 1, 0), 45);

       
	}
	
	// Update is called once per frame
	void Update () {
        setDistances();
        if (foe != null)
        {
            this.transform.LookAt(new Vector3(foe.transform.position.x,gameObject.transform.position.y,foe.gameObject.transform.position.z));
            barrel.transform.LookAt(new Vector3(foe.transform.position.x, gameObject.transform.position.y, foe.transform.position.z));
            if (weapon.readyToFire())
            {
                weapon.fire(foe);
            }
        }
        else
        {
            if(debugging) print("looking at idle");
            this.transform.LookAt(new Vector3(idleTarget.transform.position.x, gameObject.transform.position.y, idleTarget.gameObject.transform.position.z));
            barrel.transform.LookAt(new Vector3(idleTarget.transform.position.x, idleTarget.gameObject.transform.position.y, idleTarget.transform.position.z));
        }

        if (debugging) print("turret update");
    }

    void setDistances()
    {
        nearbyCol = Physics.OverlapSphere(this.gameObject.transform.position, weapon.range); // an array of nearby colliders
        nearbyEnemies = new List<BaseUnit>();
        for (int col = 0; col < nearbyCol.Length; col++)
        {
            BaseUnit a = nearbyCol[col].gameObject.GetComponent<BaseUnit>();
            if (a != null)
            {
                if (a.owner != 0 && a.owner != host.owner && weapon.isValidTarget(a) && weapon.isInRange(a))
                {
                    nearbyEnemies.Add(a);
                }
            }
        }
        float closest = -1f;
        foreach (BaseUnit a in nearbyEnemies)
        {
            float thisDistance = Vector3.Distance(a.gameObject.transform.position, gameObject.transform.position);
            if (closest == -1 | thisDistance < closest)
            {
                closest = thisDistance;
                foe = a;
            }
        }
        if (foe==null || !weapon.isInRange(foe) || foe.owner == host.owner)
        {
            foe = null;
        }
    }

}
