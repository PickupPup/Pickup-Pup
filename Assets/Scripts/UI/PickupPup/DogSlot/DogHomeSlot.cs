/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls a DogSlot for a Dog currently at home (no text).
 */

public class DogHomeSlot : DogSlot
{
    protected override Dog dog
    {
        get
        {
            return base.dog;
        }
        set
        {
            // A chance to handle any cleanup
            if (_dog != null)
            {
                handleChangeDog(_dog);
            }
            // Do not reassign slot
            _dog = value;
        }
    }
}
