﻿/*
 * Author: Isiaah Mann
 * Desc: A simple structure for interacting with Unity's UI system
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using k = PPGlobal;
using u = UnityEngine;

public class UIElement : MonoBehaviourExtended 
{
	protected const string OFF = k.OFF;
	protected const string ON = k.ON;

    static Dictionary<Type, Stack<UIElement>> uiElementSpawnPools = new Dictionary<Type, Stack<UIElement>>();

	#region Instance Accessors

	public bool hasImage 
	{
		get 
		{
			return image != null;
		}
	}

	public bool hasText 
	{
		get 
		{
			return text != null;
		}
	}

	public bool hasCanvasGroup 
	{
		get 
		{
			return canvas != null;
		}
	}

	public bool hasAlternateSprites 
	{
		get 
		{
			return alternateSprites.Length > 0;
		}
	}

	#endregion

	[SerializeField]
	protected Sprite[] alternateSprites;
    [SerializeField]
    protected bool shouldCollectInSpawnPool = true;
    protected Image image;
    protected Text text;
    protected CanvasGroup canvas;
    protected IEnumerator opacityCoroutine;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		image = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
		canvas = GetComponentInChildren<CanvasGroup>();
	}

	#endregion

	public void Show() 
	{
		gameObject.SetActive(true);
	}

	public void Hide() 
	{
		gameObject.SetActive(false);
	}

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

	public void RandomSprite() 
	{
		if(hasImage && hasAlternateSprites) 
		{
			this.image.sprite = alternateSprites[u.Random.Range(0, alternateSprites.Length)];
		}
	}

	public void SetText(string text) 
	{
		if(hasText) 
		{
			this.text.text = text;
		}
	}

	public void StartOpacityLerp(float startOpacity, float endOpacity, float time, bool loop) 
	{
		if(hasCanvasGroup) 
		{
			startOpacityCoroutine(startOpacity, endOpacity, time, loop);
		}
	}

	public void StopOpacityLerp() 
	{
		if(hasCanvasGroup) 
		{
			stopOpacityCoroutine();
		}
	}

    public override void Destroy()
    {
        collectInSpawnPool();
    }

	protected Image getTopImage()
	{
		// Assumes sort orders are standard:
		Image[] images = GetComponentsInChildren<Image>();
		return images[images.Length-1];
	}

	void startOpacityCoroutine(float startOpacity, float endOpacity, float time, bool loop) 
	{
		stopOpacityCoroutine();
		opacityCoroutine = lerpOpacity(startOpacity, endOpacity, time, loop);
		StartCoroutine(opacityCoroutine);
	}

	void stopOpacityCoroutine() 
	{
		if(opacityCoroutine != null) 
		{
			StopCoroutine(opacityCoroutine);
		}
	}

	IEnumerator lerpOpacity(float startOpacity, float endOpacity, float time, bool loop) 
	{
		bool repeat = true;
		float start = startOpacity;
		float end = endOpacity;
		while(repeat) 
		{
			float timer = 0;
			canvas.alpha = start;
			while(timer < time) 
			{
				canvas.alpha = Mathf.Lerp(start, end, timer / time);
				yield return new WaitForEndOfFrame();
				timer += Time.deltaTime;
			}
			canvas.alpha = end;
			repeat = loop;
			if(loop)
			{
				// Used to reverse the lerp (creating an oscillating effect)
				float temp = start;
				start = end;
				end = temp;
			}
		}
	}

    public static bool TryPullFromSpawnPool(Type type, out UIElement elem)
    {
        Stack<UIElement> pool;
        if(uiElementSpawnPools.TryGetValue(type, out pool))
        {
            elem = pool.Pop();
            elem.gameObject.SetActive(true);
            return true;
        }
        else
        {
            elem = null;
            return false;
        }
    }

    void collectInSpawnPool()
    {
        Stack<UIElement> pool;
        if(!uiElementSpawnPools.TryGetValue(this.GetType(), out pool))
        {
            pool = new Stack<UIElement>();   
        }
        gameObject.SetActive(false);
        pool.Push(this);
    }

}
