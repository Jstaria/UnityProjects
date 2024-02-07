using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private PlacementInput inputMan;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisual;

    private GridData objData;

    [SerializeField] private PreviewSystem preview;
    private Vector3Int lastDetectedPos = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    private IBuildingState buildingState;

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

        gridVisual.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, objData, objectPlacer);

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

        buildingState.OnAction(gridPosition);
    }

    /// <summary>
    /// Exits build mode
    /// </summary>
    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisual.SetActive(false);

        buildingState.EndState();

        inputMan.OnClicked -= PlaceStructure;
        inputMan.OnExit -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePos = inputMan.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPosition)
        {
            buildingState.UpdateState(gridPosition);

            lastDetectedPos = gridPosition;
        }

    }
}
