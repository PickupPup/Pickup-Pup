/*
 * Author: Isaiah Mann
 * Description: Used to receive events
 */

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using k = Global;
using dir = DirectionUtil;

public class UIEventListener : MonoBehaviourExtended, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Instance Accessors 

    public bool SwipeIsOccuring
    {
        get;
        private set;
    }

    #endregion

	[SerializeField]
	UIEventHandler[] handlers;

	UIElement element;

    #region Swipe Related Vars

    [SerializeField]
    float minValidSwipeDistance = k.MIN_SWIPE_THRESHOLD;
    float swipeTravelMagninute;

    Vector2 swipeTravel;

    #endregion

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		element = GetComponent<UIElement>();
		if(!element) 
		{
			element = gameObject.AddComponent<UIElement>();
		}
	}

	protected override void fetchReferences()
	{
		base.fetchReferences();
		triggerHandlersOnStart();
	}

	protected override void handleNamedEvent(string eventName) 
	{
		base.handleNamedEvent(eventName);
		foreach(UIEventHandler handler in handlers) 
		{
			if(handler.RespondsToTrigger(eventName)) 
			{
				handler.Execute(element);
			}
		}
	}

	void triggerHandlersOnStart()
	{
		foreach(UIEventHandler handler in handlers)
		{
			if(handler.RunsOnStart())
			{
				handler.Execute(element);
			}
		}
	}
        
	#endregion

    #region IPointerClickHandler Interface

    void IPointerClickHandler.OnPointerClick(PointerEventData ptrEvent)
    {
        foreach(UIEventHandler handler in handlers)
        {
            if(handler.RunsOnClick())
            {
                handler.Execute(element);
            }
        }
    }

    #endregion

    #region IBeginDragHandler Interface

    void IBeginDragHandler.OnBeginDrag(PointerEventData ptrEvent)
    {
        SwipeIsOccuring = true;
        resetSwipeTracking();
    }

    #endregion

    #region IBeginDragHandler Interface

    void IDragHandler.OnDrag(PointerEventData ptrEvent)
    {
        swipeTravelMagninute += ptrEvent.delta.magnitude;
        swipeTravel += ptrEvent.delta;
    }

    #endregion

    #region IEndDragHandler Interface

    void IEndDragHandler.OnEndDrag(PointerEventData ptrEvent)
    {
        if(wasValidSwipe(swipeTravelMagninute))
        {
            Direction swipeDirection = determineSwipeDirection(swipeTravel);
            executeSwipeAction(swipeDirection);
        }
        SwipeIsOccuring = false;
        resetSwipeTracking();
    }

    #endregion

    void executeSwipeAction(Direction swipeDirection)
    {
        element.InitializeSwipe(swipeDirection);
        foreach(UIEventHandler handler in handlers)
        {
            if(handler.RunsOnSwipe(swipeDirection))
            {
                handler.Execute(element);
            }
        }
    }
        
    void resetSwipeTracking()
    {
        swipeTravel = Vector2.zero;
        swipeTravelMagninute = NONE_VALUE;
    }

    Direction determineSwipeDirection(Vector2 swipeTravel)
    {
        return dir.DirectionFromVector(swipeTravel);
    }

    bool wasValidSwipe(float swipeTravelMagnitude)
    {
        return swipeTravelMagnitude >= minValidSwipeDistance;
    }

}
