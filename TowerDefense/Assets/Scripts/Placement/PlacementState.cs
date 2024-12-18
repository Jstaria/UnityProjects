using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int ID;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectsDatabaseSO database;
    private GridData gridData;
    private ObjectPlacer objectPlacer;
    private BankSystem bank;
    private WaveSpawner waveSpawner;
    private TowerManager towerManager;

    public PlacementState(int ID, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData gridData, ObjectPlacer objectPlacer, BankSystem bank, WaveSpawner waveSpawner, TowerManager towerManager)
    {
        this.ID = ID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.bank = bank;
        this.waveSpawner = waveSpawner;
        this.towerManager = towerManager;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
            database.objectsData[selectedObjectIndex].Prefab,
            database.objectsData[selectedObjectIndex].Size);

        }
        else throw new System.Exception($"No object with ID {ID}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        int withdrawalAmount = database.objectsData[selectedObjectIndex].Cost;
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex) && bank.CheckWithdrawalValidity(withdrawalAmount);

        if (!placementValidity) return;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = gridData;

        Tower newTower = new Tower(selectedObjectIndex, database.objectsData[selectedObjectIndex].Prefab.transform.Find("Head").gameObject, waveSpawner, database.objectsData[selectedObjectIndex].ActiveRadius);

        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index,
            newTower);

        bank.WithdrawPoints(withdrawalAmount);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// Returns if the selected grid position already has objects in it
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="selectedObjIndex"></param>
    /// <returns></returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        GridData selectedData = gridData;

        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
