using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankSystem : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private TextMeshProUGUI text;

    public int Points { get; private set; }

    private void Start()
    {
        UpdateText();
    }

    public void DepositPoints(int amount)
    {
        points += amount;
        UpdateText();
    }

    public void WithdrawPoints(int amount)
    {
        points -= amount;
        UpdateText();
    }

    public bool CheckWithdrawalValidity(int amount)
    {
        return points - amount >= 0;
    }

    private void UpdateText()
    {
        text.text = points.ToString();
    }
}
