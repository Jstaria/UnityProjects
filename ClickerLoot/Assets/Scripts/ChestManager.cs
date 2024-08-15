using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private HitBox hitBox;
    [SerializeField] private GameObject latch;
    [SerializeField] private int pointsNeeded;
    [SerializeField] private int clicksNeeded = 5;
    public int numItemsDropped = 1;
    [SerializeField] private Transform ChestGlow;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField][Range(0,10)] private float lerpSpeed;
    [SerializeField] private List<int> rarityWeights;

    public int chestID;
    public int Cost { get { return pointsNeeded; } }

    private bool chestDone = false;
    private bool chestOpen = false;
    private List<GameObject> spawnedItems;
    private List<Item> itemRefs;
    private List<LootRarity> toBeSpawnedRarities;

    private LootSystem LootSystem;

    private int[] weightValues;
    private int totalWeight;

    private int clickAttempts;
    private float OGScale = .67f;

    private void Start()
    {
        LootSystem = GameObject.Find("LootSystem").GetComponent<LootSystem>();
        SetupWeights();
        GetRarities();

        hitBox.OnClick += OnClick;
    }

    private void Update()
    {
        ChestGlow.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime));

        float scale = Mathf.Clamp((float)clickAttempts - 1, 0, clicksNeeded) / (float)clicksNeeded * OGScale;

        if (chestDone)
        {
            scale = 0;
            lerpSpeed += .01f;
        }
        else if(chestOpen)
        {
            scale = OGScale * 2;
            lerpSpeed = 10f;
        }

        float blend = MathF.Pow(.5f, Time.deltaTime * lerpSpeed);

        ChestGlow.localScale = Vector3.Lerp(new Vector3(scale, scale, scale), ChestGlow.localScale, blend);
    }

    public void OnClick()
    {
        if (clickAttempts >= clicksNeeded) return;

        clickAttempts++;

        chestAnimator.SetTrigger("WiggleLock");

        Debug.Log("clicked");

        if (clickAttempts >= clicksNeeded) Open();
    }

    private void Open()
    {
        GameObject.Destroy(latch);
        chestAnimator.SetTrigger("Open");

        StartCoroutine(SpawnItem());
    }

    private void GetRarities()
    {
        toBeSpawnedRarities = new List<LootRarity>();

        for (int i = 0; i < numItemsDropped; i++)
        {
            toBeSpawnedRarities.Add(LootSystem.PickRarity(totalWeight, weightValues));
        }


        //toBeSpawnedRarities.Sort();
    }

    private IEnumerator SpawnItem()
    {
        if (spawnedItems != null)
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                GameObject.Destroy(spawnedItems[i]);
            }
        }
        
        spawnedItems = new List<GameObject>();
        itemRefs = new List<Item>();
        yield return new WaitForSeconds(.5f);

        chestOpen = true;

        for (int i = 0; i < numItemsDropped; i++)
        {
            Item item = LootSystem.GetItem(toBeSpawnedRarities[i]);
            itemRefs.Add(item);
            spawnedItems.Add(Instantiate(item.item, SpawnPoint.position, Quaternion.identity, transform));

            float maxOffset = 3;

            spawnedItems[i].GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-maxOffset, maxOffset), 10, Random.Range(-maxOffset, maxOffset));
            spawnedItems[i].GetComponent<Rigidbody>().detectCollisions = false;
            spawnedItems[i].GetComponent<Rigidbody>().freezeRotation = true;

            yield return new WaitForSeconds(.1f);

            spawnedItems[i].GetComponent<Rigidbody>().detectCollisions = true;
        }

        chestDone = true;
        lerpSpeed = 1;

        yield return new WaitForSeconds(2);

        InventorySingleton.Instance.ShowNewItems(itemRefs);
    }
    private void SetupWeights()
    {
        weightValues = new int[rarityWeights.Count];

        for (int i = 0; i < rarityWeights.Count; i++)
        {
            totalWeight += rarityWeights[i];

            weightValues[i] = rarityWeights[i] + (i == 0 ? 0 : weightValues[i - 1]);
        }
    }
}
