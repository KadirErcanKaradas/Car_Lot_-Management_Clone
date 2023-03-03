using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CarUI : MonoBehaviour
{
    private CarController carController;
    private UIController uIController;
    [SerializeField] private GameObject FillImage;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private int money;
    private void Start()
    {
        carController = GetComponent<CarController>();
        uIController = UIController.Instance;
        int moneyNumber = Random.Range(20, 50);
        money = moneyNumber;
        moneyText.text = money.ToString();
    }
    Sequence timerZeroSequence;
    public void TimerFillToZero()
    {
        if (timerZeroSequence != null && timerZeroSequence.IsPlaying())
            return;
        timerZeroSequence = DOTween.Sequence();
        Image fill = FillImage.GetComponent<Image>();
        fill.fillAmount = 1;
        FillImage.SetActive(true);
        timerZeroSequence.Join(fill.DOFillAmount(0, carController.waitTime));
        timerZeroSequence.OnComplete(() =>
        {
            if (gameObject.CompareTag("Car"))
            {
                gameObject.tag = "Untagged";
                carController.CorGo();
            }
        });

    }
    Sequence timerOneSequence;
    public void TimerFillToOne()
    {
        if (timerOneSequence != null && timerOneSequence.IsPlaying())
            return;
        timerOneSequence = DOTween.Sequence();
        Image fill = FillImage.GetComponent<Image>();
        fill.fillAmount = 0;
        FillImage.SetActive(true);
        timerOneSequence.Join(fill.DOFillAmount(1, carController.waitTime));
        timerOneSequence.OnComplete(() =>
        {
            if (gameObject.CompareTag("Untagged"))
            {
                carController.ParkingAreaWaiting();
                uIController.AddMoney(money);
            }
        });

    }
}
