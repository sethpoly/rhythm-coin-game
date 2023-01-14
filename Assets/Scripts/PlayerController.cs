using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public Coin coin;
    
    void Awake()
    {
        playerInput = new PlayerInput();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerInput.Map.Enable();
        PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayerInput() 
    {
        playerInput.Map.Flip.started += OnPlayerFlip;
    }

    private void OnPlayerFlip(InputAction.CallbackContext context)
    {
        if(coin.IsSpinning()) 
        {
            coin.OnFlipEnd();
        } else 
        {
            coin.OnFlipStart();
        }
    }
}
