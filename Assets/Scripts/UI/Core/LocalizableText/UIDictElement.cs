/*
 * Author(s): Isaiah Mann
 * Description: Sets UI text based of dict lookup (based on language)
 * Usage: [no notes]
 */

public abstract class UIDictElement : UIElement 
{	
	#region Instance Accessors

	public bool HasLanguage
	{
		get 
		{
			return language != null;
		}
	}

	#endregion

	LanguageDatabase language;

	protected override void fetchReferences()
	{
		base.fetchReferences();
		language = LanguageDatabase.Instance;
		setTextFromDict();
	}

	protected abstract void setTextFromDict();

	protected string getTerm(string key)
	{
		if(HasLanguage)
		{
			return language.GetTerm(key);
		}
		else 
		{
			return string.Empty;
		}
	}

	protected void setTextFromKey(string key)
	{
		SetText(getTerm(key));
	}

}
