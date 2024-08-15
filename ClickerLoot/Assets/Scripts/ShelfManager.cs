using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public List<GameObject> displayChests;

    public int selectedID = 0;
    public GameObject selectedChest;

    private GameObject instantiatedChest;

    private Transform pedestal;

    private void Start()
    {
        pedestal = GameObject.Find("Pedestal").transform;

        CameraArm.Instance.transform.position = new Vector3(37, 16, -15.75f);
        CameraArm.Instance.transform.rotation = Quaternion.Euler(new Vector3(20, 90, 0));

        CameraArm.Instance.desiredPosition = new Vector3(37, 16, -15.75f);
        CameraArm.Instance.desiredRotation = new Vector3(20, 90, 0);

        for (int i = 0; i < displayChests.Count; i++)
        {
            displayChests[i].GetComponent<DisplayChest>().HitBox.OnClick += displayChests[i].GetComponent<DisplayChest>().OnClick;
            displayChests[i].GetComponent<DisplayChest>().HitBox.OnClick += DisplayChestOnClick;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && InventorySingleton.Instance.isInvOpen)
        {
            InventorySingleton.Instance.HideInventory();
            ViewManager.Instance.SetPosition("Chest Shelf");
        }
    }

    public void DisplayChestOnClick()
    {
        if (PointManager.Instance.currentPoints < selectedChest.GetComponent<ChestManager>().Cost) return;

        PointManager.Instance.RemovePoints(selectedChest.GetComponent<ChestManager>().Cost);

        if (instantiatedChest != null) GameObject.Destroy(instantiatedChest);

        instantiatedChest = Instantiate(selectedChest, pedestal.position, pedestal.rotation, pedestal);

        ViewManager.Instance.SetPosition("Chest");
    }
}
