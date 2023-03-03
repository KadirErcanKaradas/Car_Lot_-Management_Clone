using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private GameManager manager;
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private int money;

    private void Start()
    {
        manager = GameManager.Instance;
        money = PlayerPrefs.GetInt("Money");
        moneyText.text = money.ToString();
    }

    public void TapToStartButton()
    {
        manager.SetGameStage(GameStage.Started);
        InGamePanel.SetActive(true);
    }
    public void AddMoney(int count)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + count);
        money = PlayerPrefs.GetInt("Money");
        moneyText.text = money.ToString();
    }
    public void RemoveMoney(int count)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - count);
        money = PlayerPrefs.GetInt("Money");
        moneyText.text = money.ToString();
    }
}
