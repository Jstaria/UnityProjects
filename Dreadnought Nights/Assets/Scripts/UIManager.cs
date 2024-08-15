using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public AudioManager AudioManager;
    public List<TableUIHandler> UI;

    public void SetInActive()
    {
        foreach (TableUIHandler item in UI)
        {
            item.BeginEndGlow();
        }

        AudioManager.PlayClip(1, .1f);
        AudioManager.UnLoopSource();
    }

    public void SetActive()
    {
        foreach (TableUIHandler item in UI)
        {
            item.BeginStartGlow();
        }

        AudioManager.PlayClip(0, .1f);
        AudioManager.LoopClip(2);
    }
}
