/*
 * Author: Isaiah Mann 
 * Desc: World object controller
 */

using UnityEngine;
using System.Collections;

public class WorldObjectBehaviour : MonoBehaviourExtended 
{
	#region Instance Accessors

	public Color Colour 
	{
		get;
		private set;
	}

	#endregion

	protected Renderer[] renderers;
	protected bool [] ignoreColorChanges; // Parallel array to renderers

	IEnumerator colorCoroutine;


	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
		setRenderers();
		Colour = sampleColour();
	}

	#endregion

	void setRenderers() 
	{
		renderers = GetComponentsInChildren<Renderer>();
		refreshColourIgnore();
	}

	protected void setColour(Color colour, bool updateStoredColour = true) 
	{
		if(updateStoredColour) 
		{
			this.Colour = colour;
		}
		refreshColour(colour);
	}

	protected void refreshColour(Color colour) 
	{
		if(renderers != null) 
		{
			for(int i = 0; i < renderers.Length; i++) 
			{
				if(!ignoreColorChanges[i]) 
				{
					refreshRenderer(renderers[i], colour);
				}
			}
		}
	}

	Color sampleColour() 
	{
		if(renderers.Length >= 1) 
		{
			return renderers[0].material.color;
		}
		else
		{
			return default(Color);
		}
	}

	void refreshRenderer(Renderer renderer, Color color) 
	{
		renderer.material.color = color;
	}

	IEnumerator lerpToColour(Color targetColour, float totalTime, bool updateStoredColour = false) 
	{
		float timer = 0;
		Color startColour = sampleColour();
		while(timer <= totalTime) 
		{
			setColour(Color.Lerp(startColour, targetColour, timer), updateStoredColour);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		setColour(targetColour, updateStoredColour);
	}

	protected void startLerpColor(Color targetColour, float totalTime, bool updateStoredColour = false) 
	{
		haltLerpColor();
		colorCoroutine = lerpToColour(targetColour, totalTime, updateStoredColour);
		StartCoroutine(colorCoroutine);
	}

	protected void haltLerpColor()
	{
		if(colorCoroutine != null) 
		{
			StopCoroutine(colorCoroutine);
		}
	}

	protected void refreshColourIgnore() 
	{
		ignoreColorChanges = new bool[renderers.Length];
		for(int i = 0; i < renderers.Length; i++) 
		{
			Ignore ignore;
			if((ignore = renderers[i].GetComponent<Ignore>()) && ignore.ColorChange) 
			{
				ignoreColorChanges[i] = true;
			}
			else
			{
				ignoreColorChanges[i] = false;
			}
		}
	}

}
