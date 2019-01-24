using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeVertical : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        GetComponent<Animator>().SetTrigger("v");
        Destroy(this, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
