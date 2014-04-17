using UnityEngine;

public interface ILabel
{
	bool IsVisible
	{
		get;
		set;
	}

	string Text 
	{ 
		get; 
		set; 
	}

	int FontSize
	{ 
		get; 
		set; 
	}
	
	Label.Alignment Alignment
	{
		get;
		set;
	}

	Label.Pivot Pivot
	{
		get;
		set;
	}

	Color FontColor
	{
		get;
		set;
	}

	bool IsMultiLine
	{
		get;
		set;
	}

	int MaxWidth
	{
		get;
		set;
	}

	Transform Transform 
	{
		get;
	}
}

namespace Label
{
	public enum Alignment 
	{
		Left = 0,
		Center = 1,
		Right = 2
	}
	
	public enum Pivot
	{
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}
}