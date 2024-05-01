using System;
using System.Collections;
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

    [SerializeField] private AudioClip speechAudio;
    [SerializeField] private AudioSource speechSource;
    [SerializeField] private AudioSource speechSource2;
    [SerializeField] private float dialogueCooldown = .1f;
    [SerializeField] private float dialogueCDFlux = .05f;
    [SerializeField] private float maxPitchFlux = .5f;
    [SerializeField] private float textSpeed = .01f;
    private float lastDialogueTime;
    private float CD;
    private bool speechSourceSwitch;
    private string[] textToBeDisplayed = new string[] { "Hi there Stranger!" , "I'm gonna put you \nthrough hell!" };

    private int index;

    private float pitchMod;
    private float CDMod;

    private float lerpSpeed;

    private Vector3 desiredPosition;
    private Vector3 defaultPosition;
    private Vector3 downPosition;

    public bool IsTalking { get; private set; }

    private void Start()
    {
        CD = dialogueCooldown;
        speechSource.clip = speechAudio;
        speechSource2.clip = speechAudio;

        defaultPosition = panelTransform.position;
        downPosition = panelTransform.position - new Vector3(0, 400);

        panelTransform.position = downPosition;
        text.text = string.Empty;
    }

    private void Update()
    {
        float blend = 1 - MathF.Pow(.5f, lerpSpeed * Time.deltaTime);
        panelTransform.position = Vector3.Lerp(panelTransform.position, desiredPosition, blend);
    }

    public void StartDialogue(float pitchMod, float CDMod, string[] lines)
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
        foreach (char c in textToBeDisplayed[index].ToCharArray())
        {
            text.text += c;
            Speak(pitchMod, CDMod);
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
        if (index >= textToBeDisplayed.Length - 1)
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
