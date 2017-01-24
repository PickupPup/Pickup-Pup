/*
 * Author(s): Isaiah Mann
 * Description: Displays info from a dog's scouting adventure
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

public class ScoutingReportUI : UIElement 
{	
	[SerializeField]
	Image dogPortrait;
	[SerializeField]
	Image rewardIcon;
	[SerializeField]
	Text reportText;

	UIButton dismissButton;

	#region MonoBehaviourExtended 

	protected override void setReferences ()
	{
		base.setReferences ();
		dismissButton = ensureReference<UIButton>();
	}

	#endregion

	public void Init(ScoutingReport report)
	{
		this.dogPortrait.sprite = report.Dog.Portrait;
		this.rewardIcon.sprite = report.Currency.Icon;
		this.reportText.text = report.ToString();
		Show();
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
public class ScoutingReport : PPData
{
	public DogDescriptor Dog{get; private set;}
	public CurrencyData Currency{get; private set;}

	public ScoutingReport(DogDescriptor dog, CurrencyData currency)
	{
		this.Dog = dog;
		this.Currency = currency;
	}

	public override string ToString()
	{
		return string.Format(REPORT_FORMAT, Dog.Name, Currency.Amount, Currency.Type);
	}

}
