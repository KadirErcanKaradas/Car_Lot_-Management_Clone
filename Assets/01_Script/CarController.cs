using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class CarController : MonoBehaviour, ICar
{
    private GameManager manager;
    private Transform cashTransform;
    private NavMeshAgent agent;
    public string targetString;
    public string slotString;
    private Transform finish;
    public float waitTime;
    public float parkingTime;
    private Transform turnTarget;
    private void Awake()
    {
        cashTransform = transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        manager = GameManager.Instance;
    }
    Sequence seq;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetCar") && other.name == targetString && gameObject.CompareTag("UsedCar"))
        {
            gameObject.tag = "Untagged";
            float rotY = 0;
            if (other.transform.position.x > 0)
            {
                rotY = -90f;
                finish = GameObject.Find("FinishRight").transform;
            }
            else
            {
                rotY = 90f;
                finish = GameObject.Find("FinishLeft").transform;
            }

            seq = DOTween.Sequence();
            seq.Append(cashTransform.DORotate(new Vector3(0, rotY, 0), 1f));
            seq.OnComplete(() =>
            {
                agent.SetDestination(other.transform.parent.position);
            });
        }
        if (other.CompareTag("ParkSlot") && other.name == slotString)
        {
            other.GetComponent<ParkingAreaController>().isEmpty = false;
            GetComponent<CarUI>().TimerFillToOne();
        }
        if (other.CompareTag("Reset"))
        {
            ResetCar();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ParkSlot") && other.name == slotString)
        {
            agent.enabled = false;
            WaitingAreaManager.Instance.isEmptySlot = true;
        }
        if (other.CompareTag("WaitingArea") && gameObject.CompareTag("Car"))
        {
            agent.angularSpeed = 120;
        }
    }
    public void GoToParkingArea(Transform target,string name, string targetName)
    {
        targetString = targetName;
        slotString = name;
        gameObject.tag = "UsedCar";
        GetComponent<OutlineX>().enabled = false;
        turnTarget = target;
        agent.SetDestination(target.position);
    }
    Sequence seqX;
    public void ParkingAreaWaiting()
    {
        seqX = DOTween.Sequence();
        seqX.Append(transform.DOMoveX(turnTarget.position.x, 1f));
        seqX.Append(transform.DOLookAt(finish.transform.position, 1f));
        seqX.Append(transform.DOMove(finish.transform.position, 5f));
    }
    public void CorGo()
    {
        agent.isStopped = true;
        transform.DOMoveZ(-7, 2f).OnComplete(() => agent.isStopped = false);
        agent.SetDestination(manager.carGoTo.position);
    }
    public void GoToWaitngArea()
    {
        transform.parent = null;
        gameObject.SetActive(true);
        transform.position = new Vector3(CarWaitPos().x, transform.position.y, transform.position.z);
        transform.DOMove(CarWaitPos(), 3f).OnComplete(() => agent.enabled = true);

    }
    private Vector3 CarWaitPos()
    {
        for (int i = 0; i < WaitingAreaManager.Instance.waitingArea.Count; i++)
        {
            GameObject obj = WaitingAreaManager.Instance.waitingArea[i].transform.GetChild(0).gameObject;
            if(obj.GetComponent<WaitingAreaController>().isAvailable == true)
            {
                return obj.transform.position;
            }
        }
        return transform.position;
    }
    private void ResetCar()
    {
        gameObject.tag = "Car";
        targetString = null;
        slotString = null;
        GetComponent<OutlineX>().enabled = false;
        agent.enabled = true;
        cashTransform.parent = manager.carParent;
        CarSpawnManager.Instance.cars.Enqueue(gameObject);
        ObjectPooler.Instance.PushToQueue(PoolObjectType.Car, gameObject,false);
    }
}
