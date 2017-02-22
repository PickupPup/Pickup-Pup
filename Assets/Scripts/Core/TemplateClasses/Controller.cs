/*
 * Author(s): Isaiah Mann
 * Description: Meta class that all controllers inherit from (all controllers are singletons)
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Controller : MonoBehaviourExtended 
{
    const int ORDERS_OF_EXECUTION = 2;
    const int FIRST_ORDER_EXECUTION_IDX = 0;
    const int STANDARD_ORDER_EXECUTION_IDX = 1;

    static HashSet<Controller> allSystems = new HashSet<Controller>();

    protected virtual bool hasFirstOrderExecution
    {
        get
        {
            return false;
        }
    }

    protected virtual bool shouldReSetRefsOnReset
    {
        get
        {
            return true;
        }
    }

    protected virtual bool shouldReFetchRefsOnReset
    {
        get
        {
            return true;
        }
    }

    protected bool setReferencesOverride
    {
        get
        {
            if(_setReferencesOverride)
            {
                _setReferencesOverride = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        private set
        {
            _setReferencesOverride = value;
        }
    }

    int getExecutionOrderIdx
    {
        get
        {
            if(hasFirstOrderExecution)
            {
                return FIRST_ORDER_EXECUTION_IDX;
            }
            else
            {
                return STANDARD_ORDER_EXECUTION_IDX;
            }
        }
    }

    bool _setReferencesOverride = false;

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        allSystems.Add(this);
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        allSystems.Remove(this);
    }

    #endregion
   
    protected virtual void refreshAllSystems()
    {
        refreshSystems(allSystems);
    }

    protected virtual void refreshSystems(IEnumerable collection)
    {
        HashSet<Controller>[] orderedSystems = getSystemsByExecutionOrders(collection);
        for(int i = 0; i < orderedSystems.Length; i++)
        {
            foreach(Controller controller in orderedSystems[i])
            {
                if(controller.shouldReSetRefsOnReset)
                {
                    controller.forceSetReferences();
                }
            }
        }
        // Break up calls to simulate runtime execution order of MonoBehaviour (Awake --> Start)
        for(int i = 0; i < orderedSystems.Length; i++)
        {
            foreach(Controller controller in orderedSystems[i])
            {
                if(controller.shouldReFetchRefsOnReset)
                {
                    controller.reFetchReferences();
                }
            }
        }
    }
        
    protected virtual void reSetReferences()
    {
        referencesSet = false;
        setReferences();
    }

    protected virtual void reFetchReferences()
    {
        referencesSet = false;
        fetchReferences();
    }

    HashSet<Controller>[] getSystemsByExecutionOrders(IEnumerable unsortedSystems)
    {
        HashSet<Controller>[] ordereredSystems = new HashSet<Controller>[ORDERS_OF_EXECUTION];
        for(int i = 0; i < ordereredSystems.Length; i++)
        {
            ordereredSystems[i] = new HashSet<Controller>();
        }
        foreach(Controller controller in unsortedSystems)
        {
            ordereredSystems[controller.getExecutionOrderIdx].Add(controller);
        }
        return ordereredSystems;
    }
        
    void forceSetReferences()
    {
        setReferencesOverride = true;
        reSetReferences();
    }

}
