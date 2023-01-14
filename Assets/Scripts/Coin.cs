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

    [SerializeField] float spinSpeed;
    public Timer timer;
    [SerializeField] private bool isSpinning = false;
    private bool shouldInterruptTween = false;
    private int _beatCounter = 0;

    void Start()
    {
        beatTempo = gameManager.bpm / 60f;
        GameManager.OnBeat += OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted) 
        {
            Vector3 tempoMovement = new Vector3(0f, beatTempo * Time.deltaTime, 0f);
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

        // if(IsSpinning()) {
        //     Spin();
        // }
    }

    private void OnBeat()
    {
        _beatCounter++;
    }

    public void StartTestMove()
    {
        hasStarted = true;
        _beatCounter = 0;
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

    public bool IsSpinning() {
        return isSpinning;
    }

    public void OnFlipStart()
    {
        return;
        if(isSpinning) { return; }
        isSpinning = true;
        shouldInterruptTween = false;

        // Start position of flip
        Vector3 startPosition = transform.position;
        // Vector3 at apex of coin travel
        Vector3 peakPosition = new Vector3(transform.position.x, transform.position.y + 40, transform.position.z);
        // Vector3 for when user misses coin
        Vector3 missPosition = new Vector3(transform.position.x, startPosition.y - 40, transform.position.z);

        timer.StartTimer(until: 5);
        StartCoroutine(InterruptableTweeng(1.5f,(p) => transform.position=p, startPosition, peakPosition, () => {
            Debug.Log("Coin is at the top, coming back down!");
            if(!isSpinning) { return; }

            // Coin reached peak, fall back down
            StartCoroutine(InterruptableTweeng(1.5f,(p) => transform.position=p,
                transform.position, startPosition, () => {
                    Debug.Log("Coin is at the start position");
                    
                    // Continue falling if player hasn't attempted a catch
                    if(!isSpinning) { return; }
                    StartCoroutine(InterruptableTweeng(1.5f, (p) => transform.position=p, transform.position, missPosition, () => {
                        Debug.Log("Coin was completely missed!");
                    }));
                }));
        }));
    }

    public void OnFlipEnd() 
    {
        return;
        if(!isSpinning) { return; }
        Debug.Log("OnFlipEnd");
        isSpinning = false;
        shouldInterruptTween = true;
        timer.StopTimer();
        decimal currentTime = Decimal.Round(((decimal)timer.GetCurrentTime()), 2);
        decimal correctTime = 3.00m;

        if(currentTime < correctTime) {
            Debug.Log("EARLY");
        }
        if(currentTime > correctTime) {
            Debug.Log("LATE");
        }
        if(currentTime == correctTime) {
            Debug.Log("PERFECT");
        }
    }

    private void Spin()
    {
        transform.Rotate(spinSpeed * Time.deltaTime, 0, 0);
    }

    private IEnumerator InterruptableTweeng(
        float duration,
        System.Action<Vector3> var,
        Vector3 startPos,
        Vector3 endPos,
        System.Action onCompletion) 
        {
        float sT = Time.time;
        float eT = sT + duration;
        
        while (Time.time < eT && !shouldInterruptTween)
        {
            float t = (Time.time-sT)/duration;
            var( Vector3.Lerp(startPos, endPos, t ) );
            yield return null;
        }
        
        // If not force interrupting, move Vector3 to end pos
        if(!shouldInterruptTween) 
        {
            var(endPos);
        }

        // Invoke completion callback
        if(onCompletion != null) { onCompletion(); }
        }
}
