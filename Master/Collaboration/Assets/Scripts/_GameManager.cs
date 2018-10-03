using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour {

    #region Singleton
    public static _GameManager instance;

    private void Awake()
    {
        if (instance != this)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public GameObject player;
    public GameObject heart;

    public bool gameOver;

    public List<GameObject> enemies = new List<GameObject>();

    public GameObject bossBot;

    private void Update()
    {
        if (gameOver)
            Time.timeScale = 0;
    }
}
