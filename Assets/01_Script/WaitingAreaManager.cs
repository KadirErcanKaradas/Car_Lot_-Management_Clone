using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingAreaManager : Singleton<WaitingAreaManager>
{
    public List<GameObject> waitingArea = new List<GameObject>();
    public bool isEmptySlot = true;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            waitingArea.Add(obj);
        }
    }
    public bool IsParkingAreaReady()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (waitingArea[i].transform.GetChild(0).GetComponent<WaitingAreaController>().isAvailable == true)
            {
                return true;
            }
        }
        return false;
    }
}
