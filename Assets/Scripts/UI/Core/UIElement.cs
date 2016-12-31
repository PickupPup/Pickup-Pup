using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElement : MonoBehaviourExtended {
	public bool hasImage {
		get {
			return image != null;
		}
	}
	public bool hasText {
		get {
			return text != null;
		}
	}
	public bool hasCanvasGroup {
		get {
			return canvas != null;
		}
	}
	public bool hasAlternateSprites {
		get {
			return alternateSprites.Length > 0;
		}
	}

	[SerializeField]
	protected Sprite[] alternateSprites;

    protected Image image;
    protected Text text;
    protected CanvasGroup canvas;
    protected IEnumerator opacityCoroutine;

	protected override void SetReferences () {
		base.SetReferences ();
		image = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
		canvas = GetComponentInChildren<CanvasGroup>();
	}

	public void Show () {
		gameObject.SetActive(true);
	}

	public void Hide () {
		gameObject.SetActive(false);
	}

	public void RandomSprite () {
		if (hasImage && hasAlternateSprites) {
			this.image.sprite = alternateSprites[Random.Range(0, alternateSprites.Length)];
		}
	}

	public void SetText (string text) {
		if (hasText) {
			this.text.text = text;
		}
	}

	public void StartOpacityLerp (float startOpacity, float endOpacity, float time, bool loop) {
		if (hasCanvasGroup) {
			startOpacityCoroutine(startOpacity, endOpacity, time, loop);
		}
	}

	public void StopOpacityLerp () {
		if (hasCanvasGroup) {
			stopOpacityCoroutine();
		}
	}

	void startOpacityCoroutine (float startOpacity, float endOpacity, float time, bool loop) {
		stopOpacityCoroutine();
		opacityCoroutine = lerpOpacity(startOpacity, endOpacity, time, loop);
		StartCoroutine(opacityCoroutine);
	}

	void stopOpacityCoroutine () {
		if (opacityCoroutine != null) {
			StopCoroutine(opacityCoroutine);
		}
	}

	IEnumerator lerpOpacity (float startOpacity, float endOpacity, float time, bool loop) {
		bool repeat = true;
		float start = startOpacity;
		float end = endOpacity;
		while (repeat) {
			float timer = 0;
			canvas.alpha = start;
			while (timer < time) {
				canvas.alpha = Mathf.Lerp(start, end, timer / time);
				yield return new WaitForEndOfFrame();
				timer += Time.deltaTime;
			}
			canvas.alpha = end;
			repeat = loop;
			if (loop) {
				// Used to reverse the lerp (creating an oscillating effect)
				float temp = start;
				start = end;
				end = temp;
			}
		}
	}
}
