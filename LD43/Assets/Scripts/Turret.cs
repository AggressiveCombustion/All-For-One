using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour {

    public Transform head;
    public Transform gun;
    public Image cooldownMeter;
    public GameObject explosionPrefab;

    public LineRenderer lr;

    public float duration = 3;
    public float elapsed = 0;
    public bool canFire = true;

    public Transform target;

    public float rateOfFire = 0.5f;
    public float fireTime = 0;

    public float ammo = 3;

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        cooldownMeter.fillAmount = (elapsed / duration);

        if(GameManager.instance.elapsed < 5)
        {
            lr.enabled = false;
            target = null;
            ammo = 3;
            fireTime = 0;
            elapsed = duration;
            head.transform.rotation = transform.rotation;
        }


        lr.enabled = false;
        elapsed += Time.deltaTime;
        if(elapsed > duration)
        {
            elapsed = duration;
            canFire = true;
        }

        if(canFire)
        {
            if(target != null)
            {
                lr.enabled = true;
                var dir = target.position - head.transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                lr.SetPosition(0, gun.position);
                lr.SetPosition(1, target.position);

                fireTime += Time.deltaTime;
                if(fireTime > rateOfFire)
                {
                    Fire(target.GetComponent<PlayerController>());

                    fireTime = 0;
                    

                    if (ammo == 0)
                    {
                        elapsed = 0;
                        canFire = false;
                        target = null;
                        head.transform.rotation = Quaternion.Slerp(head.transform.rotation, transform.rotation, Time.deltaTime);
                    }
                }
            }
        }
		
	}

    void Fire(PlayerController player)
    {
        GetComponent<AudioSource>().Play();
        ammo -= 1;
        Debug.Log("FIRE!");
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        player.Freeze();
        player.AddShake();
        if(!player.GetComponent<AudioSource>().isPlaying)
            player.GetComponent<AudioSource>().Play();

        Instantiate(explosionPrefab, gun.position, transform.rotation);
        GameObject e = Instantiate(explosionPrefab, 
            player.transform.position + new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f)),
            transform.rotation);
        e.GetComponent<SpriteRenderer>().color = player.color;

        if (ammo == 0)
        {
            // kill player
            player.Explode();
            player.Teleport();
            target = null;
        }
    }
}
