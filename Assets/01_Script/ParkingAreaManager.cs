using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingAreaManager : Singleton<ParkingAreaManager>
{
    public List<GameObject> parkingArea = new List<GameObject>();
    public bool isBuy = false;
    private void OnEnable()
    {
        GameEvent.BuyParkingArea += OpenNextParkingArea;
    }
    private void OnDisable()
    {
        GameEvent.BuyParkingArea -= OpenNextParkingArea;
    }
    protected override void Awake()
    {
        base.Awake();
        for (int i = 2; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            parkingArea.Add(obj);
        }
    }
    private void OpenNextParkingArea()
    {
        if (parkingArea.Count != 0)
        {
            parkingArea[0].SetActive(true);
            parkingArea.RemoveAt(0);
        }
    }
}
