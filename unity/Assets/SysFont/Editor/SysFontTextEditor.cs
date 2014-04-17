/*
 * Copyright (c) 2012 Mario Freitas (imkira@gmail.com)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SysFontText), true)]
public class SysFontTextEditor : SysFontEditor
{
  protected SysFontText _text;

  public override void OnInspectorGUI()
  {
    _text = (SysFontText)target;

    ISysFontTexturableEditor.DrawInspectorGUI(_text);

    DrawInspectorGUI(_text);
  }

  public static void DrawInspectorGUI(SysFontText text)
  {
    GUILayout.BeginHorizontal();
    {
      LookLikeControls(70f);

      //
      // FontColor property
      //
      Color fontColor;
      fontColor = EditorGUILayout.ColorField("Font Color",
          text.FontColor, GUILayout.Width(160f));
      if (fontColor != text.FontColor)
      {
        RegisterUndo(text, "SysFont Color Change");
        text.FontColor = fontColor;
      }

      LookLikeControls(40f);

      //
      // Pivot property
      //
      SysFontText.PivotAlignment pivot;
      pivot = (SysFontText.PivotAlignment)EditorGUILayout.EnumPopup("Pivot",
          text.Pivot, GUILayout.Width(130f));
      if (pivot != text.Pivot)
      {
        RegisterUndo(text, "SysFont Pivot Change");
        text.Pivot = pivot;
      }

      LookLikeControls();
    }
    GUILayout.EndHorizontal();
  }
}
