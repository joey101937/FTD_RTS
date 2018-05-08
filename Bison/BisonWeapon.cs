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
    }
    override
    public void init()
    {
        range = 45;
        damage = 10;
        antiGround = true;
        antiAir = false;
        fireRate = 2f;
    }
}
