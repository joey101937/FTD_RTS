using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnit : ProductionFacility{
    int updateDelay = 1; //used so that we cant spawn a bunch of units in the same second
    public override void init()
    {
        setUnitName("Test Factory");
        maxHP = 100;
        HP = maxHP;
        selected = false;
        setClassification(Classification.Structure);
        print("Test Factory init done");
        setMyPathfinder(transform.parent.gameObject.GetComponent<Pathfinder>());

    }

    public void Update()
    {
        if (Input.GetKeyDown("q") && selected)
        {
            print("test producing product 0");
            produce(0);
        }
        if (Time.time < lastSpawn + updateDelay) return; //wait a  while
        //whenever the spawn is clear, try to spawn the next thing in the list of units that need to be spawned
        if (spawnClear() && backorder.Count>0)
        {
            spawn(backorder[0]) ;
            backorder.RemoveAt(0);
            lastSpawn = Time.time;
        }
        
    }


    public override Vector3 getHitOffset()
    {
        int i = Random.Range(1, 31);
        float offset = 1 + (i * .1f);
        return new Vector3(offset, offset, offset);
    }



    public override int GetCostOfProduct(int number)
    {
        switch (number)
        {
            case 0:
                return 10; //bison, stored in index 0, costs 10 resources
            default:
                print("(2)invalid product num: " + number);
                return -1;

        }
    }

 



    public void spawn(GameObject go)
    {
        Vector3 spawnLocation = getSpawnPad().transform.position + new Vector3(0, 4, 0); //4 units in the air
        Instantiate(go, spawnLocation, new Quaternion(0, 0, 0, 0));
    }
}
