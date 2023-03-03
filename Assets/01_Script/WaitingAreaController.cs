using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingAreaController : MonoBehaviour
{
    private WaitingAreaManager waitingAreaManager;
    private BoxCollider boxCollider;
    public bool isAvailable = true;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text buyPrizeText;
    public int buyPrize;
    public int index;
    private void Start()
    {
        waitingAreaManager = WaitingAreaManager.Instance;
        boxCollider = GetComponent<BoxCollider>();
        buyPrizeText.text = buyPrize.ToString();
    }
    private void Update()
    {
        IsMoneyEnough();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            isAvailable = false;
            other.GetComponent<CarUI>().TimerFillToZero();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isAvailable = true;
        if (other.CompareTag("UsedCar"))
        {
            GameEvent.WaitingAreaReady();
            other.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }
    public void BuyButton()
    {
        if (IsMoneyEnough())
        {
            canvas.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            boxCollider.enabled = true;
            waitingAreaManager.waitingArea[index].SetActive(true);
            isAvailable = true;
            UIController.Instance.RemoveMoney(buyPrize);
        }
    }

    private bool IsMoneyEnough()
    {
        if (buyPrize <= PlayerPrefs.GetInt("Money"))
        {
            button.GetComponent<Image>().color = Color.green;
            return true;
        }
        else
        {
            button.GetComponent<Image>().color = Color.red;
            return false;
        }
    }
}
