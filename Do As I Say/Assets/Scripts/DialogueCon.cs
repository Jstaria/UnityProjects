using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource), typeof(Wiggle))]
public class DialogueCon : MonoBehaviour
{
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private TextMeshProUGUI text;

    private AudioClip speechAudio;
    [SerializeField] private AudioSource speechSource;
    [SerializeField] private AudioSource speechSource2;
    [SerializeField] private float dialogueCooldown = .1f;
    [SerializeField] private float dialogueCDFlux = .05f;
    [SerializeField] private float maxPitchFlux = .5f;

    private float lastDialogueTime;
    private float CD;
    private bool speechSourceSwitch;
    private List<string> textToBeDisplayed = new List<string>() { "Hi there Stranger!" , "I'm gonna put you \nthrough hell!" };

    private int index;

    private float pitchMod;
    private float CDMod;

    private float lerpSpeed;

    private Vector3 desiredPosition;
    private Vector3 defaultPosition;
    private Vector3 downPosition;

    public AudioClip SpeechAudio 
    { 
        get 
        { 
            return speechAudio; 
        } 
        set 
        { 
            speechAudio = value;

            speechSource.clip = speechAudio;
            speechSource2.clip = speechAudio;
        } 
    }
    public bool IsTalking { get; private set; }

    public void Start()
    {
        CD = dialogueCooldown;
        
        defaultPosition = panelTransform.position;
        downPosition = panelTransform.position - new Vector3(0, 3.25f);

        panelTransform.position = downPosition;
        text.text = string.Empty;
    }

    public void Update()
    {
        float blend = 1 - MathF.Pow(.5f, lerpSpeed * Time.deltaTime);
        panelTransform.position = Vector3.Lerp(panelTransform.position, desiredPosition, blend);
    }

    public void StartDialogue(float pitchMod, float CDMod, List<string> lines)
    {
        textToBeDisplayed = lines;
        index = 0;
        StartSpeech(pitchMod, CDMod);
    }

    private void StartSpeech(float pitchMod, float CDMod)
    {
        StopAllCoroutines();
        text.text = string.Empty;
        this.pitchMod = pitchMod;
        this.CDMod = CDMod;
        desiredPosition = defaultPosition;
        lerpSpeed = 7f;

        StartCoroutine(StartType());
    }

    private IEnumerator StartType()
    {
        string displayText = textToBeDisplayed[index];

        for (int i = 0; i < displayText.Length; i++)
        {
            char c = displayText[i];

            if (displayText[i] == '\\')
            {
                text.text += Environment.NewLine;
            }
            else
            {
                text.text += c;
                Speak(pitchMod, CDMod);
            }
            
            yield return new WaitForSeconds(CD);
        }
    }

    private void Speak(float pitchMod, float CDMod)
    {

        AudioSource current = speechSourceSwitch ? speechSource : speechSource2;
        speechSourceSwitch = !speechSourceSwitch;

        current.pitch = Mathf.Clamp(Random.Range(-maxPitchFlux, maxPitchFlux / 2) + pitchMod, .15f, 2);
        current.Play();

        lastDialogueTime = Time.time;
        CD = dialogueCooldown + Random.Range(-dialogueCDFlux / 2, dialogueCDFlux) + CDMod;

    }

    internal bool NextSpeech()
    {
        StopAllCoroutines();

        if (index >= textToBeDisplayed.Count - 1)
        {
            lerpSpeed = 4f;
            desiredPosition = downPosition;

            return false;
        }

        index++;
        StartSpeech(pitchMod, CDMod);
        return true;
    }
}
