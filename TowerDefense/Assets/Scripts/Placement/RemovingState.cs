using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData gridData;
    private ObjectsDatabaseSO database;
    private ObjectPlacer objectPlacer;
    private BankSystem bank;

    public RemovingState(Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData gridData, ObjectPlacer objectPlacer, BankSystem bank)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.database = database;
        this.bank = bank;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        if (gridData.CanPlaceObjAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = this.gridData;
        }

        if (selectedData == null)
        {
            // sound
            return;
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            if (gameObjectIndex == -1) return;

            int databaseIndex = database.objectsData.FindIndex(data => data.ID == gridData.GetDatabaseIndex(gridPosition));
            bank.DepositPoints(database.objectsData[databaseIndex].Cost);

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !gridData.CanPlaceObjAt(gridPosition, Vector2Int.one);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
