/*
 * Author(s): Isaiah Mann
 * Description: Controls a Canvas in the Unity UI System
 * Usage: [no notes]
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UICanvas : UIElement
{
    static int topSortLayer 
    {
        get 
        {
            return canvasLayers.Count - SINGLE_VALUE;
        }
    }

    static bool layerStackEmpty
    {
        get 
        {
            return canvasLayers.Count == NONE_VALUE;
        }
    }

    static Stack<UICanvas> canvasLayers = new Stack<UICanvas>();

    bool trackedInAutoSort
    {
        get 
        {
            return canvasLayers.Contains(this);
        }
    }


    bool overrideAutoSort
    {
        get 
        {
            return sortRule == UICanvasSortType.Ignore;
        }
    }
        
    bool isTopLayerCanvas
    {
        get
        {
            return sortRule == UICanvasSortType.TopLayer;
        }
    }

    bool isBottomLayerCanvas
    {
        get
        {
            return sortRule == UICanvasSortType.BottomLayer;
        }
    }

    [SerializeField] 
    UICanvasSortType sortRule;

    Canvas layer;

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        layer = GetComponent<Canvas>();
        if(shouldAutoSort())
        {
            addToAutoSort();
        }
    }

    protected override void cleanupReferences()
    {
        Hide();
        base.cleanupReferences();
    }

    public override void Destroy()
    {
        Hide();
        base.Destroy();
    }
        
    #endregion

    protected virtual bool shouldAutoSort()
    {
        return !overrideAutoSort && isActiveAndEnabled;
    }

    #region UIElement Overrides 

    public override void Show()
    {
        base.Show();
        if(shouldAutoSort())
        {
            addToAutoSort();
        }

    }

    public override void Hide()
    {
        if(trackedInAutoSort)
        {
            removeFromAutoSort();
        }
        base.Hide();
    }

    #endregion

    void addToAutoSort()
    {
        if(!trackedInAutoSort)
        {
            Stack<UICanvas> canvasBuffer;
            if(this.isTopLayerCanvas)
            {
                // Set to default empty Stack to prevent null ref errors
                canvasBuffer = new Stack<UICanvas>(NONE_VALUE);
            }
            else if(isBottomLayerCanvas)
            {
                canvasBuffer = moveAllNonBottomCanvasesToBuffer();
            } 
            else 
            {
                canvasBuffer = moveTopLayerCanvasesToBuffer();
            }
            this.setSortLayer(getNextSortLayer());
            canvasLayers.Push(this);
            if(!this.isTopLayerCanvas)
            {
                addLayersFromBuffer(canvasBuffer);
            }
        }
    }

    Stack<UICanvas> moveTopLayerCanvasesToBuffer()
    {
        Stack<UICanvas> canvasBuffer = new Stack<UICanvas>();
        while(!layerStackEmpty && canvasLayers.Peek().isTopLayerCanvas)
        {
            canvasBuffer.Push(canvasLayers.Pop());
        }
        return canvasBuffer;
    }

    Stack<UICanvas> moveAllNonBottomCanvasesToBuffer()
    {
        Stack<UICanvas> canvasBuffer = moveTopLayerCanvasesToBuffer();
        while(!layerStackEmpty && !canvasLayers.Peek().isBottomLayerCanvas)
        {
            canvasBuffer.Push(canvasLayers.Pop());
        }
        return canvasBuffer;
    }

    void removeFromAutoSort()
    {
        Stack<UICanvas> canvasBuffer = new Stack<UICanvas>();
        UICanvas currentCanvas;
        do
        {
            currentCanvas = canvasLayers.Pop();
            canvasBuffer.Push(currentCanvas);
        }
        while(currentCanvas != this);

        // Remove this canvas from the stack
        canvasBuffer.Pop();
        addLayersFromBuffer(canvasBuffer);
    }

    void addLayersFromBuffer(Stack<UICanvas> buffer)
    {
        while(buffer.Count > NONE_VALUE)
        {
            UICanvas currentCanvas = buffer.Pop();
            currentCanvas.setSortLayer(getNextSortLayer());
            canvasLayers.Push(currentCanvas);   
        }
    }

    int getNextSortLayer()
    {
        return topSortLayer + SINGLE_VALUE;
    }

    void setSortLayer(int sortLayer)
    {
        layer.sortingOrder = sortLayer;
    }

}

public enum UICanvasSortType
{
    Standard,
    TopLayer,
    BottomLayer,
    Ignore,

}
