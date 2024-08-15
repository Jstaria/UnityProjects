using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using UnityEngine;

struct PointData
{
    public int points;
    public int multiplier;
}

public class PointManager : Singleton<PointManager>
{
    public int currentPoints { get; private set; }
    private int multiplier = 1;
    private TextMeshProUGUI pointTMP;

    private string dataDirPath;
    private string dataFileName = "PointsSaveData";

    private TextAsset pointsSaveData;

    private void Start()
    {
        dataDirPath = Application.persistentDataPath;

        pointTMP = GameObject.Find("PointText").GetComponent<TextMeshProUGUI>();
        pointTMP.text = "Points: " + currentPoints;

        LoadPointSaveData();
    }

    public void AddPoints()
    {
        currentPoints += multiplier;

        UpdatePointsText();
        SavePointsSaveData();
    }

    private void UpdatePointsText()
    {
        pointTMP.text = "Points: " + currentPoints;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePointsSaveData();
        }
    }

    public void SetMultiplier()
    {

    }

    public void LoadPointSaveData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (!File.Exists(fullPath)) return;

        string data = "";

        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }
        }

        PointData loadedData = JsonUtility.FromJson<PointData>(data);

        currentPoints = loadedData.points;
        multiplier = loadedData.multiplier;

        UpdatePointsText();
    }

    public void SavePointsSaveData()
    {
        PointData pointData = new PointData();

        pointData.points = currentPoints;
        pointData.multiplier = multiplier;

        // https://youtu.be/aUi9aijvpgs

        string fullPath = Path.Combine(dataDirPath, dataFileName);

         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string dataToStore = JsonUtility.ToJson(pointData, true);

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }

        UpdatePointsText();
    }

    internal void RemovePoints(int cost)
    {
        currentPoints = Mathf.Clamp(currentPoints - cost, 0, int.MaxValue);

        SavePointsSaveData();
    }
}
