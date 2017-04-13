/*
 * Author(s): Timothy Ng
 * Description: Brains behind the dogs actions on the world
 * Usage: [no notes]
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using k = PPGlobal;

public class DogAI : MonoBehaviour {

    enum State
    {
        Idle,
        Wandering,
        Eating,
        Pooping
    }

    State currentState = State.Wandering;
    Vector2 wanderCenter;
    float wanderRadius = 500;

    Vector2 target;
    int frameCounter = 0;

    int tapCount = 0;

	// Use this for initialization
	void Start () {
        wanderCenter = GetComponent<RectTransform>().anchoredPosition;
        target = GetComponent<RectTransform>().anchoredPosition;
    }
	
	void Update () {

        currentState = DecideState();

        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Wandering:
                Wander();
                break;
            case State.Eating:
                break;
            case State.Pooping:
                break;
            default:
                break;
        }
	}

    State DecideState()
    {
        if(frameCounter < 120)
        {
            frameCounter++;
            return currentState;
        }
        else
        {
            frameCounter = 0;
            switch(Random.Range(0, 2))
            {
                case 0:
                    return State.Idle;
                case 1:
                    return State.Wandering;
            }
            return State.Idle;
        }

    }

    void Wander()
    {
        if ((target - GetComponent<RectTransform>().anchoredPosition).sqrMagnitude < 0.5f)
        {
            float ct = Random.Range(0, 2 * Mathf.PI);
            float cr = wanderRadius * Mathf.Sqrt(Random.Range(0f, 1f));
            float x = cr * Mathf.Cos(ct);
            float y = cr * Mathf.Sin(ct);
            target = new Vector2(x, y) + wanderCenter;
        }
        else
        {
            MoveTo(target);
        }

    }

    void MoveTo(Vector3 target)
    {
        Vector2 moveVec = Vector3.MoveTowards(GetComponent<RectTransform>().anchoredPosition, target, Time.deltaTime * 40);
        GetComponent<RectTransform>().anchoredPosition = moveVec;
    }

    public void Pet()
    {
        tapCount++;
        if(tapCount >= 5)
        {
            tapCount = 0;
            GetComponent<DogWorldSlot>().PeekDog.IncreaseAffection();
        }


        EventController.Event(k.GetPlayEvent(k.BARK));
    }


}
