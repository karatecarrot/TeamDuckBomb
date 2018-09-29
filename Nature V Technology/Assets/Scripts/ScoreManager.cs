using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // The player's score.
    public int score;

    // Reference to the Text component.
    public Text text;

    void Awake()
    {
        // Set up the reference.
        //text = GetComponent<Text>();

        // Reset the score.
        score = 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Set the displayed text to be the word "Score" followed by the score value.
        text.text = "ROBOTS DESTROYED: " + score;
    }
}