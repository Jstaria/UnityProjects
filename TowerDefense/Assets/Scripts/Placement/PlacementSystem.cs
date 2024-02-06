using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private PlacementInput inputMan;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisual;
    private int selectedObjIndex = -1;

    private GridData objData;
    private Renderer previewRenderer;
    private List<GameObject> placedGameObjects = new();

    private void Start()
    {
        StopPlacement();

        objData = new GridData();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        Debug.Log("Was Clicked");
        StopPlacement();
        selectedObjIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObjIndex < 0) { Debug.Log($"No ID found {ID}"); return; }

        gridVisual.SetActive(true);
        cellIndicator.SetActive(true);

        inputMan.OnClicked += PlaceStructure;
        inputMan.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputMan.IsPointerOverUI()) return;

        Vector3 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);

        if (!placementValidity) return;

        GameObject newObject = Instantiate(database.objectsData[selectedObjIndex].Prefab);

        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);

        GridData selectedData = objData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjIndex].Size,
            database.objectsData[selectedObjIndex].ID,
            placedGameObjects.Count - 1);

    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        GridData selectedData = objData;

        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjIndex = -1;

        gridVisual.SetActive(false);
        cellIndicator.SetActive(false);

        inputMan.OnClicked -= PlaceStructure;
        inputMan.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjIndex < 0) return;

        Vector3 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
