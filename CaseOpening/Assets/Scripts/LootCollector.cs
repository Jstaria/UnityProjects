using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootCollector : Singleton<LootCollector>
{
    private List<GameObject> wonItems;
    private float xOffset = 20;

    private bool suckActive;
    private bool isLastItemCoroutineActive;

    [SerializeField] private Transform suckPosition;

    [SerializeField] private Transform suckTube;
    private Spring tubeSpring;
    private float tubeSpringPos;
    private float tubeSpringPosUp;

    private void Start()
    {
        tubeSpringPos = suckTube.position.y;
        tubeSpringPosUp = suckTube.position.y + 5;

        tubeSpring = new Spring(20, .5f, tubeSpringPosUp, true);
    }

    private void Update()
    {
        tubeSpring.Update();

        Vector3 position = transform.position;
        position.y = tubeSpring.Position;
        transform.position = position;
    }

    public void ClearWonItems()
    {
        for (int i = 0; i < wonItems.Count; i++)
        {
            Destroy(wonItems[i]);
        }

        wonItems.Clear();
    }

    public void AddItem(GameObject item, float initialBounceSpeed)
    {
        if (wonItems == null)
            wonItems = new List<GameObject>();

        xOffset -= .01f;

        Vector3 position = item.transform.position;
        position.x -= xOffset;
        item.transform.position = position;

        Bounce itemBounce = item.AddComponent<Bounce>();
        item.AddComponent<Suck>();
        wonItems.Add(item);
        itemBounce.StartBounceY(initialBounceSpeed, -15);
    }

    public IEnumerator ItemStackSuck()
    {
        tubeSpring.RestPosition = tubeSpringPos;
        suckActive = true;

        float timeInBetween = .5f;

        for (int i = 0; i < wonItems.Count; i++)
        {
            Destroy(wonItems[i].GetComponent<Bounce>());
            StartCoroutine(ItemSuck(i));

            yield return new WaitForSeconds(timeInBetween);

            timeInBetween *= .85f;
            timeInBetween = Mathf.Clamp(timeInBetween, .025f, int.MaxValue);

            isLastItemCoroutineActive = true;

            yield return new WaitForNextFrameUnit();
        }

        while (isLastItemCoroutineActive)
        {
            yield return null;
        }

        tubeSpring.RestPosition = tubeSpringPosUp;

        suckActive = false;
    }

    private IEnumerator ItemSuck(int i)
    {
        wonItems[i].GetComponent<Suck>().StartSuck(suckPosition.position, 10);

        Vector2 position1 = new Vector2(wonItems[i].transform.position.y, wonItems[i].transform.position.z);
        Vector2 position2 = new Vector2(suckPosition.position.y, suckPosition.position.z);

        while (Vector2.Distance(position1, position2) > 1)
        {
            position1 = new Vector2(wonItems[i].transform.position.y, wonItems[i].transform.position.z);
            position2 = new Vector2(suckPosition.position.y, suckPosition.position.z);

            yield return null;
        }

        if (i + 1 == wonItems.Count)
            isLastItemCoroutineActive = false;
    }
}
