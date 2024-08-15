using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float waitTime = 1;
    public void ChangeToRoom()
    {
        StartCoroutine(LoadLevel("Room"));
    }

    public void ChangeToMenu()
    {
        StartCoroutine(LoadLevel("Menu"));
    }

    public void ChangeToOptions()
    {
        StartCoroutine(LoadLevel("Options"));
    }

    public void ChangeToCredits()
    {
        StartCoroutine(LoadLevel("Credits"));
    }

    IEnumerator LoadLevel(string scene)
    {
        // Play animation
        transition.SetTrigger("start");

        // Wait
        yield return new WaitForSeconds(waitTime);

        // LoadScene
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
