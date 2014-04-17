using UnityEngine;
using System.Collections;

public class SysFontAdapter : SysFontText, ILabelAdapter
{
	private const int MAX_WIDTH = 2048;
	private const string NAME_SUFFIX = "(SysFont)";

	private Transform _parent;
	
	public bool IsVisible
	{
		get 
		{ 
			return gameObject.activeSelf; 
		}
		set
		{
			gameObject.SetActive (value);
		}
	}
	
	public new Label.Alignment Alignment
	{
		get
		{
			return (Label.Alignment)base.Alignment;
		}
		set
		{
			base.Alignment = (SysFont.Alignment)value;
		}
	}
	
	public new Label.Pivot Pivot
	{
		get
		{
			return (Label.Pivot)base.Pivot;
		}
		set
		{
			base.Pivot = (PivotAlignment)value;
		}
	}
	
	public int MaxWidth
	{
		get 
		{
			return base.MaxWidthPixels; 
		}
		set
		{
			base.MaxWidthPixels = value > 0 ? value : MAX_WIDTH;
		}
	}

	public Transform Transform
	{
		get { return _transform; }
	}

	public void CopyPropertiesOf (ILabelAdapter baseLabel)
	{
		Text = baseLabel.Text;
		FontSize = baseLabel.FontSize;
		Alignment = baseLabel.Alignment;
		Pivot = baseLabel.Pivot;
		FontColor = baseLabel.FontColor;
		MaxWidth = baseLabel.MaxWidth;
	}

	public void SetParent (Transform parent)
	{
		_parent = parent;

		if (_parent != null)
		{
			Transform.parent = _parent.parent;
			Transform.localPosition = _parent.localPosition;
			gameObject.layer = _parent.gameObject.layer;
			name = string.Format ("{0} {1}", _parent.name, NAME_SUFFIX);
		}
	}
}
