using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    public GameManager gameManager;
    private float beatTempo;
    public bool hasStarted;
    public bool canBePressed;

    public int targetBeat;  // The beat to hit on
    public int speedMultiplier; 

    [SerializeField] float spinSpeed;
    private int _beatCounter = 0;


    void Start()
    {
        beatTempo = gameManager.bpm / 60f;
        GameManager.OnBeat += OnBeat;
        GameManager.OnStart += OnStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted) 
        {
            Vector3 tempoMovement = new Vector3(0f, (beatTempo * speedMultiplier) * Time.deltaTime, 0f);
            // If we reached half the target beat, start falling downwards
            if(_beatCounter < (targetBeat / 2) + 1) 
            {
                transform.position += tempoMovement;
            } else 
            {   
                transform.position -= tempoMovement;
            }
            Spin();
        }
    }

    private void OnStart()
    {
        hasStarted = true;
        _beatCounter = 0;
    }

    private void OnBeat()
    {
        _beatCounter++;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canBePressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canBePressed = false;
        }
    }

    private void Spin()
    {
        transform.Rotate(spinSpeed * Time.deltaTime, 0, 0);
    }
}
