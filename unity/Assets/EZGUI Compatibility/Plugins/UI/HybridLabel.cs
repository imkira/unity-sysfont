using UnityEngine;
using System.Collections;

public class HybridLabel
{
	private string text;
	private ILabelAdapter defaultLabel;
	private ILabelAdapter specialLabel;

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

	public ILabelAdapter EnabledLabel
	{
		get
		{
			return (defaultLabel.IsVisible) ? defaultLabel : specialLabel;
		}
	}

	private ILabelAdapter DisabledLabel
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

	public HybridLabel (ILabelAdapter defaultLabel)
	{
		this.defaultLabel = defaultLabel;
		this.specialLabel = new EmptyLabelAdapter ();
		UseDefaultLabel = true;
	}

	public HybridLabel (ILabelAdapter defaultLabel, ILabelAdapter specialLabel) : this (defaultLabel)
	{
		this.specialLabel = specialLabel;
		UseDefaultLabel = true;
	}

	public HybridLabel (ILabelAdapter defaultLabel, ILabelFactory labelFactory) : this (defaultLabel)
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
