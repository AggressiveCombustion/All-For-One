using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public class Input
{

}*/

public class PlayerController : MonoBehaviour {
    bool hasMoved = false;
    public float speed = 1.0f;
    
    Vector2 moveDirection = new Vector2(0, 0);
    public float groundCheckDistance = 1.0f;

    public Transform playerSprite;
    public Animator playerAnim;

    bool canMove = true;

    public int index = 0;
    public bool active = false;
    public Vector2 startingPoint;

    public Transform pointer;
    public Transform arrow;
    public Transform recMeter;
    public Transform shake;

    public PlayerState state = PlayerState.idle;

    public Dictionary<float, Vector2> timePos = new Dictionary<float, Vector2>();

    //public Dictionary<float, string> inputLog = new Dictionary<float, string>();
    public string[] inputLog = new string[1000];

    public Vector2[] posLog = new Vector2[1000];
    public float[] timeLog = new float[1000];

    public Color color = Color.white;

    public GameObject dotPrefab;
    public List<GameObject> dots = new List<GameObject>();

    int playbackElapsed = 0;



    // Use this for initialization
    void Start () {
        playerAnim = playerSprite.GetComponent<Animator>();
        //startingPoint = transform.position;
        
        // clear arrays
        for (int i = 0; i < 1000; i++)
        {
            posLog[i] = Vector2.zero;
            inputLog[i] = "";
            timeLog[i] = 0;
        }

        switch (index)
        {
            case 0:
                color = new Color(1.0f, 0.75f, 0.75f);
                break;
            case 1:
                color = new Color(0.75f, 0.75f, 1.0f);
                break;
            case 2:
                color = new Color(0.75f, 1.0f, 0.75f);
                break;
            case 3:
                color = new Color(1.0f, 1.0f, 0.75f);
                break;
        }

        playerSprite.GetComponent<SpriteRenderer>().color = color;
        recMeter.GetComponent<Image>().color = color;
        arrow.GetComponent<SpriteRenderer>().color = color;
        
    }
    
    void RegisterStartPoint()
    {
        transform.parent = null;
        //Debug.Log("STARTING POINT IS: " + transform.position.x + ", " + transform.position.y);
        startingPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (startingPoint.x == 0 && startingPoint.y == 0)
            GameManager.instance.timers.Add(new Timer(1, RegisterStartPoint));

        if (!CheckForParent())
            transform.parent = null;

        switch (state)
        {
            case PlayerState.idle:
                UpdateIdle();
                break;

            case PlayerState.recording:
                UpdateRecording();
                break;

            /*case PlayerState.playback:
                UpdatePlayback();
                break;*/
        }

        recMeter.GetComponent<Image>().fillAmount = (GameManager.instance.elapsed / (float)GameManager.instance.limit);
        
	}

    private void FixedUpdate()
    {
        switch (state)
        {
            /*case PlayerState.idle:
                UpdateIdle();
                break;

            case PlayerState.recording:
                UpdateRecording();
                break;*/

            case PlayerState.playback:
                UpdatePlayback();
                break;
        }
    }

    void UpdateIdle()
    {
        playerAnim.SetBool("running", false);
        float v = moveDirection.y;

        if (!Grounded())
        {
            v -= GameManager.instance.gravity * Time.deltaTime;
            if (v < -8)
                v = -8;
        }
        else
        {
            v = 0;
        }

        moveDirection.y = v;
        moveDirection.x = 0;

        transform.Translate(moveDirection * speed * Time.deltaTime * GameManager.instance.speed);

        if (active)
        {
            arrow.gameObject.SetActive(true);
            recMeter.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartRecording();
            }
        }

