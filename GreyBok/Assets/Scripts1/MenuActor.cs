using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActor : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string level_name)
    {
        SceneManager.LoadScene(level_name);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}