using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum Decision
{
    Hit,
    Stand,
    Surrender,
    Split,
    DoubleDown
}
public class Dealer : MonoBehaviour
{
    public CardDeck deck;

    public Animator flipAnimator;
    public Transform cardHolder;
    public Transform rotationHolder;

    public float startingLerpSpeed = 10;

    private List<GameObject> availableCards;

    public Transform cardSpawn;

    public Hand dealersHand;

    public List<Hand> playerHands;
    public float dealSpeed;

    public UnityEvent OnBettingStart;

    public UnityEvent OnPlayerTurn;

    public UnityEvent OnEndTurn;

    public UnityEvent OnRoundEnd;

    public DealerAI dealerAI;

    public UnityEvent OnBeginDeal;

    public AudioManager audioManager;

    #region Start

    private void Update()
    {
        GlobalVariables.DealerIsFlipping = flipAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flipped");
    }

    public void BeginDealer()
    {
        StartCoroutine(StartDealer());
    }

    private IEnumerator StartDealer()
    {
        yield return GetBets();

        OnBeginDeal.Invoke();

        yield return DealDealer();

        yield return DealPlayers();

        OnPlayerTurn.Invoke();

        yield return null;
    }

    private IEnumerator GetBets()
    {
        GlobalVariables.InBettingStage = true;

        OnBettingStart.Invoke();

        while (GlobalVariables.InBettingStage)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    private void Shuffle()
    {
        //dealersHand.StartDeleteHand();

        //for (int i = 0; i < playerHands.Count; i++)
        //{
        //    playerHands[i].StartDeleteHand();
        //}

        availableCards = new List<GameObject>();
        availableCards.AddRange(deck.cards);
    }

    private IEnumerator DealDealer()
    {
        UnFlipCard();
        Shuffle();

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 2; i++)
        {
            SpawnCard(-1, dealersHand, 0, PickCardFromAvailable(), i == 1, .45f);

            yield return new WaitForSeconds(dealSpeed);
        }
        yield return null;
    }

    private IEnumerator DealPlayers()
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < playerHands.Count; i++)
            {
                SpawnCard(i, playerHands[i], 0, PickCardFromAvailable(), false, .25f);

                yield return new WaitForSeconds(dealSpeed);
            }
        }

        yield return null;
    }

    private GameObject PickCardFromAvailable()
    {
        int rand = Random.Range(0, availableCards.Count);

        GameObject tempCard = availableCards[rand];

        availableCards.RemoveAt(rand);

        return tempCard;
    }

    #endregion

    #region DuringPlay

    private void SpawnCard(int v, Hand hand, int deck, GameObject tempCard, bool isFlipped, float cardSpacing)
    {
        float cardOffset = .05f;

        Transform cardSpot = hand.gameObject.transform;
        List<GameObject> cards = hand.Hands[deck];

        Vector3 rotation = v == -1 ? (isFlipped ? new Vector3(0, 0, 180) + cardSpot.rotation.eulerAngles : cardSpot.rotation.eulerAngles) + new Vector3(0, Random.Range(0, 5)) : cardSpot.rotation.eulerAngles + new Vector3(0, Random.Range(0, 5)); ;
        Vector3 position = cardSpot.position + -cardSpacing * cardSpot.right + cards.Count * cardSpacing * cardSpot.right + new Vector3(Random.Range(-cardOffset, cardOffset), cards.Count * .0001f, Random.Range(-cardOffset, cardOffset));
        Vector3 startRotation = rotation + new Vector3(0, Random.Range(270, 359), 0);
        startRotation.y = Mathf.Min(startRotation.y, 359);

        if (isFlipped) rotationHolder.position = position;

        hand.AddCard(deck, Instantiate(tempCard, cardSpawn.position, Quaternion.Euler(startRotation), isFlipped ? cardHolder : cardSpot));
        StartCoroutine(cards[cards.Count - 1].GetComponent<Card>().MoveTo(position, rotation, startingLerpSpeed, 5));

        audioManager.PlayClip(Random.Range(0, audioManager.clips.Count), .1f);

        hand.GetHandValue();
    }

    public void OnDecisionMade(Decision decision)
    {
        Hand currentHand = GameManager.currentPlayer.hand;

        switch (decision)
        {
            case Decision.Stand:
                OnEndTurn.Invoke();
                break;

            case Decision.Hit:
                SpawnCard(0, currentHand, 0, PickCardFromAvailable(), false, .25f);

                if (currentHand.CheckBust()) currentHand.OnBust.Invoke();

                break;
        }
    }

    public void OnDealerDecision(Decision decision)
    {
        Hand currentHand = dealersHand;

        switch (decision)
        {
            case Decision.Stand:
                GlobalVariables.RoundIsReady = false;

                OnEndTurn.Invoke();
                OnRoundEnd.Invoke();
                break;

            case Decision.Hit:
                SpawnCard(-1, currentHand, 0, PickCardFromAvailable(), false, .45f);

                if (currentHand.CheckBust()) currentHand.OnBust.Invoke();

                break;
        }
    }

    public void FlipCard()
    {
        flipAnimator.ResetTrigger("UnFlip");
        flipAnimator.SetTrigger("Flip");
    }

    public void UnFlipCard()
    {
        flipAnimator.SetTrigger("UnFlip");
        flipAnimator.ResetTrigger("Flip");
    }

    #endregion
}
