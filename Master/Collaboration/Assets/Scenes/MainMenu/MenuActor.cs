using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActor : MonoBehaviour
{
    public void LoadLevel(string level_name)
    {
        StartCoroutine(LoadScene(level_name));
    }

    IEnumerator LoadScene (string level_name)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(level_name);
    }

    public void ApplicationQuit()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame ()
    {
        yield return new WaitForSeconds(.5f);
        Application.Quit();
    }
}