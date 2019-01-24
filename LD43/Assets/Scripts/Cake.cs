using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cake : MonoBehaviour {

    bool done = false;
    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CAKE GET");
        if (other.transform.tag == "Player")
        {
            if (done)
                return;

            done = true;

            // do end of level stuff
            GameManager.instance.timers.Add(new Timer(2, ReturnToTitle));
            GameManager.instance.increaseTime = false;
            Debug.Log("WIN");
            if (gameObject.GetComponent<AudioSource>() != null)
                if (!gameObject.GetComponent<AudioSource>().isPlaying)
                    gameObject.GetComponent<AudioSource>().Play();

            GameObject.Find("Fade").AddComponent<FadeIn>();
            //GameObject.Find("EatSound").GetComponent<AudioSource>().Play();

            /*GameManager.instance.timers.Add(new Timer(0.8f, AddExplosion));
            GameManager.instance.timers.Add(new Timer(1.1f, AddExplosion));
            GameManager.instance.timers.Add(new Timer(1.4f, AddExplosion));*/

        }
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("MidTitle");
    }

    public void AddExplosion()
    {
        Instantiate(explosionPrefab, new Vector3(1000, 1000, 0), transform.rotation);
    }


}
