using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : NetworkBehaviour
{
    public List<Card> currentDeck;
    public List<GameObject> currentDeckCards;

    public Card defaultCard;

    public Camera camera;

    public float desiredWidth = 17;
    public float cardOffset = .5f;
    public float minOffset = -1;
    public Vector3 deckOffset;
    private Vector3 defaultDeckPos;
    private Vector3 cardPosition;
    private Vector3 deckPosition;

    public float lerpSpeed = 2;

    public Transform spawnPoint;

    public List<GameObject> playerSpecific;

    public HitBox hitBox;
    private bool hit;

    private CardInfo cardInfo;

    private void Start()
    {
        currentDeckCards = new List<GameObject>();
        currentDeck = new List<Card>();

        defaultDeckPos = transform.localPosition;

        for (int i = 0; i < playerSpecific.Count; i++)
        {
            playerSpecific[i].SetActive(IsOwner);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        SetCardPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddCard(defaultCard);
        }

        Vector2 view = camera.ScreenToViewportPoint(Input.mousePosition);
        bool isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        hit = hitBox.IsMouseOver() && !isOutside;

        Debug.Log(hit);

        deckPosition = hit ? defaultDeckPos + deckOffset : defaultDeckPos;
        cardPosition = hit ? transform.parent.position + defaultDeckPos + deckOffset : transform.parent.position + defaultDeckPos;
    }

    private void SetCardPosition()
    {
        if (currentDeck.Count != 0)
        {
            float width = GetWorldSize(defaultCard.prefab.GetComponent<SpriteRenderer>()).x * transform.localScale.x;

            float desiredCardOffset = Mathf.Min((desiredWidth - (currentDeck.Count * width)) / (currentDeck.Count - 1), minOffset);

            float deckWidth = width * currentDeckCards.Count + desiredCardOffset * (currentDeckCards.Count - 1);
            float currentDeckWidth = width * currentDeckCards.Count + cardOffset * (currentDeckCards.Count - 1);

            cardInfo = null;
            float distanceFromCenterLow = int.MaxValue;

            for (int i = 0; i < currentDeck.Count; i++)
            {
                CardInfo currentCard = currentDeckCards[i].GetComponent<CardInfo>();
                currentCard.DesiredPos = cardPosition + new Vector3(
                  (i * width + i * desiredCardOffset) - ((currentDeck.Count * width + (currentDeck.Count - 1) * desiredCardOffset) / 2) + width / 2, 0);

                float distance = Mathf.Abs(currentCard.DistanceFromCenter(camera.ScreenToWorldPoint(Input.mousePosition)));

                if (distance < distanceFromCenterLow)
                {
                    distanceFromCenterLow = distance;
                    cardInfo = currentCard;
                }
            }

            if (cardInfo != null && hit)
            {
                cardInfo.InFocus = true;
                cardInfo.DesiredPos += new Vector3(0, 2, 0);
            }
        }

        transform.localPosition = Vector3.Lerp(deckPosition, transform.localPosition, Mathf.Pow(.5f, lerpSpeed * 4 * Time.deltaTime));
    }

    private void AddCard(Card card)
    {
        if (currentDeck.Count >= 10) return;
        currentDeck.Add(card);
        currentDeckCards.Add(Instantiate(card.prefab, spawnPoint.position, Quaternion.identity, transform));
        currentDeckCards[currentDeckCards.Count - 1].GetComponent<CardInfo>().LerpSpeed = lerpSpeed;
    }

    Vector2 GetWorldSize(SpriteRenderer spriteRenderer)
    {
        // Get the local size of the sprite in world units
        Sprite sprite = spriteRenderer.sprite;
        Vector2 localSize = new Vector2(sprite.rect.width / sprite.pixelsPerUnit, sprite.rect.height / sprite.pixelsPerUnit);

        // Get the world scale of the sprite considering parent scales
        Vector3 worldScale = GetWorldScale(spriteRenderer.transform);

        // Calculate the world size
        Vector2 worldSize = new Vector2(localSize.x * worldScale.x, localSize.y * worldScale.y);

        return worldSize;
    }

    Vector3 GetWorldScale(Transform obj)
    {
        Vector3 worldScale = obj.localScale;
        Transform parent = obj.parent;

        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }

        return worldScale;
    }
}
