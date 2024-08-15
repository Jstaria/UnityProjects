using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GuideState
{
    Talking,
    Chilling
}

[RequireComponent(typeof(Wiggle))]
public class GuideInteraction : MonoBehaviour
{
    [SerializeField] private float ClickCooldown = .5f;
    [SerializeField] private float size = .75f;
    [SerializeField] private AudioLoader audioLoader;

    private GuideState currentState = GuideState.Chilling;

    private float timeClicked;
    private bool inCooldown;

    [SerializeField] private GuideFaceController faceCon;
    [SerializeField] private HitBox hitBox;
    [SerializeField] private Wiggle wiggle;
    [SerializeField] private DialogueCon dialogueCon;
    [SerializeField] private TextAsset text;

    private List<List<string>> randomLines;
    private int lastIndex;

    public void Start()
    {
        currentState = GuideState.Chilling;
        dialogueCon.SpeechAudio = audioLoader.clip["SoundEffects"][0];

        wiggle.OnSquiggle += faceCon.Squiggle;
        wiggle.SizeScale = size;

        GetRandomLines();
    }

    private void GetRandomLines()
    {
        string textData = text.text;

        string[] data = textData.Split("\r\n");

        randomLines = new List<List<string>>();
        int index = 0;

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == "###START")
            {
                randomLines.Add(new List<string>());
                continue;
            }
            if (data[i] == "###END")
            {
                index++;
                continue;
            }

            randomLines[index].Add(data[i]);
        }
    }

    public void Update()
    {
        inCooldown = Time.time - timeClicked <= ClickCooldown;

        wiggle.WiggleMax = OnMouseOver() || dialogueCon.IsTalking;
        wiggle.LerpMax = inCooldown;

    }

    private bool OnMouseOver()
    {
        return hitBox.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnClick()
    {
        if (Time.time - timeClicked <= ClickCooldown) return;

        switch (currentState)
        {
            case GuideState.Chilling:

                Debug.Log(randomLines.Count);

                int ran = Random.Range(0, randomLines.Count);

                dialogueCon.StartDialogue(.8f, 0, randomLines[ran]);
                currentState = GuideState.Talking;
                break;

            case GuideState.Talking:
                currentState = dialogueCon.NextSpeech() ? GuideState.Talking : GuideState.Chilling;
                break;
        }

        wiggle.OnClick();
        timeClicked = Time.time;
    }
}
