using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicaniamtionchange : MonoBehaviour
{
    public Animator minibot;
    public Animator MedBot;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Minibot());
	}
	
    IEnumerator Minibot()
    {
        yield return new WaitForSeconds(5);
        minibot.SetBool("Attacking", true);
        MedBot.SetBool("MediumAttacking", true);
    }

}
