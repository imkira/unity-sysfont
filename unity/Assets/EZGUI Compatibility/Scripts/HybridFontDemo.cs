using UnityEngine;
using System.Collections;

public class HybridFontDemo : MonoBehaviour
{
	private const int BTN_X = 10;
	private const int BTN_Y = 10;
	
	private const int BTN_W = 100;
	private const int BTN_H = 60;

	private const int PADDING_Y = 5;
	
	[SerializeField]
	private UIHybridLabel labelA;

	[SerializeField]
	private UIHybridLabel labelB;
	
	private Rect rect;
	private int x, row;
	
	private void OnGUI ()
	{
		x = BTN_X;
		row = 0;
		if (DrawButton ("change text", x, row))
		{
			labelA.Text = Time.realtimeSinceStartup.ToString ();
		}

		row++;
		if (DrawButton ("use ezgui", x, row))
		{
			labelA.UseDefaultLabel = true;
		}

		row++;
		if (DrawButton ("use sysfont", x, row))
		{
			labelA.UseDefaultLabel = false;
		}

		x = Screen.width - BTN_X - BTN_W;
		row = 0;
		if (DrawButton ("change text", x, row))
		{
			labelB.Text = Time.realtimeSinceStartup.ToString ();
		}

		row++;
		if (DrawButton ("use ezgui", x, row))
		{
			labelB.UseDefaultLabel = true;
		}
		
		row++;
		if (DrawButton ("use sysfont", x, row))
		{
			labelB.UseDefaultLabel = false;
		}
	}
	
	private bool DrawButton (string label, int x, int rowIdx)
	{
		rect = new Rect (x, BTN_Y + ((BTN_H + PADDING_Y) * rowIdx), BTN_W, BTN_H);
		return GUI.Button (rect, label);
	}
}
