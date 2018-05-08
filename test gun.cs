using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testgun : Weapon {

    override
    public void fire()
    {
        //dont do anything if we try to fire without target
    }
    override
    public void fire(BaseUnit target)
    {
        target.TakeDamage(damage);
    }
}
