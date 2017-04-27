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
	Dog dog
	{
		get
		{
			return dogSlot.PeekDog;
		}
	}

    DogSlot dogSlot;
    DogState currentState = DogState.Idle;
    Vector2 wanderCenter;

    Vector2 target;

    int tapCount = 0;

    IEnumerator currentStateRoutine;

    bool isActive = true;

    //TODO: Replace with q system
    float minTimePerState;
    float maxTimePerState;

    int tapToHeart;
    float dogSpeed;

    #region MonoBehaviourExtended Overrides 

	// Use this for initialization
	protected override void setReferences()
    {
        base.setReferences();
        wanderCenter = GetComponent<RectTransform>().anchoredPosition;
        target = GetComponent<RectTransform>().anchoredPosition;
        setupDecisionRoutine();
        PPTuning tuning = PPGameController.GetInstance.Tuning;
        tapToHeart = tuning.TapToHeart;
        dogSpeed = tuning.DogSpeed;
        minTimePerState = tuning.MinDogStateTime;
        maxTimePerState = tuning.MaxDogStateTime;
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        GetComponent<UIButton>().SubscribeToClick(Pet);
		dogSlot = GetComponent<DogWorldSlot>();
    }

    #endregion
	
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
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimePerState, maxTimePerState));
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
                float cTheta = UnityEngine.Random.Range(0, 2 * Mathf.PI);
                float cRadius = Screen.width / 4 * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
                float x = cRadius * Mathf.Cos(cTheta);
                float y = cRadius * Mathf.Sin(cTheta);
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
		if(dog)
		{
	        tapCount++;
	        if(tapCount >= tapToHeart)
	        {
	            tapCount = 0;
	            dog.IncreaseAffection();
	        }
	        dog.Bark();
		}
    }
        
}
