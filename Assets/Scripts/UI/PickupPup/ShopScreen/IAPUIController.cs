/*
 * Author(s): Isaiah Mann
 * Description: Controls the display and behaviour of the In-App Purchases
 * Usage: [no notes]
 */

using UnityEngine;

public class IAPUIController : PPUIController
{
	[SerializeField]
	PPUIButton buy1000CoinsButton;

	#region MonoBehaviourExtended Overrides

	protected override void fetchReferences()
	{
		base.fetchReferences();
		buy1000CoinsButton.SubscribeToClick(IAPController.Instance.PurchaseCoins1000);
	}

	#endregion

}
