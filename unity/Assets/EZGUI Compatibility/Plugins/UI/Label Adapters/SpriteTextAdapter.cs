using UnityEngine;
using System.Collections;

public class SpriteTextAdapter : SpriteText, ILabel
{
	private Transform _transform;

	public bool IsVisible
	{
		get 
		{ 
			return !IsHidden (); 
		}
		set
		{
			Hide (!value);
		}
	}
	
	public int FontSize
	{ 
		get
		{
			return (int)characterSize;
		}
		set
		{
			characterSize = (float)value;
		}
	}
	
	public Label.Alignment Alignment
	{
		get
		{
			return (Label.Alignment)alignment;
		}
		set
		{
			alignment = (Alignment_Type)value;
		}
	}
	
	public Label.Pivot Pivot
	{
		get
		{
			return (Label.Pivot)anchor;
		}
		set
		{
			anchor = (Anchor_Pos)value;
		}
	}
	
	public Color FontColor
	{
		get
		{
			return Color;
		}
		set
		{
			SetColor (value);
		}
	}
	
	public bool IsMultiLine
	{
		get
		{
			return multiline;
		}
		set
		{
			multiline = value;
		}
	}
	
	public int MaxWidth
	{
		get
		{
			return (int)maxWidth;
		}
		set
		{
			maxWidth = (int)value;
		}
	}

	public Transform Transform
	{
		get { return transform; }
	}

	protected override void Awake ()
	{
		maxWidthInPixels = true;
		base.Awake ();
	}
}
