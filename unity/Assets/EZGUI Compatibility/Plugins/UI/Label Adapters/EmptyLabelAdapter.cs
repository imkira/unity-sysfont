using UnityEngine;
using System.Collections;

public class EmptyLabelAdapter : ILabel 
{
	public bool IsVisible
	{
		get;
		set;
	}
	
	public string Text 
	{ 
		get; 
		set; 
	}
	
	public int FontSize
	{ 
		get; 
		set; 
	}
	
	public Label.Alignment Alignment
	{
		get;
		set;
	}
	
	public Label.Pivot Pivot
	{
		get;
		set;
	}
	
	public Color FontColor
	{
		get;
		set;
	}
	
	public bool IsMultiLine
	{
		get;
		set;
	}
	
	public int MaxWidth
	{
		get;
		set;
	}

	public Transform Transform
	{
		get { return null; }
	}
}
