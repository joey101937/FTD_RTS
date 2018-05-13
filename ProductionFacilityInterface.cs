using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface that all production facilities will use; can create a set of units (GameObjects), which are accessed via the get product method.
//number passed determines which unit to give.
public interface ProductionFacilityInterface {
    //gets unit corresponding to index
    GameObject GetProduct(int number);

    //get list of items quueed to spawn whenever the pad is open
    List<GameObject> getSpawnQueue();

    //cost of product in given index
    int getCostOfProduct(int number);

    //weather or not the product at given index has the requirements satisfied for creation (cost, tech unlock, etc)
    bool canAfford(int number);

    //produces the unit stored in <number> index (add to queue of objects to spawn)
    void produce(int number);

    //produces a given game object (add to queue of objects to spawn)
    void produce(GameObject go);
    
    //sets this facility's pad to passed pad
    void setSpawnPad(SpawnPad sp);
    
    //is the spawnpoint clear
    bool spawnClear();

    //actually spawns the object
    void spawn(GameObject go);

    SpawnPad getSpawnPad();
}
