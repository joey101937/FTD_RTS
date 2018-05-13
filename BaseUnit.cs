using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class goes to every game unit, contains basics for unity mechanics including health, selection info, and unittype 
 */
public class BaseUnit : MonoBehaviour {
    public Weapon hullMountedWeapon = null;
    public int maxHP, HP;
    public int owner = 0; //0 is neutral
    public bool selected;
    public bool isArmed = true; //does this have a weapon or turret
    private int weaponRange = 0; //range of weapon(s)
    private string unitName = "Unnamed Unit";
    private Pathfinder myPathfinder = null; //this is to be set on start
    public List<Turret> turrets = new List<Turret>();
    public GameObject deathExplosion = null; //spawns on death
    public GameObject selectionCircle = null; //the green thing that is around the unit when selected
    private GameObject spawnedSelectionCircle = null; //the actual circle thing that is currently around the unit
    private Classification classification = Classification.Simple;

    public Pathfinder getMyPathfinder()
    {
        return myPathfinder;
    }
    public void setMyPathfinder(Pathfinder p)
    {
        myPathfinder = p;
    }
    public Classification GetClassification()
    {
        return classification;
    }
    public void setClassification(Classification c)
    {
        classification = c;
    }
    public string getUnitName()
    {
        return unitName;
    }
    public void setUnitName(string s)
    {
        unitName = s;
    }

    void Start()
    {
        init();
        recordUnit();
    }
    public int getWeaponRnage()
    {
        return weaponRange;
    }
    public void setWeaponRange(int i )
    {
        weaponRange = i;
    }
    //adds unit to list of that player
    public void recordUnit()
    {
        try
        {
            if (PlayerControl.playerUnits[owner] == null)
            {
                PlayerControl.playerUnits.Add(owner, new HashSet<BaseUnit>());
            }
            PlayerControl.playerUnits[owner].Add(this);
        }
        catch (KeyNotFoundException knfe)
        {
            PlayerControl.playerUnits.Add(owner, new HashSet<BaseUnit>());
            PlayerControl.playerUnits[owner].Add(this);
            print("added key " + owner);
        }

    }
    //removes unit from playerlist
    public void recordUnitDeath()
    {
        if (PlayerControl.playerUnits[owner].Contains(this))
        {
            PlayerControl.playerUnits[owner].Remove(this);
        }
    }
    public void setupWeapons()
    {
        int lowestRange = 99999;
        Turret[] tur = this.gameObject.GetComponentsInChildren<Turret>();
        if (tur != null && tur.Length > 0)
        {
            foreach (Turret t in tur)
            {
                if (t == null) continue;
                if (!turrets.Contains(t)) turrets.Add(t);
                if (t.getWeapon().range < lowestRange)
                {
                    lowestRange = t.getWeapon().range;
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
        owner = 1;
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
        recordUnitDeath();
        OnDeath();

        if (gameObject.transform.parent != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }
    //what happens when this dies
    public virtual void OnDeath()
    {
        if (deathExplosion != null)
        {
            Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
        }
    }



    //used to modify where bullets should aim. 
    public virtual Vector3 getHitOffset()
    {
        return new Vector3(0, 0, 0);
    }


    public void SetSelected(bool b)
    {
        if (b)
        {
            selected = true;
            if (selectionCircle != null && spawnedSelectionCircle==null)
            {
                spawnedSelectionCircle = Instantiate(this.selectionCircle, new Vector3(transform.parent.transform.position.x, -.35f, transform.parent.transform.position.z), new Quaternion(0, 0, 0, 0), this.transform);
            }
            //add here code that make a torus appear around the unit BROM ADD IT, your selection method sucks
        }
        else
        {
            selected = false;
            if (spawnedSelectionCircle != null)
            {
                Destroy(spawnedSelectionCircle);
            }
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
