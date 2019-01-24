using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    public float alpha = 0f;
    public float fadeSpeed = 0.5f;

    public Transform colorObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        alpha += Time.deltaTime * fadeSpeed;

        //GetComponent<Image>().color = Camera.main.backgroundColor;

        //Color color = GetComponent<Image>().color;

        Color color = Camera.main.backgroundColor;
        if(colorObject != null)
        {
            if(colorObject.GetComponent<Text>() != null)
            {
                color = colorObject.GetComponent<Text>().color;
            }

            if (colorObject.GetComponent<Image>() != null)
            {
                color = colorObject.GetComponent<Image>().color;
            }
        }

        if (GetComponent<Image>() != null)
        {
            //Color color = Camera.main.backgroundColor;
            GetComponent<Image>().color = new Color(color.r,
                                                    color.g,
                                                    color.b,
                                                    alpha);
        }

        if (GetComponent<Text>() != null)
        {
            //Color color = GetComponent<Text>().color;
            GetComponent<Text>().color = new Color(color.r,
                                                    color.g,
                                                    color.b,
                                                    alpha);
        }

        if (alpha >= 1.0f)
            Destroy(this);

    }
}
