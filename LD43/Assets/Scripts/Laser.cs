using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public LineRenderer line;
    public Vector2 end;
    public bool active = true;

    public PressureButton button;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if(button != null)
        {
            active = button.pressed;
        }

        end = transform.position + (Vector3.down * 100f);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector2.down, out hit, 100.0f))
        {
            if(hit.transform != null)
            {
                if(hit.transform.tag == "Ground")
                {
                    //active = true;
                    end = hit.point;
                }

                if (hit.transform.tag == "Player")
                {
                    if (active)
                    {
                        hit.transform.GetComponent<PlayerController>().LaserDeath();
                        GameManager.instance.timers.Add(new Timer(4, Activate));
                    }
                    end = hit.point;

                    hit.transform.position = new Vector2(transform.position.x, hit.transform.position.y);

                    active = false;

                }
            }
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, end);
	}

    public void Activate()
    {
        active = true;
    }
}
