using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

        transform.position = new Vector3(mousePos.x, mousePos.y, -5);

        if(GameObject.Find("GameManager") && Input.GetButtonDown("Fire1") && GameManager.instance.elapsed < 5)
        {
            Debug.Log("CLICK");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
            {
                if(hit.transform.tag == "Player")
                {
                    Debug.Log("HIT");
                    foreach(PlayerController p in GameObject.FindObjectsOfType<PlayerController>())
                    {
                        p.Deactivate();
                    }

                    hit.transform.GetComponent<PlayerController>().Activate();
                }
            }
        }
    }
}
