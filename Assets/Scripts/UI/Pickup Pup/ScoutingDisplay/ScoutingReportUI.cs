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

	public void Init(ScoutingReport report)
	{
		this.dogPortrait.sprite = report.Dog.Portrait;
		this.rewardIcon.sprite = report.Currency.Icon;
		this.reportText.text = report.ToString();
		Show();
	}

}

[System.Serializable]
public class ScoutingReport : PPData
{
	const string REPORT_FORMAT = "{0} brought you {1} {2}";

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
