using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    public void ChangeToLootbox()
    {
        StartCoroutine(LoadLevel("Lootbox Scene"));
    }

    public void ChangeToPet()
    {
        StartCoroutine(LoadLevel("PetScene"));
    }

    IEnumerator LoadLevel(string scene)
    {
        // Play animation
        //transition.SetTrigger("start");

        // Wait
        yield return new WaitForSeconds(0);

        // LoadScene
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
