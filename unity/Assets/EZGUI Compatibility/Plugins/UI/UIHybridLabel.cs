using UnityEngine;

[RequireComponent (typeof (SpriteTextAdapter))]
public class UIHybridLabel : MonoBehaviour
{
	[SerializeField]
	private SysFontAdapter sysfontLabel;

	private SpriteTextAdapter ezguiLabel;

	private HybridLabel hybrid;

	public string Text
	{
		get
		{
			return hybrid.Text;
		}
		set
		{
			hybrid.Text = value;
		}
	}

	public bool UseDefaultLabel
	{
		set 
		{
			hybrid.UseDefaultLabel = value;
		}
	}

	private void Awake ()
	{
		if (ezguiLabel == null)
		{
			ezguiLabel = GetComponent<SpriteTextAdapter> ();
		}

		if (sysfontLabel == null)
		{
			ILabelFactory sysFontFactory = new SysFontAdapterFactory ();
			hybrid = new HybridLabel (ezguiLabel, sysFontFactory);
		}
		else
		{
			hybrid = new HybridLabel (ezguiLabel, sysfontLabel);
		}

		hybrid.Text = Text;
	}
}
