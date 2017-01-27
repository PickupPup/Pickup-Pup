/*
 * Author(s): Isaiah Mann
 * Description: Displays info from a dog's scouting adventure
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

public class GiftReportUI : UIElement 
{	
	[SerializeField]
	Image dogPortrait;
	[SerializeField]
	Image rewardIcon;
	[SerializeField]
	Text reportText;
    [SerializeField]
    bool autoDestroyOnClick = true;

	UIButton dismissButton;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences()
	{
		base.setReferences();
        dismissButton = ensureReference<UIButton>(searchChildren:true);
	}

    protected override void fetchReferences()
    {
        base.fetchReferences();
        if(autoDestroyOnClick)
        {
            dismissButton.SubscribeToClick(Destroy);
        }
    }

	public override bool TryUnsubscribeAll()
	{
		base.TryUnsubscribeAll();
		dismissButton.TryUnsubscribeAll();
		return true;
	}

    public override void Destroy()
    {
        EventController.Event("PlayBack");
        base.Destroy();
    }

    #endregion

    public void Init(GiftReport report)
	{
		if(report.HasDog)
		{
			this.dogPortrait.sprite = report.Dog.Portrait;
		}
		this.rewardIcon.sprite = report.Currency.Icon;
		this.reportText.text = report.ToString();
		Show();
        EventController.Event("PlayGiftRedeem");
	}

	public void Init(CurrencyData gift)
	{
		Init(new GiftReport(gift));
	}

	public void SubscribeToDimiss(MonoAction action)
	{
		dismissButton.SubscribeToClick(action);
	}

	public void UnsubscribeFromDismiss(MonoAction action)
	{
		dismissButton.UnsubscribeFromClick(action);
	}

}

[System.Serializable]
public class GiftReport : PPData
{
	#region Instance Accessors

	public bool HasDog
	{
		get
		{
			return Dog != null;
		}
	}

	public DogDescriptor Dog
	{
		get; 
		private set;
	}

	public CurrencyData Currency
	{
		get;
		private set;
	}

	#endregion

	public GiftReport(DogDescriptor dog, CurrencyData currency)
	{
		this.Dog = dog;
		this.Currency = currency;
	}

	public GiftReport(CurrencyData currency)
	{
		this.Currency = currency;
	}

	public override string ToString()
	{
		if(HasDog)
		{
			return string.Format(DOG_GIFT_REPORT_FORMAT, Dog.Name, Currency.Amount, Currency.Type);
		}
		else
		{
			return string.Format(GENERIC_GIFT_REPORT_FORMAT, Currency.Amount, Currency.Type);
		}
	}

}
