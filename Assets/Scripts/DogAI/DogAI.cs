/*
 * Author(s): Timothy Ng, Isaiah Mann
 * Description: Brains behind the dogs actions on the world
 * Usage: [no notes]
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using k = PPGlobal;

public class DogAI : MonoBehaviourExtended 
{
    static DogState[] implementedStates = new DogState[]{DogState.Idle, DogState.Wandering};
        
    DogState currentState = DogState.Idle;
    Vector2 wanderCenter;
    float wanderRadius = 500;

    Vector2 target;

    int tapCount = 0;

    IEnumerator decisionRoutine;
    IEnumerator currentStateRoutine;

    bool isActive = true;

    float timePerState = 4f;

	// Use this for initialization
	protected override void setReferences()
    {
        base.setReferences();
        wanderCenter = GetComponent<RectTransform>().anchoredPosition;
        target = GetComponent<RectTransform>().anchoredPosition;
        setupDecisionRoutine();
    }
	
    void setupDecisionRoutine()
    {
        decisionRoutine = decideState();
        StartCoroutine(decisionRoutine);
    }

    DogState chooseRandomState()
    {
        return implementedStates[Random.Range(0, implementedStates.Length)];
    }

    IEnumerator decideState()
    {
        while(isActive)
        {
            switchToState(chooseRandomState());
            yield return new WaitForSeconds(timePerState);
        }
    }

    void switchToState(DogState state)
    {
        if(state != this.currentState)
        {
            if(currentStateRoutine != null)
            {
                StopCoroutine(currentStateRoutine);
            }
            currentStateRoutine = getCoroutineForState(state);
            if(currentStateRoutine != null)
            {
                StartCoroutine(currentStateRoutine);
            }
            this.currentState = state;
        }
    }

    IEnumerator getCoroutineForState(DogState state)
    {
        switch(state)
        {
            case DogState.Wandering:
                return wander();
            default:
                return null;
        }
    }

    IEnumerator wander()
    {
        while(isActive)
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
            yield return new WaitForEndOfFrame();
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
