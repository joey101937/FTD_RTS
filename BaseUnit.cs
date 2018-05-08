﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class goes to every game unit, contains basics for unity mechanics including health, selection info, and unittype 
 */
public class BaseUnit : MonoBehaviour {
    public Weapon hullMountedWeapon = null;
    public int maxHP, HP;
    public int team = 0; //0 is neutral
    public bool selected;
    public bool isArmed = true; //does this have a weapon or turret
    public int weaponRange = 0; //range of weapon(s)
    public string unitName = "Unnamed Unit";
    public Pathfinder myPathfinder = null; //this is to be set on start
    public List<Turret> turrets = new List<Turret>();
    public Classification classification = Classification.Simple;

    void Start()
    {
        init();
        setupWeapons();
    }

    public void setupWeapons()
    {
        int lowestRange = 99999;
        Turret[] tur = this.gameObject.GetComponentsInChildren<Turret>();
        if (tur != null && tur.Length>0)
        {
            foreach (Turret t in tur)
            {
                if (t == null) continue;
                if(!turrets.Contains(t))turrets.Add(t);
                if (t.weapon.range < lowestRange)
                {
                    lowestRange = t.weapon.range;
                }
            }
        }
        if (hullMountedWeapon != null && hullMountedWeapon.range < lowestRange) lowestRange = hullMountedWeapon.range;
        weaponRange = lowestRange;
        if (lowestRange > 9999)
        {
            //no guns
            isArmed = false;
        }
    }
    //should be overwritten by each unit, sets up initial params
    public virtual void init()
    {
        unitName = "unnamed Unit";
        maxHP = 100;
        HP = maxHP;
        selected = false;
        classification = Classification.Simple;
        myPathfinder = gameObject.GetComponent<Pathfinder>();
        team = 1;
    }
   


    /*
     * dont just modify the raw HP, use this method
     */
    public void TakeDamage(int amount)
    {
        this.HP -= amount;
        if (HP <= 0)
        {
            Kill();
        }
    }
    /*
     * removes this unit from the game
     */
    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void SetSelected(bool b)
    {
        if (b)
        {
            selected = true;
            //add here code that make a torus appear around the unit BROM ADD IT
        }
        else
        {
            selected = false;
            //add here code that makes the torus invisable BROM ADD IT
        }
    }

    public bool IsSelected()
    {
        return selected;
    }
}
/*
 * used in the backend for determining which pathfinder to use and other mechaincal things
 * simple is basic look at target then shoot
 * tank should support a rotating turret
 * heli floats in air and looks at target
 * plane flies in lines around target
 * structure doesnt move
 */
public enum Classification { Simple, Tank, Plane, Heli, Structure };
/*
 * used for in-game systems like dealing extra damage to armored units
 */
public enum Attribute { Light, Armored, Flier, Structure, Biological } 
