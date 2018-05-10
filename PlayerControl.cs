using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float movementSpeed = 1;
    private static List<BaseUnit> selectedUnits = new List<BaseUnit>();
    private Ray selectionRay;
    public GameObject waypointObject; //set in inspector, this spawns where the player clicks to issue an order to show they did it

	// Use this for initialization
	void Start () {
		
	}


    void Update()
    {
        float rotX = Input.GetAxis("Mouse X") * 1.5f;
        float rotY = Input.GetAxis("Mouse Y") * -1.5f; //dont ask me why i have to multiply it by that
        transform.Rotate(rotY, rotX, 0);
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed);
        speed = transform.rotation * speed;
        transform.Translate(speed, Space.World);

        if (Input.GetMouseButton(0))
        {
            bool clickedUnit = false;
            RaycastHit hit;
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, transform.forward * 300, Color.green);

            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity)) // If the vector pointing forward hits something 
            {
                print("ray hit " + hit.collider.gameObject.name);
                BaseUnit unit = hit.collider.GetComponent<BaseUnit>();
                if (unit != null)
                {
                    select(unit);
                    clickedUnit = true;
                }
                else
                {
                    Turret tur = hit.collider.gameObject.GetComponent<Turret>();
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
            if (!clickedUnit)
            {
                deselectAll();
            }
        }

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(selectionRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    foreach(BaseUnit u in getSelected())
                    {
                        u.myPathfinder.issueMoveOrder(hit.point);
                    }
                }
                if (getSelected().Count > 0 && waypointObject!=null)
                {
                    Instantiate(waypointObject, hit.point, new Quaternion(0, 0, 0, 0));
                }
            }
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
