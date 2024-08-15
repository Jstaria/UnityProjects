using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private GuideCon guideCon;
    [SerializeField] private DialogueCon dialogueCon;
    [SerializeField] private BackgroundMusicCon bgMusicCon;

    private void Start()
    {
        guideCon.Start();
        dialogueCon.Start();
    }

    private void Update()
    {
        guideCon.Update();
        dialogueCon.Update();
        bgMusicCon.Update();
        sceneManager.Update();
    }
}
