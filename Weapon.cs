using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * fires at other units.
 * dont cofuse with turret
 */
public class Weapon : MonoBehaviour {
    public Vector3 bulletSpawnOffset = new Vector3(0, 0, 0); //where the bullet spawns in relation to the weapon
    public int range = 10;
    public int damage = 10;
    public GameObject projectileObject; //should be set to the thing we want to fire
    public Projectile projectile = null; //projectile script to be taken from projectile object
    public bool antiGround, antiAir; //can it shoot at ground / air targets
    public float fireRate = 1f; //how  often it can fire
    public float lastFireTime = 0;
	// Use this for initialization
	void Start () {
        init();
	}

    public virtual void init()
    {
        loadProjectile();
    }


	public void loadProjectile()
    {
        projectile = projectileObject.GetComponent<Projectile>();
    }

    public virtual void fire()
    {
        print("base fire without target");
        this.lastFireTime = Time.time;
        //instanciate bullet and ahve it travel forward
        //then set lastFireTime to Time.time
    }

    public virtual void fire(BaseUnit target)
    {
        print("base fire at target");
        this.lastFireTime = Time.time;
        //instanciate a bullet and have it travel to the target
        //then set lastFireTime to Time.time
    }

    public virtual bool isValidTarget(BaseUnit target)
    {
       if((target.classification == Classification.Heli || target.classification == Classification.Plane) && !antiAir) {
            return false;
        }
       if((target.classification == Classification.Structure || target.classification == Classification.Tank || target.classification == Classification.Simple) && !antiGround)
        {
            return false;
        }
        return true;
    }
    public bool isInRange(BaseUnit target)
    {
        if(Vector3.Distance(transform.position,target.transform.position) <= range)
        {
            return true;
        }
        return false;
    }

    public bool readyToFire()
    {
        if(Time.time - lastFireTime >= fireRate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
