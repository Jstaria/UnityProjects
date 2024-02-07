using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private PlacementInput inputMan;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisual;
    private int selectedObjIndex = -1;

    private GridData objData;
    private List<GameObject> placedGameObjects = new();

    [SerializeField] private PreviewSystem preview;
    private Vector3Int lastDetectedPos = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();

        objData = new GridData();
    }

    /// <summary>
    /// Starts build mode on the grid
    /// </summary>
    /// <param name="ID"></param>
    public void StartPlacement(int ID)
    {
        Debug.Log("Was Clicked");
        StopPlacement();
        selectedObjIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObjIndex < 0) { Debug.Log($"No ID found {ID}"); return; }

        gridVisual.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjIndex].Prefab,
                                             database.objectsData[selectedObjIndex].Size);

        inputMan.OnClicked += PlaceStructure;
        inputMan.OnExit += StopPlacement;
    }

    /// <summary>
    /// Places structure at current grid coordinates if that placement is valid
    /// </summary>
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

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// Returns if the selected grid position already has objects in it
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="selectedObjIndex"></param>
    /// <returns></returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        GridData selectedData = objData;

        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    }

    /// <summary>
    /// Exits build mode
    /// </summary>
    private void StopPlacement()
    {
        selectedObjIndex = -1;

        gridVisual.SetActive(false);
        preview.StopShowingPreview();

        inputMan.OnClicked -= PlaceStructure;
        inputMan.OnExit -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
    }

    private void Update()
    {
        if (selectedObjIndex < 0) return;

        Vector3 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);

            mouseIndicator.transform.position = mousePos;

            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);

            lastDetectedPos = gridPosition;
        }

    }
}
