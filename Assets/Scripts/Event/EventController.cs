/*
 * Author(s): Isaiah Mann 
 * Description: A single event class that others can subscribe to and call events from
 * Currently relies on event names as strings
 * Event method can be overloaded to implement different event types
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EventController : SingletonController<EventController> 
{
	#region Event Types

	public delegate void NamedEventAction(string nameOfEvent);
	public event NamedEventAction OnNamedEvent;

	public delegate void NamedFloatAction(string valueKey, float key);
	public event NamedFloatAction OnNamedFloatEvent;

	public delegate void AudioEventAction(AudioActionType actionType, AudioType audioType);
	public event AudioEventAction OnAudioEvent;

	public delegate void PPEventAction(PPEvent gameEvent);
	public event PPEventAction OnPPEvent;

	public event PPData.DogActionStr OnNamedDogEvent;
	public event PPData.PPDogAction OnPPDogEvent;

	#endregion

	[SerializeField]
	bool alsoCallPPEventsAsStrings;
	[SerializeField]
	bool tryAlsoCallStringsAsPPEvents;

	#region Event Subscription

	public static void Subscribe(NamedEventAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnNamedEvent += eventAction;
		}
	}

	public static void Subscribe(NamedFloatAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnNamedFloatEvent += eventAction;
		}
	}

	public static void Subscribe(AudioEventAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnAudioEvent += eventAction;
		}
	}

	public static void Subscribe(PPEventAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnPPEvent += eventAction;
		}
	}

	public static void Subscribe(PPData.DogActionStr eventAction)
	{
		if(hasInstance)
		{
			Instance.OnNamedDogEvent += eventAction;
		}
	}

	public static void Subscribe(PPData.PPDogAction eventAction)
	{
		if(hasInstance)
		{
			Instance.OnPPDogEvent += eventAction;
		}
	}

	public static void Unsubscribe(NamedEventAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnNamedEvent -= eventAction;
		}
	}

	public static void Unsubscribe(NamedFloatAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnNamedFloatEvent -= eventAction;
		}
	}

	public static void Unsubscribe(AudioEventAction eventAction) 
	{
		if(hasInstance) 
		{
			Instance.OnAudioEvent -= eventAction;
		}
	}

	public static void Unsubscribe(PPEventAction eventAction) 
	{
		if(hasInstance)
		{
			Instance.OnPPEvent -= eventAction;
		}
	}

	public static void Unsubscribe(PPData.DogActionStr eventAction)
	{
		if(hasInstance)
		{
			Instance.OnNamedDogEvent -= eventAction;
		}
	}

	public static void Unsubscribe(PPData.PPDogAction eventAction)
	{
		if(hasInstance)
		{
			Instance.OnPPDogEvent -= eventAction;
		}
	}

	#endregion

	#region Event Calls

	public static void Event(string eventName, bool isCallback = false) 
	{
		if(hasInstance) 
		{
			if(Instance.OnNamedEvent != null) 
			{
				Instance.OnNamedEvent(eventName);
			}
			if(Instance.tryAlsoCallStringsAsPPEvents && !isCallback) 
			{
				tryCallStringAsPPEvent(eventName);
			}
		}
	}
		

	public static void Event(string valueKey, float value) 
	{
		if(hasInstance && Instance.OnNamedFloatEvent != null) 
		{
			Instance.OnNamedFloatEvent(valueKey, value);
		}
	}

    public static void Event(AudioActionType actionType, AudioType audioType) 
	{
		if(hasInstance && Instance.OnAudioEvent != null) 
		{
			Instance.OnAudioEvent(actionType, audioType);
        }
    }

	public static void Event(PPEvent gameEvent, bool isCallback = false) 
	{
		if(hasInstance) 
		{
			if(Instance.OnPPEvent != null) 
			{	
				Instance.OnPPEvent(gameEvent);
			}
			if(Instance.alsoCallPPEventsAsStrings && !isCallback) 
			{
				callPPEventAsString(gameEvent);
			}
		}
	}

	public static void Event(string eventName, Dog dog, bool isCallback = false)
	{
		if(hasInstance)
		{
			if(Instance.OnNamedDogEvent != null)
			{
				Instance.OnNamedDogEvent(eventName, dog);
			}
			if(Instance.tryAlsoCallStringsAsPPEvents && !isCallback)
			{
				tryCallNamedDogEventAsPPDogEvent(eventName, dog);
			}
		}
	}

	public static void Event(PPEvent gameEvent, Dog dog, bool isCallback = false)
	{
		if(hasInstance)
		{
			if(Instance.OnPPDogEvent != null)
			{
				Instance.OnPPDogEvent(gameEvent, dog);
			}
			if(Instance.alsoCallPPEventsAsStrings && !isCallback)
			{
				callPPDogEventAsString(gameEvent, dog);
			}
		}
	}

	static void callPPEventAsString(PPEvent gameEvent) 
	{
		Event(gameEvent.ToString(), isCallback:true);
	}

	static void callPPDogEventAsString(PPEvent gameEvent, Dog dog)
	{
		Event(gameEvent.ToString(), dog, isCallback:true);
	}

	static bool tryCallStringAsPPEvent(string eventName) 
	{
		try
		{
			Event((PPEvent) Enum.Parse(typeof(PPEvent), eventName), isCallback:true);
			return true;
		}
		catch 
		{
			// Cannot parse enum:
			return false;
		}
	}

	static bool tryCallNamedDogEventAsPPDogEvent(string eventName, Dog dog) 
	{
		try
		{
			Event((PPEvent) Enum.Parse(typeof(PPEvent), eventName), dog, isCallback:true);
			return true;
		}
		catch 
		{
			// Cannot parse enum:
			return false;
		}
	}

	#endregion

}

// Unique Pickup Pup Event Enum
public enum PPEvent 
{
	Play,
	Pause,
	Quit,
	LoadStart,
	LoadHome,
    LoadShop,

}
