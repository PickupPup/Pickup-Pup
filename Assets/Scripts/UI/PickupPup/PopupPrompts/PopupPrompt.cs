using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPrompt : PPUIElement
{
    protected override void fetchReferences()
    {
        base.fetchReferences();
        SetText("hello");
    }

}
