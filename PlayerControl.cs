using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float movementSpeed = 1;
    public float zoomSpeed = 500;
    private static List<BaseUnit> selectedUnits = new List<BaseUnit>();
    private Ray selectionRay;
    public GameObject waypointObject; //set in inspector, this spawns where the player clicks to issue an order to show they did it
    public static GameObject waypointGlobal; //used to easily reference a waypoint
    public int owner = 1; //the player cooresponding to this controller
    bool clickedUnit = false;
    RaycastHit hit;
    BaseUnit unit;
    Turret tur;
    public static Dictionary<int,HashSet<BaseUnit>> playerUnits = new Dictionary<int,HashSet<BaseUnit>>(); //list of units owned by a player, key=player number
    public static Dictionary<int, int> playerResources = new Dictionary<int, int>(); //amount of resources each player has, <ownerNum, resourceAmount>
    // Use this for initialization
    void Start () {
        waypointGlobal = waypointObject;
        playerResources.Add(0, 0); //player 0 is poor
        playerResources.Add(1, 20); //player 1 has 20 resources
        playerResources.Add(2, 10); //player 2 has 10
	}
    private bool dragging = false;
    private bool debugging = false;
    private Vector2 mouseDownLocation = new Vector2(0, 0); //where the player set down the mouse to start with when dragging
    void Update()
    {


        dealWithSelectionBox();
        dealWithMovement();
        dealWithSelection();
        dealWithOrders();
    }

    public void dealWithSelectionBox()
    {
        if (dragging&&debugging) print("Dragging");
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            setMouseDownLocation();
            if(debugging)print("mouse down location is " + mouseDownLocation);
        }
        if (dragging)//player is draging selection around
        {
            Vector2 currentLocation = new Vector2(0, 0);
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity))
            {
                float x = hit.point.x;
                float z = hit.point.z;
                currentLocation = new Vector2(x, z);
                //current location is now where the mouse is
            }
            if(debugging)print("current location is " + currentLocation);
            try {
            foreach (BaseUnit b in playerUnits[owner])
            {
                float x = b.gameObject.transform.position.x;
                float z = b.gameObject.transform.position.z;
                if((x >  mouseDownLocation.x && x < currentLocation.x) || x < mouseDownLocation.x && x > currentLocation.x)
                {
                    if ((z > mouseDownLocation.y && z < currentLocation.y) || (z < mouseDownLocation.y && z > currentLocation.y)) 
                    {
                        //units should now be in the box
                        select(b);
                    }
                }
            }
            }catch(KeyNotFoundException knfe)
            {
                //player has no units yet
                return;
            }
        }
    }

    private void setMouseDownLocation()
    {
        selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(selectionRay,out hit, Mathf.Infinity))
        {
            float x = hit.point.x;
            float z = hit.point.z;
            mouseDownLocation = new Vector2(x, z);
        }
        else
        {
            mouseDownLocation = new Vector2(0, 0);
        }
    }


    public void dealWithMovement()
    {
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        float zoom = Input.GetAxis("MouseScrollWheel") * zoomSpeed;
        Vector3 speed = new Vector3(sideSpeed, zoom * -1, forwardSpeed + zoom);
        transform.Translate(speed, Space.World);
    }

    public void dealWithSelection()
    {
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
                        select(tur.getHost());
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
    
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) // if we hold down shift, we will not deselect all units and will add the unit to our selection.
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
                        select(tur.getHost());
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
    }


    //runs in update method, deals with players issuing orders
    public void dealWithOrders()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    /*
                    foreach(BaseUnit u in getSelected())
                    {
                        u.myPathfinder.issueMoveOrder(hit.point);f
                    }
                    */
                    if (getSelected().Count == 0)
                    {
                        return;
                    }
                    if (getSelected().Count == 1)
                    {
                        getSelected()[0].getMyPathfinder().issueMoveOrder(hit.point);
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
            Vector3 offset = new Vector3(avgX - u.transform.position.x, u.transform.parent.transform.position.y*0, avgZ - u.transform.position.z); //offset from the center of the mass of units, ignore y axis
            Instantiate(waypointObject, hit.point-offset, new Quaternion(0, 0, 0, 0));
            u.getMyPathfinder().issueMoveOrder(hit.point-offset);
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

    public void select(BaseUnit u)
    {
        if (u.owner != owner)
        {
            return;
        }
        u.SetSelected(true);
        selectedUnits.Add(u);
    }

  
    }
