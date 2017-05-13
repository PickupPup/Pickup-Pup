/*
 * Author(s): Isaiah Mann
 * Description: Pickup Pup Specific globals
 * Usage: [no notes]
 */

using System.Text.RegularExpressions;
public class PPGlobal : Global
{
	public const string SHOP_ITEMS = "ShopItems";
	public const string GIFT_ITEMS = "GiftItems";
	public const string SAVE_FILE = "PickupPupSave.dat";
	public const string CURRENCY = "Currency";
	public const string ADOPTED = "Adopted";
	public const string SCOUTING = "Scouting";
	public const string TIME_STAMP = "TimeStamp";
	public const string DOG_GIFT_REPORT_FORMAT = "{0} brought you {1} {2}";
	public const string GENERIC_GIFT_REPORT_FORMAT = "You received {0} {1}";
    public const string REDEEM_DISPLAY_TEXT_KEY = "RedeemDisplayText";
	public const string DAILY = "Daily";
	public const string GIFT = "Gift";
	public const string COUNTDOWN = "Countdown";
	public const string DAILY_GIFT_COUNTDOWN = DAILY + GIFT + COUNTDOWN;
	public const string HAS_GIFT_TO_REDEEM = "HasGiftToRedeem";
    public const string COIN_ICON = "Coin";
    public const string FIND = "Find";
    public const string REDEEM = "Redeem";
    public const string FIND_GIFT = FIND + GIFT;
    public const string REDEEM_GIFT = REDEEM + GIFT;
    public const string TAP_TO_REDEEM = "TapToRedeem";
    public const string DOG_FOOD_ICON = "dogfood_icon";
    public const string NAV_DROPDOWN = "NavDropdown";
    public const string WORLD = "World";
    public const string SOUVENIRS = "Souvenirs";
	public const string FOOD = "Food";
    public const string UI_SPRITESHEET = "ui_spritesheet";
	public const string DOG_VOUCHER_MESSAGE = "DogVoucherMessage";
    public const string BREED = "Breed";
    public const string Color = "Color";
    public const string DOG_TIMER_LENGTH = "Dog Timer Length";
    public const string PLAY_TIME = "Session Play Time";
    public const string FEED_COUNT = "Feed Count";
    public const string ADOPTED_DOGS_COUNT = "Adopted Dogs Count";
	public const string DEFAULT_FOOD_TYPE = "Regular";
	public const string DEFAULT_FOOD_SPRITE = "WhiteBowl_Empty";
	public const string DEFAULT_FOOD_COLOR = "Red";
	public const string DEFAULT_FOOD_COLOR_HEX = "#FF0000";
    public const float FULL_PERCENT = 100f;
    public const float DEFAULT_TIME_TO_COLLECT = 10f;
    public const float DEFAULT_DISCOUNT_DECIMAL = 0.25f;

    public const int DEFAULT_COINS = 2000;
    public const int DEFAULT_DOG_FOOD = 0;
    public const int DEFAULT_CURRENCY_AMOUNT = 0;
    public const int STANDARD_DROPDOWN = 0;
    public const int ALT_SINGLE_DROPDOWN = 1;

    #region Audio System

    public const string PLAY = "Play";
    public const string STOP = "Stop";

    public const string EMPTY = "Empty";
    public const string BACK = "Back";
    public const string PURCHASE = "Purchase";
    public const string ADOPT = "Adopt";
    public const string BARK = "Bark";
    public const string GIFT_REDEEM = "GiftRedeem";
    public const string DOG_RETURN = "DogReturn";
    public const string DOG_SENDOUT = "DogSendOut";
    public const string MENU_CLICK = "MenuClick";
    public const string MENU_POPUP = "MenuPopup";
    public const string MAIN_MUSIC = "MainMusic";
    public const string CHANGE_SCENE = "ChangeScene";
    public const string ADD_FOOD = "AddFood";
    public const string EAT_FOOD = "Eating";
    public const string PET = "Pet";

    public static string GetPlayEvent(string id)
    {
        return PLAY + id;
    }

    public static string GetStopEvent(string id)
    {
        return STOP + id;
    }

    #endregion

}
