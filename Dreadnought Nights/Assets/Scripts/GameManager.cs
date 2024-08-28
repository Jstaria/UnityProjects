using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnStartFirstTime;
    public UnityEvent StartEvent;
    public UnityEvent<List<int>, int> OnBlackJack;
    public List<IPlayer> players;

    public static IPlayer currentPlayer;
    public int currentPlayerInt;
    public bool blackJackNotFound;

    public PlayerLightController playerLightCon;

    private void Start()
    {
        OnStartFirstTime.Invoke();
        StartEvents();
        SetRoundReady();
    }

    public void StartEvents()
    {
        if (StartEvent != null) StartEvent.Invoke();
    }

    public void StartBeginTurns()
    {
        StartCoroutine(BeginTurns());
    }

    private IEnumerator BeginTurns()
    {
        CheckForBlackJacks();

        while (!GlobalVariables.IsGameReadyToStart())
        {
            //Debug.Log("IM STUCK");
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].winState == WinState.BlackJack) continue;

            currentPlayerInt = i;

            yield return new WaitForSeconds(1);

            IPlayer player = players[i];
            if (player as PlayerAI) ((PlayerAI)player).StartTurn();
            if (player as DealerAI) ((DealerAI)player).StartTurn();
            else player.StartTurn();
            currentPlayer = player;

            while (player.IsInTurn())
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void CheckForBlackJacks()
    {
        List<int> ints = new List<int>();

        players[players.Count - 1].winState = WinState.Lost;

        for (int i = 0; i < players.Count - 1; i++)
        {
            players[i].winState = WinState.Lost;

            if (players[i].hand.GetHandValue() == 21)
            {
                ints.Add(i);
                players[i].winState = WinState.BlackJack;
            }
        }

        if (blackJackNotFound = ints.Count == 0) return;

        GlobalVariables.IsInBlackJackPhase = true;
        OnBlackJack.Invoke(ints, currentPlayerInt);
    }

    public void OnEndTurn()
    {
        currentPlayer.EndTurn();
    }

    public void StartOnBust()
    {
        StartCoroutine (OnBust());
    }

    public IEnumerator OnBust()
    {
        yield return new WaitForSeconds(1);

        if (currentPlayer.hand.Hands[1] != null)
        {
            currentPlayer.hand.currentHand = 1;
        }
        else
        {
            OnEndTurn();
        }
    }

    public void StartSweep()
    {
        StartCoroutine(Sweep());
    }

    private IEnumerator Sweep()
    {
        foreach (IPlayer player in players)
        {
            player.hand.StartSweepHand();
        }

        StartCoroutine(playerLightCon.SweepLights());

        foreach (IPlayer player in players)
        {
            StartCoroutine(player.hand.DeleteHand());
            if (player.chipBank != null && !(player is DealerAI)) player.chipBank.AddBetWin(player.winState);
            player.winState = WinState.Lost;
        }

        while (!AreAllDecksEmpty())
        {
            yield return new WaitForEndOfFrame();
        }

        GlobalVariables.RoundIsReady = true;
    }

    private bool AreAllDecksEmpty()
    {
        foreach (IPlayer player in players)
        {
            if (!player.hand.isHandEmpty) return false; 
        }

        return true;
    }

    public void AllowPlayerBetting()
    {
        for (int i = 0; i < 3; i++)
        {
            if (players[i].chipBank != null) players[i].chipBank.StartBetting();
        }
        
    }

    public void SetRoundReady()
    {
        GlobalVariables.RoundIsReady = true;
    }
}
