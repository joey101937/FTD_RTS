using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * parent pathfinder for all kinds of unit
 * this is the pathfinder to be used with "simple" classification units
 */
public class Pathfinder : MonoBehaviour {
    private NavMeshAgent agent;
    private BaseUnit host;
    private Weapon myWeapon;
    private int scanRadius = 70;
    public bool debugging = true;
    private BaseUnit foe = null;
    private bool travelingByInstruction = false; //is the unit going somewhere teh player insctructed
    private Vector3 givenDestination = new Vector3(0,0,0); //where the player told the unit to go
    private GameObject waypoint = null; //waypoint, displayed when unit is selected so we can see where it is trying to go
    // set distances fields
    Collider[] nearbyCol;
    List<BaseUnit> nearbyEnemies;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, 70);
    }

    public BaseUnit getHost()
    {
        return host;
    }

    public NavMeshAgent getAgent()
    {
        return agent;
    }
    public Weapon getMyWeapon()
    {
        return myWeapon;
    }
    public int getScanRadius()
    {
        return scanRadius;
    }
    public BaseUnit getFoe()
    {
        return foe;
    }
    public Vector3 getGivenDestination()
    {
        return givenDestination;
    }
    public void setGivenDestination(Vector3 i)
    {
        givenDestination = i;
    }
    public bool isTravelingByInstruction()
    {
        return travelingByInstruction;
    }

    private void Update()
    {
        if (host.GetClassification() == Classification.Structure)
        {
            agent.speed = 0;
            agent.SetDestination(transform.position);
            return; 
        }
        if (host.selected && travelingByInstruction && waypoint==null)
        {
            waypoint = Instantiate(PlayerControl.waypointGlobal, givenDestination, new Quaternion(0, 0, 0, 0));
        }
        SetDistances();
        if (travelingByInstruction)
        {
            //if the player said to go somewhere, go there and thats it.

            agent.SetDestination(givenDestination);
            if (Vector3.Distance(transform.position, givenDestination) <= 10)
            {
                travelingByInstruction = false; //we have arrived
            }
            return;
        }
        else
        {
            if(foe!=null)
            {
                //there is a foe around, go to it
                agent.SetDestination(foe.transform.position);
                if (Vector3.Distance(foe.transform.position, transform.position) <= host.getWeaponRnage()+0) //+2 so we go alittle under the required range
                {
                    agent.SetDestination(transform.position); //stop once in range
                    if (host.hullMountedWeapon != null)
                    {
                        host.transform.LookAt(foe.transform);
                        if (host.hullMountedWeapon.readyToFire())
                        {
                            host.hullMountedWeapon.fire(foe);
                        }
                    }
                }
            }
        }
    }


    void Awake()
    {
        scanRadius = 70;
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        host = this.gameObject.GetComponentInChildren<BaseUnit>();
        myWeapon = host.hullMountedWeapon;
        givenDestination =  gameObject.transform.position;
        if (agent == null || host == null)
        {
            print("YOU HAVE A PATHFINDER OBJECT WITH NO BASEUNIT AND/OR AGENT");
            return;
        }
    }


    public void SetDistances()
    {
        nearbyCol = Physics.OverlapSphere(this.gameObject.transform.position, scanRadius); // an array of nearby colliders
        nearbyEnemies = new List<BaseUnit>();
        for (int col = 0; col < nearbyCol.Length; col++)
        {
            BaseUnit a = nearbyCol[col].gameObject.GetComponent<BaseUnit>();
            if (a != null)
            {
                if (debugging) print("found a unit of team " + a.owner + " (im team " + host.owner+ ")");
                if (a.owner != 0 && a.owner != host.owner)
                {
                    nearbyEnemies.Add(a);
                }
            }
        }
        if (debugging)
        {
            print("unit of team " + host.owner + " found " + nearbyEnemies.Count +" nearby enemies");
        }
        float closest = -1f;
        foreach (BaseUnit a in nearbyEnemies)
        {
            float thisDistance = Vector3.Distance(a.gameObject.transform.position, gameObject.transform.position);
            if (closest == -1 || thisDistance < closest)
            {
                closest = thisDistance;
                foe = a;
            }
        }
        if (debugging) print("END OF SETDIST, FOE IS  " + foe);
    }
    

    


    public void issueMoveOrder(Vector3 destination)
    {
        this.travelingByInstruction = true;
        givenDestination = destination;
    }
}
