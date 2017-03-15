using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour {

	public void Buy()
    {
        IAPManager.Instance.Buy();
    }
}
