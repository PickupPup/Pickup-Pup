/*
 * Author: Isaiah Mann
 * Description: Shows a hidden UI element
 */

using UnityEngine;

[CreateAssetMenuAttribute(fileName = "PageSwitch", menuName = "UIEvent/PageSwitch", order = 5)]
public class UIPageSwitchAction : UIAction 
{
	#region UIAction Overrides

	public override void Execute(UIElement target)
	{
        if(target is IPageable)
        {
            if(target.HasReceivedSwipe)
            {
                IPageable pageController = target as IPageable;
                switch(target.MostRecetSwipeDirection)
                {
                    case Direction.East:
                        if(pageController.CanPageBackward)
                        {
                            pageController.PageBackward();
                        }
                        break;
                    case Direction.West:
                        if(pageController.CanPageForward)
                        {
                            pageController.PageForward();
                        }
                        break;
                        
                }
            }
        }
        else
        {
            Debug.LogErrorFormat("Target {0} does not implement IPageable. Cannot switch pages", target);
        }
	}

	#endregion

}
