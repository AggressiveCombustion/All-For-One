using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour {

    public LineRenderer line;
    public Vector2 end;
    public bool active = true;

    public PressureButton button;
    public Turret turret;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (button != null)
        {
            active = button.pressed;
        }
        if(active)
            end = transform.position + (Vector3.down * 100f);
        else
        {
            end = transform.position;
        }

        if (turret.target != null)
            end = transform.position;

        if (active && turret.target == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Ground")
                    {
                        //active = true;
                        end = hit.point;
                    }

                    if (hit.transform.tag == "Player" && turret.canFire)
                    {
                        GetComponent<AudioSource>().Play();
                        turret.target = hit.transform;
                        end = hit.point;
                        turret.ammo = 3;
                    }
                }
            }
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, end);

        line.enabled = turret.canFire;
    }

    public void Activate()
    {
        active = true;
    }
}
