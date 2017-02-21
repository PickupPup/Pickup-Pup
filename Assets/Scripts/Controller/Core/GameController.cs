/*
 * Author: Isaiah Mann
 * Description: Game Controller class
 */

public class GameController : SingletonController<GameController> 
{
    protected override bool hasFirstOrderExecution {
        get 
        {
            return true;
        }
    }

    // Requires caller to authenticate this was not a use violation
    public virtual void HandleSystemReset(object caller)
    {
        if(caller is PPSceneController || caller is DataController)
        {
            refreshAllSystems();
        }
    }

}
