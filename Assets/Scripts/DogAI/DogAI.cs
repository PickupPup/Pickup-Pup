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

    DogState currentState = DogState.Wandering;
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

        currentState = decideState();

        switch (currentState)
        {
            case DogState.Idle:
                break;
            case DogState.Wandering:
                wander();
                break;
            case DogState.Eating:
                break;
            case DogState.Pooping:
                break;
            default:
                break;
        }
	}

    DogState decideState()
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
                    return DogState.Idle;
                case 1:
                    return DogState.Wandering;
            }
            return DogState.Idle;
        }

    }

    void wander()
    {
        if (target == GetComponent<RectTransform>().anchoredPosition)
        {
            float ctheta = Random.Range(0, 2 * Mathf.PI);
            float cradius = wanderRadius * Mathf.Sqrt(Random.Range(0f, 1f));
            float x = cradius * Mathf.Cos(ctheta);
            float y = cradius * Mathf.Sin(ctheta);
            target = new Vector2(x, y) + wanderCenter;
        }
        else
        {
            moveTo(target);
        }

    }

    void moveTo(Vector3 target)
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