        else
        {
            arrow.gameObject.SetActive(false);
            recMeter.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.instance.elapsed = 0;
            StartPlayback();
        }
    }

    public void StartRecording()
    {
        arrow.gameObject.SetActive(true);
        recMeter.gameObject.SetActive(false);

        GameManager.instance.increaseTime = true;

        arrow.gameObject.SetActive(false);
        recMeter.gameObject.SetActive(true);

        playerSprite.gameObject.SetActive(true);
        canMove = true;

        GameManager.instance.elapsed = 0;
        state = PlayerState.recording;
        timePos.Clear();
        //inputLog.Clear();
        // clear arrays
        for(int i = 0; i < 1000; i++)
        {
            posLog[i] = Vector2.zero;
            inputLog[i] = "";
            timeLog[i] = 0;
        }

        foreach(GameObject g in dots)
        {
            Destroy(g);
        }
        dots.Clear();

        //GameManager.instance.timers.Add(new Timer(10, Deactivate));

        /* Do playback for others */
        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            if (p.transform.name != name)
            {
                p.StartPlayback();
            }
        }

        hasMoved = true;
    }

    public void StopRecording()
    {
        //GameManager.instance.elapsed = GameManager.instance.limit;
        moveDirection = Vector2.zero;
        Debug.Log("STOP RECORDING");
        // finish out log if the recording is ended
        for(int i = GameManager.instance.elapsed - 1; i< GameManager.instance.limit; i++)
        {
            posLog[i] = posLog[GameManager.instance.elapsed - 1];
        }

        state = PlayerState.idle;
        Deactivate();
        foreach(PlayerController p in FindObjectsOfType<PlayerController>())
        {
            if (p.state == PlayerState.playback)
                p.state = PlayerState.idle;
        }

        GameManager.instance.increaseTime = false;
        GameManager.instance.elapsed = 0;

        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.ReturnToStart();
        }
    }

    void UpdateRecording()
    {

        if(GameManager.instance.elapsed >= GameManager.instance.limit - 1)
        {
            StopRecording();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {

            StopRecording();
        }

        if(GameManager.instance.elapsed % (2 * LevelManager.instance.numPlayers) == 0)
        {
            GameObject d = Instantiate(dotPrefab, transform.position, transform.rotation);
            d.GetComponent<SpriteRenderer>().color = color;
            dots.Add(d);
        }

        /* Draw Path */
        /*int count = 0;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.startColor = new Color(color.r, color.g, color.b, 0.3f);
        lr.endColor = new Color(color.r, color.g, color.b, 0.3f);

        for (int i = 0; i < 10000; i++)
        {
            lr.positionCount = count + 1;
            lr.SetPosition(count, transform.position);
            if (i % 10 == 0)
            {
                if (timePos.ContainsKey(i))
                {
                    lr.SetPosition(count, timePos[i]);
                    count += 1;
                }
            }
        }*/
        /* End draw path */


        float h = Input.GetAxis("Horizontal");

        if (!canMove)
            h = 0;

        RaycastHit sideHit;
        if (Physics.Raycast(transform.position, transform.right * h, out sideHit, 0.1f))
        {
            if (sideHit.transform.tag == "Ground" || sideHit.transform.tag == "Player")
                h = 0;
        }

        float v = moveDirection.y;

        if (!Grounded())
        {
            v -= GameManager.instance.gravity * Time.deltaTime;
            if (v < -4)
                v = -4;
        }
        else
        {
            v = 0;
        }

        playerAnim.SetBool("grounded", Grounded());

        bool didJump = false;

        moveDirection = new Vector2(h, v);

        if (Input.GetButtonDown("Jump") && Grounded())
        {
            Jump();
            didJump = true;
        }

        if (h != 0)
        {
            playerSprite.GetComponent<SpriteRenderer>().flipX = (h > 0 ? false : true);
        }

        playerAnim.SetBool("running", h != 0);

        //transform.Translate(moveDirection * speed * Time.deltaTime * GameManager.instance.speed);
        GetComponent<CharacterController>().Move(moveDirection * speed * Time.deltaTime * GameManager.instance.speed);

        if(!timePos.ContainsKey(GameManager.instance.elapsed))
            timePos.Add(GameManager.instance.elapsed, transform.position);

        posLog[GameManager.instance.elapsed] = transform.position;
        timeLog[GameManager.instance.elapsed] = Time.deltaTime;

        string log = "";
        if (h > 0)
            log = "right";
        else if (h < 0)
            log = "left";
        /*if(!inputLog.ContainsKey(GameManager.instance.elapsed))
            inputLog.Add(GameManager.instance.elapsed, log);*/
        //else
        {
            inputLog[GameManager.instance.elapsed] += log;
            //Debug.Log("LOGGED <" + inputLog[GameManager.instance.elapsed] + ">");
        }


        if (!didJump)
        {
            /*System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Resources/test.txt", true);
            sw.WriteLine(GameManager.instance.elapsed + " - " + "<" + log + ">");
            sw.Close();*/
        }
    }

    void Jump()
    {
        transform.parent = null;
        Debug.Log("Jump");
        //v = 3;
        moveDirection.y = 3;
        playerAnim.SetTrigger("jump");

        string log = "jump";
        /*if (inputLog.ContainsKey(GameManager.instance.elapsed))
            inputLog[GameManager.instance.elapsed] = log;
        else
            inputLog.Add(GameManager.instance.elapsed, log);*/
        inputLog[GameManager.instance.elapsed] += log;
        if(GameManager.instance.elapsed > 0)
            inputLog[GameManager.instance.elapsed-1] += log;
        if (GameManager.instance.elapsed > 1)
            inputLog[GameManager.instance.elapsed - 2] += log;

        /*"System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Resources/test.txt", true);
        sw.WriteLine(GameManager.instance.elapsed + " - " + "<" + log + ">");
        sw.Close();*/
    }

    void UpdatePlayback()
    {
        if (!hasMoved)
            return;

        playbackElapsed += 1;

        // has issues, but works
        /*if(timePos.ContainsKey(GameManager.instance.elapsed))
        {
            transform.position = timePos[GameManager.instance.elapsed];
        }*/
        Vector2 newPos = posLog[GameManager.instance.elapsed];
        newPos.y = transform.position.y;

        RaycastHit sideHit;
        Vector3 dir;
        // get direction we're trying to go in
        if (newPos.x > transform.position.x) //moving right
            dir = transform.right;
        else
            dir = transform.right * -1;

        if (Physics.Raycast(transform.position, dir, out sideHit, 0.2f))
        {
            if(sideHit.transform.tag == "Ground" || sideHit.transform.tag == "Player")
            {
                newPos.x = transform.position.x;
                canMove = false;
            }
        }

        if (!canMove)
            newPos.x = transform.position.x;

        if (GameManager.instance.elapsed < GameManager.instance.limit)
            transform.position = newPos;

        int elapsed = GameManager.instance.elapsed;

        bool doJump = false;

        if(inputLog[elapsed].Contains("jump"))
        {
            doJump = true;
        }

        float v = moveDirection.y;
        float recTime = timeLog[GameManager.instance.elapsed];

        if (!Grounded())
        {
            v -= GameManager.instance.gravity * recTime;
            if (v < -8)
                v = -8;
        }
        else
        {
            v = 0;
        }

        moveDirection = new Vector2(0, v);

        if (doJump)
            Jump();

        GetComponent<CharacterController>().Move(moveDirection * 
                                                (speed) * 
                                                recTime * 
                                                GameManager.instance.speed);


        // aargh

        /*int elapsed = GameManager.instance.elapsed;
        bool doJump = false;
        float recTime = timeLog[GameManager.instance.elapsed];
        float v = moveDirection.y;
        float h = 0;

        if (!Grounded())
        {
            v -= GameManager.instance.gravity * recTime;
            if (v < -8)
                v = -8;
        }
        else
        {
            v = 0;
        }
        

        //if (inputLog.ContainsKey(elapsed))
        {
            string log = inputLog[elapsed];

            //System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Resources/test2.txt", true);
            //sw.WriteLine(GameManager.instance.elapsed + " - " + "<" + log + ">");
            //sw.Close();

            //Debug.Log("READ <" + log + ">");

            if (log == "")
            {
                h = 0;
            }
            if (log.Contains("left"))
            {
                h = -1;
            }
            if (log.Contains("right"))
            {
                h = 1;
            }
            if (log.Contains("jump"))
            {
                doJump = true;
            }
            if (log == "jump")
            {
                h = 0;
                doJump = true;
            }
        }

        RaycastHit sideHit;
        if (Physics.Raycast(transform.position, transform.right * h, out sideHit, speed * Time.deltaTime))
        {
            if (sideHit.transform.tag == "Ground")
                h = 0;
        }

        if (!canMove)
            h = 0;

        moveDirection = new Vector2(h, v);

        if (doJump && Grounded())
            Jump();

        
        //transform.Translate(moveDirection * speed * Time.deltaTime * GameManager.instance.speed);
        GetComponent<CharacterController>().Move(moveDirection * (speed) * recTime * GameManager.instance.speed);
        */
        if (elapsed >= GameManager.instance.limit - 1)
        {
            ReturnToStart();
            Deactivate();
        }
    }

    public void Activate()
    {
        //startingPoint = transform.position;
        active = true;

        pointer.gameObject.SetActive(true);
        arrow.gameObject.SetActive(true);
        recMeter.gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        playerSprite.gameObject.SetActive(true);
        canMove = true;

        active = false;
        pointer.gameObject.SetActive(false);
        state = PlayerState.idle;

        moveDirection = Vector2.zero;
        //transform.position = startingPoint;
        //GameManager.instance.elapsed = 0;
    }

    public void ReturnToStart()
    {
        transform.parent = null;
        transform.position = startingPoint;
    }

    public void StartPlayback()
    {
        Debug.Log("START PLAYBACK");
        GameManager.instance.increaseTime = true;
        playbackElapsed = 0;
        playerSprite.gameObject.SetActive(true);
        canMove = true;

        active = false;
        pointer.gameObject.SetActive(true);
        arrow.gameObject.SetActive(false);
        recMeter.gameObject.SetActive(true);

        state = PlayerState.playback;
    }
    

    bool Grounded()
    {
        //Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector2.down, out hit, groundCheckDistance))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Player" /*|| hit.transform.tag == "Button"*/)
            {
                
                //
                
                /*if (hit.transform.tag == "Button")
                    transform.parent = hit.transform.parent;

                else*/
                    transform.parent = hit.transform;
                return true;
            }

            else
            {
                transform.parent = null;
                return false;
            }
        }
        

        return false;
    }

    bool CheckForParent()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector2.down, out hit, groundCheckDistance))
        {
            if (hit.transform == transform.parent)
                return true;
        }

        return false;
    }

    public void LaserDeath()
    {
        Debug.Log("LASERDEATH");
        GameManager.instance.timers.Add(new Timer(2, Explode));
        playerAnim.SetBool("scream", true);
        canMove = false;
    }

    public void Explode()
    {
        Debug.Log("EXPLODE");
        GameObject ex = Instantiate(GameManager.instance.explosionPrefab, transform.position, transform.rotation);
        ex.GetComponent<SpriteRenderer>().color = color;

        /*if (state == PlayerState.recording)
            StopRecording();*/
    }

    public void DestroySelf()
    {
        //Destroy(gameObject);
        playerSprite.gameObject.SetActive(false);
    }

    public void ReceiveMessage(string message)
    {
        switch(message)
        {
            case "jump":
                Jump();
                break;
        }
    }

    public void Freeze()
    {
        canMove = false;
        playerAnim.SetBool("scream", true);
    }

    public void Teleport()
    {
        transform.position = new Vector2(1000, 1000);
        playerAnim.SetBool("scream", false);
        /*if (state == PlayerState.recording)
            StopRecording();*/
        GameManager.instance.CameraShake();
    }

    public void AddShake()
    {
        shake.gameObject.AddComponent<ShakeHorizontal>();
    }
    
}

public enum PlayerState
{
    idle,
    recording,
    playback,
}