using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float movementSpeed = 1;
    public float zoomSpeed = 5;
    private static List<BaseUnit> selectedUnits = new List<BaseUnit>();
    private Ray selectionRay;
    public GameObject waypointObject; //set in inspector, this spawns where the player clicks to issue an order to show they did it
    public static GameObject waypointGlobal; //used to easily reference a waypoint
    bool clickedUnit = false;
    RaycastHit hit;
    BaseUnit unit;
    Turret tur;

    // Use this for initialization
    void Start () {
        waypointGlobal = waypointObject;
	}


    void Update()
    {
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        float zoom = Input.GetAxis("MouseScrollWheel") * zoomSpeed;
        Vector3 speed = new Vector3(sideSpeed , zoom * -1, forwardSpeed + zoom);
        transform.Translate(speed, Space.World);


        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)) // if we left click, we'll deselect all units and then only control the unit we clicked.
        {
            deselectAll();
            clickedUnit = false;
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, transform.forward * 300, Color.green);

            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity)) // If the vector pointing forward hits something 
            {
                print("ray hit " + hit.collider.gameObject.name);
                unit = hit.collider.GetComponent<BaseUnit>();
                if (unit != null)
                {
                    select(unit);
                    clickedUnit = true;
                }
                else
                {
                    tur = hit.collider.gameObject.GetComponent<Turret>();
                    if (tur != null)
                    {
                        select(tur.host);
                        clickedUnit = true;
                    }
                }
            }
            else
            {
                print("ray hit nothing");
                //we didnt hit anything so do nothing
            }
            }

                if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) // if we hold down shift, we will not deselect all units and will add the unit to our selection.
                {
 
                    clickedUnit = false;
                    selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(transform.position, transform.forward * 300, Color.green);

                    if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity)) // If the vector pointing forward hits something 
                    {
                        print("ray hit " + hit.collider.gameObject.name);
                        unit = hit.collider.GetComponent<BaseUnit>();
                        if (unit != null)
                        {
                            select(unit);
                            clickedUnit = true;
                        }
                        else
                        {
                            tur = hit.collider.gameObject.GetComponent<Turret>();
                            if (tur != null)
                            {
                                select(tur.host);
                                clickedUnit = true;
                            }
                        }

                    }
            else
            {
                print("ray hit nothing");
                //we didnt hit anything so do nothing
            }
            if (!clickedUnit && !Input.GetKey(KeyCode.LeftShift))
            {
                deselectAll();
            }
        }

        if (Input.GetMouseButtonDown(1) )
        {
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    /*
                    foreach(BaseUnit u in getSelected())
                    {
                        u.myPathfinder.issueMoveOrder(hit.point);
                    }
                    */
                    if (getSelected().Count == 0)
                    {
                        return;
                    }
                    if (getSelected().Count == 1)
                    {
                        getSelected()[0].myPathfinder.issueMoveOrder(hit.point);
                        Instantiate(waypointObject, hit.point, new Quaternion(0, 0, 0, 0));
                    }
                    else
                    {
                        //moving multiple units
                        fleetMove(getSelected(), hit.point);
                    }

                }
            }
        }
    }
    public void fleetMove(List<BaseUnit> units ,Vector3 destination)
    {

        float avgX = 0;
        float avgZ = 0;
        foreach(BaseUnit u in units)
        {
            avgX += u.transform.position.x;
            avgZ += u.transform.position.z;
        }
        avgX /= units.Count;
        avgZ /= units.Count;
        foreach (BaseUnit u in units)
        {
            Vector3 offset = new Vector3(avgX - u.transform.position.x, u.transform.position.y, avgZ - u.transform.position.z); //offset from the center of the mass of units, ignore y axis
            Instantiate(waypointObject, hit.point-offset, new Quaternion(0, 0, 0, 0));
            u.myPathfinder.issueMoveOrder(hit.point-offset);
        }
    }

    public static void deselectAll()
    {
        int times = selectedUnits.Count;
        for (int i = 0; i < times; i++)
        {
            BaseUnit u = selectedUnits[0];
            deselect(u);
        }
    }

    public static void deselect(BaseUnit u )
    {
        if (selectedUnits.Contains(u))
        {
            u.SetSelected(false);
            selectedUnits.Remove(u);
        }
    }

    public static List<BaseUnit> getSelected()
    {
        return selectedUnits;
    }

    public static void select(BaseUnit u)
    {
        u.SetSelected(true);
        selectedUnits.Add(u);
    }

 
    }
