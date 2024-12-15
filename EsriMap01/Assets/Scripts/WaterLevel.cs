using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField] private List<GameObject> water;
    [SerializeField] private GameObject waterMask;
    [SerializeField] private ControlBar conBar;
    [SerializeField] private SceneSwitch sceneSwitch;
    public bool IsRising {  get; set; }

    public float waterLevel;
    public float waterLevelSpeed;
    
    public AnimationCurve waterCurve;
    
    private Color waterColor;
    private Color black;

    // Start is called before the first frame update
    void Start()
    {
        IsRising = true;

        waterColor = waterMask.GetComponent<MeshRenderer>().material.color;
        black = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        bool isBelow = waterLevel > waterMask.transform.position.y;

        foreach (GameObject item in water)
        {
            item.SetActive(!isBelow);
        }

        waterMask.SetActive(isBelow);
        conBar.reduce = isBelow;

        if (conBar.length <= 0 )
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            sceneSwitch.SwitchScene("Lose");
        }

        //waterMask.GetComponent<MeshRenderer>().material.color = Color.Lerp(black, Color.clear, waterCurve.Evaluate(conBar.length));

        if (waterLevel >= 7)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            IsRising = false;
            sceneSwitch.SwitchScene("Win");
        }

        if (!IsRising) return;

        foreach (GameObject obj in water)
        {
            Vector3 waterLevelPos = obj.transform.position;
            waterLevelPos.y = waterLevel;

            obj.transform.position =  waterLevelPos;
        }

        waterLevel += waterLevelSpeed * Time.deltaTime;
    }
}
