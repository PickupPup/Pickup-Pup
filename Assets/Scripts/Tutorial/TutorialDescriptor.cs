/*
 * Author(s): Isaiah Mann
 * Description: Describes a single tutorial
 * Usage: [no notes]
 */

[System.Serializable]
public class TutorialDescriptor : Descriptor
{
    #region Instance Accessors 

    public string ID
    {
        get;
        private set;
    }

    public bool IsComplete
    {
        get;
        private set;
    }

    #endregion

    public TutorialDescriptor(string id)
    {
        this.ID = id;
    }

    public void Complete()
    {
        this.IsComplete = true;
    }

    public void Reset()
    {
        this.IsComplete = false;
    }

    #region Object Overrides 

    public override bool Equals(object obj)
    {
        if(obj is TutorialDescriptor)
        {
            TutorialDescriptor other = obj as TutorialDescriptor;
            return this.GetHashCode().Equals(other.GetHashCode());
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public override int GetHashCode()
    {
        return this.ID.GetHashCode();
    }

    #endregion

}
