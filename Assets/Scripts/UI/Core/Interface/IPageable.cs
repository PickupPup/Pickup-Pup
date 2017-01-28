/*
 * Author(s): Isaiah Mann
 * Description: Interface w/ a UI element w/ multiple pages (that can be freely swapped between)
 * Usage: [no notes]
 */

public interface IPageable 
{
    #region Instance Accessors 

    bool PageWrapAllowed
    {
        get;
    }

    bool CanPageForward
    {
        get;
    }

    bool CanPageBackward
    {
        get;
    }
        
    #endregion

    void PageForward();
    void PageBackward();

}
