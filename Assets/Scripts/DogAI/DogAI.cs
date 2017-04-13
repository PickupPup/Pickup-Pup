/*
 * Author(s): Timothy Ng, Isaiah Mann
 * Description: Brains behind the dogs actions on the world
 * Usage: [no notes]
 */

using System.Collections;
using System;
using UnityEngine;
using k = PPGlobal;

public class DogAI : MonoBehaviourExtended 
{
    DogState currentState = DogState.Idle;
    Vector2 wanderCenter;

    Vector2 target;

    int tapCount = 0;

    IEnumerator currentStateRoutine;

    bool isActive = true;

    //TODO: Replace with q system
    float timePerState = 4f;

    int tapToHeart;
    float dogSpeed;

	// Use this for initialization
	protected override void setReferences()
    {
        base.setReferences();
        wanderCenter = GetComponent<RectTransform>().anchoredPosition;
        target = GetComponent<RectTransform>().anchoredPosition;
        setupDecisionRoutine();
        GetComponent<UIButton>().SubscribeToClick(Pet);
        tapToHeart = PPGameController.GetInstance.Tuning.TapToHeart;
        dogSpeed = PPGameController.GetInstance.Tuning.DogSpeed;
    }
	
    void setupDecisionRoutine()
    {
        StartCoroutine(decideState());
    }

    DogState chooseRandomState()
    {
        return (DogState)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(DogState)).Length));
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
            case DogState.Idle:
                return null;
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
                float ctheta = UnityEngine.Random.Range(0, 2 * Mathf.PI);
                float cradius = Screen.width * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
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

    void moveTo(Vector2 target)
    {
        Vector2 moveVec = Vector2.MoveTowards(GetComponent<RectTransform>().anchoredPosition, target, Time.deltaTime * dogSpeed);
        GetComponent<RectTransform>().anchoredPosition = moveVec;
    }

    public void Pet()
    {
        tapCount++;
        if(tapCount >= tapToHeart)
        {
            tapCount = 0;
            GetComponent<DogWorldSlot>().PeekDog.IncreaseAffection();
        }


        EventController.Event(k.GetPlayEvent(k.BARK));
    }
        
}
