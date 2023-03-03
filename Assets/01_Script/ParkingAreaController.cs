using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParkingAreaController : MonoBehaviour
{
    private BoxCollider boxCollider;
    public Transform target;
    public bool isEmpty = true;

    public TMP_Text numberText;
    public int number;
    private string targetName;
    public string parkingSlotName;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text buyPrizeText;
    public int buyPrize;
    private void Awake()
    {
        numberText.text = number.ToString();

        targetName = number.ToString();
        parkingSlotName = "ParkSlot" + number.ToString();
    }
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        buyPrizeText.text = buyPrize.ToString();
        transform.GetChild(1).name = targetName;
        gameObject.name = parkingSlotName;
    }
    private void Update()
    {
        IsMoneyEnough();
    }
    public void CarIsComing()
    {
        StartCoroutine(ColorChange(Color.green));
    }
    public void CarIsHere()
    {
        StartCoroutine(ColorChange(Color.red));
    }
    private IEnumerator ColorChange(Color colorX)
    {
        GetComponent<Renderer>().material.color = colorX;
        GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        isEmpty = true;
        GameEvent.WaitingAreaReady();
    }
    public void BuyButton()
    {
        if (IsMoneyEnough())
        {
            transform.GetChild(0).gameObject.SetActive(true);
            canvas.SetActive(false);
            boxCollider.enabled = true;
            GameEvent.BuyParkingArea();
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
