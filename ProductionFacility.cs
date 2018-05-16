using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductionFacility : BaseUnit {
    public List<GameObject> products = new List<GameObject>(); //should be populated via inspector. index 0 should have a bison tank for testing
    protected SpawnPad myPad;
    public List<GameObject> backorder = new List<GameObject>(); //list of units waiting to spawn
    protected float lastSpawn = 0;
    public bool rallySet = false;
    public Vector3 rallyPoint = new Vector3(0, 0, 0);

    public GameObject GetProduct(int number)
    {
        switch (number)
        {
            case 0:
                return products[0];
            default:
                print("(1)invalid product num: " + number);
                return null;
        }
    }


    public abstract int GetCostOfProduct(int number);

    public virtual bool canAfford(int number)
    {
        if (GetCostOfProduct(number) == -1)
        {
            throw new System.Exception("geting to test affordability of product index that doesnt exist: " + number);
        }
        return PlayerControl.playerResources[owner] >= GetCostOfProduct(number);
    }


    public void produce(int number)
    {
        if (!canAfford(number)) return;
        PlayerControl.playerResources[owner] -= GetCostOfProduct(number);
        //this is where we would put a construction delay
        backorder.Add(GetProduct(number));
    }

    public void spawn(GameObject go)
    {
        Vector3 spawnLocation = getSpawnPad().transform.position + new Vector3(0, 4, 0); //4 units in the air
        GameObject spawned = Instantiate(go, spawnLocation, new Quaternion(0, 0, 0, 0));
        print("just spawned + " + spawned.name);
        if (rallyPointSet())
        {
            Pathfinder p = spawned.GetComponent<Pathfinder>();
            p.issueMoveOrder(getRallyPoint());
        }

    }

    public void produce(GameObject go)
    {
        backorder.Add(go);
    }


    public void setSpawnPad(SpawnPad sp)
    {
        print("setting spawnpad");
        myPad = sp;
    }

    public SpawnPad getSpawnPad()
    {
        return myPad;
    }

    public bool spawnClear()
    {
        return myPad.isOpen;
    }
    public bool rallyPointSet()
    {
        return rallySet;
    }
    public void setRallyPoint(Vector3 rally)
    {
        rallySet = true;
        rallyPoint = rally;
    }
    public Vector3 getRallyPoint()
    {
        return rallyPoint;
    }

}
