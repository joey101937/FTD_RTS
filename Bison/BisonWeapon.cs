using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisonWeapon : Weapon {

    public override void fire()
    {
        print("Fire()");
    }
    
    public override void fire(BaseUnit target)
    {
        print("Fire(" + target + ")");
        lastFireTime = Time.time;
        GameObject bullet = Instantiate(this.projectileObject,transform.position+this.bulletSpawnOffset,transform.rotation);
        Projectile bulletP = bullet.GetComponent<Projectile>();
        bulletP.target = target.gameObject;
        bulletP.launch();
    }
    override
    public void init()
    {
        range = 55;
        damage = 15;
        antiGround = true;
        antiAir = false;
        fireRate = 2f;
        loadProjectile();
        this.bulletSpawnOffset = new Vector3(0, 0, 0);
    }
}
