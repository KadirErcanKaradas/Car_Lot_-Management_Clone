using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : Singleton<CarSpawnManager>
{
    private ObjectPooler pool;
    private WaitingAreaManager waitingAreaManager;
    public Queue<GameObject> cars = new Queue<GameObject>();
    public int carCount;
    public bool isFirst = true;
    [SerializeField] private List<Material> carMaterial = new List<Material>();
    private void OnEnable()
    {
        GameEvent.WaitingAreaReady += CheckWaitingAreaReady;
    }
    private void OnDisable()
    {
        GameEvent.WaitingAreaReady -= CheckWaitingAreaReady;
    }
    private void Start()
    {
        pool = ObjectPooler.Instance;
        waitingAreaManager = WaitingAreaManager.Instance;
        CarSpawner();
        CheckWaitingAreaReady();
    }

    private void CarSpawner()
    {
        for (int i = 0; i < carCount; i++)
        {
            GameObject obj = pool.SpawnFromPool(PoolObjectType.Car, Vector3.zero, Quaternion.identity,false);

            cars.Enqueue(obj);
        }
    }
    Coroutine sendToCarCoroutine;
    private void CheckWaitingAreaReady()
    {
        if (sendToCarCoroutine == null)
        {
            sendToCarCoroutine = StartCoroutine(SendToCarWaitngArea());
        }
    }
    private IEnumerator SendToCarWaitngArea()
    {
        int areaCount = waitingAreaManager.waitingArea.Count;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < areaCount; i++)
        {
            if (waitingAreaManager.IsParkingAreaReady())
            {
                GameObject obj = cars.Dequeue();
                obj.transform.localPosition = Vector3.zero;
                int randomMaterial = Random.Range(0, 4);
                Material mat = carMaterial[randomMaterial];
                Material[] mats = obj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials;
                mats[0]= mat;
                obj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials = mats;
                obj.GetComponent<ICar>().GoToWaitngArea();
            }
            else
            {
                sendToCarCoroutine = null;
                break;
            }
            yield return new WaitForSeconds(3f);
        }
        sendToCarCoroutine = null;
    }
}