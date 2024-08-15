using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;

    private void Update()
    {
        sceneManager.Update();
    }
}
