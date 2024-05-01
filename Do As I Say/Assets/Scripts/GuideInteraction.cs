using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuideState
{
    Talking,
    Chilling
}

[RequireComponent(typeof(Wiggle))]
public class GuideInteraction : MonoBehaviour
{
    [SerializeField] private float ClickCooldown = .5f;

    private GuideState currentState;

    private float timeClicked;
    private bool inCooldown;

    private GuideFaceController faceCon;
    private HitBox hitBox;
    private Wiggle wiggle;
    private DialogueCon dialogueCon;

    private List<string[]> RandomLines;

    private void Start()
    { 
        currentState = GuideState.Chilling;
        faceCon = GetComponent<GuideFaceController>();
        hitBox = GetComponent<HitBox>();
        wiggle = GetComponent<Wiggle>();
        dialogueCon = GetComponent<DialogueCon>();

        wiggle.OnSquiggle += faceCon.Squiggle;
        hitBox.OnHitBoxTrigger += OnClick;

        GetRandomLines();
    }

    private void GetRandomLines()
    {
        List<string> data = FileIO.ReadFrom("..//Dialogues//RandomGuide");
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

    private void OnClick()
    {
        if (Time.time - timeClicked <= ClickCooldown) return;

        switch (currentState)
        {
            case GuideState.Chilling:
                dialogueCon.StartDialogue(.8f, 0, RandomLines[0]);
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
