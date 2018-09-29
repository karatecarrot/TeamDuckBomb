using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicaniamtionchange : MonoBehaviour
{
    public Animator minibot;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Minibot());
	}
	
    IEnumerator Minibot()
    {
        yield return new WaitForSeconds(10);
        minibot.SetBool("Attacking", true);
    }

}
