using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnit : BaseUnit, ProductionFacilityInterface{

    public List<GameObject> products = new List<GameObject>(); //should be populated via inspector. index 0 should have a bison tank for testing
    private SpawnPad myPad;
    public List<GameObject> backorder = new List<GameObject>(); //list of units waiting to spawn
    private float lastSpawn = 0;
    int updateDelay = 1;
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
        int i = Random.Range(1, 11);
        float offset = 1 + (i * .1f);
        return new Vector3(offset, offset, offset);

    }

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

    public int getCostOfProduct(int number)
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
        public bool canAfford(int number)
        {
            switch (number)
            {
                case 0:
                    return PlayerControl.playerResources[owner] >= getCostOfProduct(number);
                default:
                    print("(3)invalid product num: " + number);
                    return false;

            }
        }

            public void produce(int number)
             {
                if (!canAfford(number)) return;
                PlayerControl.playerResources[owner] -= getCostOfProduct(number);
                backorder.Add(GetProduct(number));
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

    public List<GameObject> getSpawnQueue()
    {
        throw new System.NotImplementedException();
    }

    public void spawn(GameObject go)
    {
        Vector3 spawnLocation = getSpawnPad().transform.position + new Vector3(0, 4, 0); //4 units in the air
        Instantiate(go, spawnLocation, new Quaternion(0, 0, 0, 0));
    }
}
