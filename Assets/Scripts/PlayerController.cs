using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameManager gameManager;
    public Coin coin;

    public float damper = 0.1f;
    public Vector3 onBeatScale = new Vector3(2f, 2f, 2f);
    public Vector3 onBeatRotate = new Vector3(0, 0, -45f);
    public int beatInterval = 1;    
    public int beatCounter;
    
    void Awake()
    {
        playerInput = new PlayerInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.Map.Enable();
        PlayerInput();
        GameManager.OnBeat += OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * damper);
    }

    private void OnBeat() 
    {
        beatCounter++;
        if (beatCounter >= beatInterval)
        {
            transform.localScale = onBeatScale;
            //transform.Rotate(onBeatRotate);
            beatCounter = 0;
        }
    }

    private void PlayerInput() 
    {
        playerInput.Map.Flip.started += OnPlayerFlip;
    }

    private void OnPlayerFlip(InputAction.CallbackContext context)
    {
        if(!coin.hasStarted)
        {
            coin.StartTestMove();
            gameManager.PlayMusic();
        } else 
        {
            if (coin.canBePressed) 
            {
                Debug.Log("Nice!");
                coin.transform.position = new Vector3(coin.transform.position.x, 0, coin.transform.position.z);
            } else 
            {
                Debug.Log("Miss!");
            }
            float yPos = coin.transform.position.y;
            coin.hasStarted = false;
            Debug.Log(yPos);
        }
    }
}
