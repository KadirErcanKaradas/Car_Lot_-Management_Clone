using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public GameObject firstSelected;
    public GameObject currentCarSelected;
    public GameObject secondSelected;
    public bool isFirst = true;
    public bool isSecond = false;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if (isFirst && selection.CompareTag("Car"))
                {
                    selection.GetComponent<OutlineX>().enabled = true;
                    firstSelected = selection.gameObject;
                    isFirst = false;
                    isSecond = true;
                }
                else if (isFirst == false && selection.CompareTag("Car"))
                {
                    currentCarSelected = selection.gameObject;
                    if (currentCarSelected != firstSelected)
                    {
                        firstSelected.GetComponent<OutlineX>().enabled = false;
                        currentCarSelected.GetComponent<OutlineX>().enabled = true;
                        firstSelected = currentCarSelected;
                    }
                }
                else if (isSecond && selection.CompareTag("ParkSlot"))
                {
                    if (selection.GetComponent<ParkingAreaController>().isEmpty)
                    {
                        if (firstSelected.CompareTag("Car"))
                        {
                            selection.GetComponent<ParkingAreaController>().CarIsComing();
                            selection.GetComponent<ParkingAreaController>().isEmpty = false;
                            selection.GetChild(1).GetComponent<Collider>().enabled = true;
                            secondSelected = selection.gameObject;
                            isSecond = false;
                            var parkingAreaController = secondSelected.GetComponent<ParkingAreaController>();
                            var carController = firstSelected.GetComponent<CarController>();
                            carController.GoToParkingArea(parkingAreaController.target, parkingAreaController.parkingSlotName, parkingAreaController.target.name);
                        }
                        Reset();
                    }
                    else
                    {
                        selection.GetComponent<ParkingAreaController>().CarIsHere();
                    }

                }
                else
                {
                    Reset();
                }
            }
        }
    }
    public void Reset()
    {
        isFirst = true;
        isSecond = false;
        firstSelected = null;
        secondSelected = null;
    }
}
