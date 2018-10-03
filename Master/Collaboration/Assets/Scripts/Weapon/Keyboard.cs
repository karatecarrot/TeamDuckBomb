using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {

    public Material codeMaterial;
    public float speed;

    private float curValue = 1;

	void Update ()
    {
        float value = (curValue += Time.deltaTime) * speed;
        codeMaterial.SetTextureOffset("_MainTex", new Vector2(value, 0));
	}
}
