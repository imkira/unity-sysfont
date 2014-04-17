using UnityEngine;
using System.Collections;

public class HybridLabel
{
	private string text;
	private ILabel defaultLabel;
	private ILabel specialLabel;

	public string Text 
	{ 
		get 
		{
			return text; 
		}
		set
		{
			text = value;

			UpdateTextOfEnabledLabel (value);
		}
	}

	public ILabel EnabledLabel
	{
		get
		{
			return (defaultLabel.IsVisible) ? defaultLabel : specialLabel;
		}
	}

	private ILabel DisabledLabel
	{
		get
		{
			return (!defaultLabel.IsVisible) ? defaultLabel : specialLabel;
		}
	}

	public bool UseDefaultLabel
	{
		set
		{
			defaultLabel.IsVisible = value;
			specialLabel.IsVisible = !value;

			UpdateTextOfEnabledLabel (DisabledLabel.Text);
		}
	}

	public HybridLabel (ILabel defaultLabel)
	{
		this.defaultLabel = defaultLabel;
		this.specialLabel = new EmptyLabelAdapter ();
		UseDefaultLabel = true;
	}

	public HybridLabel (ILabel defaultLabel, ILabel specialLabel) : this (defaultLabel)
	{
		this.specialLabel = specialLabel;
		UseDefaultLabel = true;
	}

	public HybridLabel (ILabel defaultLabel, ILabelFactory labelFactory) : this (defaultLabel)
	{
		specialLabel = labelFactory.CreateLabel (defaultLabel);
		UseDefaultLabel = true;
	}

	private void UpdateTextOfEnabledLabel (string value)
	{
		if (!string.IsNullOrEmpty (value))
		{
			EnabledLabel.Text = value;
		}
	}
}
