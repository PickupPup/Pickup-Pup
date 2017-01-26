/*
 * Author(s): Isaiah Mann
 * Description: Marks a currency that is a discount
 * Usage: [no notes]
 */

[System.Serializable]
public class DiscountData : CurrencyData
{
    #region Public Accessors 

    public float DiscountDecimal 
    {
        get
        {
            return _discount;
        }
           
    }

    #endregion

    float _discount;

    public DiscountData(float discountPercentAsDecimal, int numDiscounts):
    base(CurrencyType.DogDiscount, numDiscounts)
    {
        this._discount = discountPercentAsDecimal;
    }

}
