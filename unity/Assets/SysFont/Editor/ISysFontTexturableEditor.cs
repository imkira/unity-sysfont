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

public class ISysFontTexturableEditor : SysFontEditor
{
  public static void DrawInspectorGUI(Object target)
  {
    ISysFontTexturable texturable = (ISysFontTexturable)target;

    LookLikeControls();

    //
    // Text property
    //
    string text = string.IsNullOrEmpty(texturable.Text) ? "" : texturable.Text;

    EditorGUILayout.LabelField("Text");
    GUI.skin.textArea.wordWrap = true;
    text = EditorGUILayout.TextArea(text, GUI.skin.textArea,
        GUILayout.Height(50f));

    if (text.Equals(texturable.Text) == false)
    {
      RegisterUndo(target, "SysFont Text Change");
      texturable.Text = text;
    }

    GUILayout.BeginHorizontal();
    {
      LookLikeControls(60f);
      EditorGUILayout.PrefixLabel("Font Size");
      //
      // FontSize property
      //
      int fontSize = EditorGUILayout.IntField(texturable.FontSize,
          GUILayout.Width(30f));
      if (fontSize != texturable.FontSize)
      {
        RegisterUndo(target, "SysFont Font Size Change");
        texturable.FontSize = fontSize;
      }

      LookLikeControls(30f);

      //
      // IsBold property
      //
      EditorGUILayout.PrefixLabel("Bold");
      bool isBold = EditorGUILayout.Toggle(texturable.IsBold,
          GUILayout.Width(30f));
      if (isBold != texturable.IsBold)
      {
        RegisterUndo(target, "SysFont Style Change");
        texturable.IsBold = isBold;
      }

      //
      // IsItalic property
      //
      EditorGUILayout.PrefixLabel("Italic");
      bool isItalic = EditorGUILayout.Toggle(texturable.IsItalic,
          GUILayout.Width(30f));
      if (isItalic != texturable.IsItalic)
      {
        RegisterUndo(target, "SysFont Style Change");
        texturable.IsItalic = isItalic;
      }

      LookLikeControls(60f);

      //
      // Pivot property
      //
      SysFont.Alignment alignment;
      alignment = (SysFont.Alignment)EditorGUILayout.EnumPopup("Alignment",
          texturable.Alignment, GUILayout.Width(120f));
      if (alignment != texturable.Alignment)
      {
        RegisterUndo(target, "SysFont Alignment Change");
        texturable.Alignment = alignment;
      }
    }
    GUILayout.EndHorizontal();

    LookLikeControls(100f);
    //
    // AppleFontName property
    //
    string appleFontName = EditorGUILayout.TextField("iOS/MacOSX Font",
        texturable.AppleFontName);
    if (appleFontName != texturable.AppleFontName)
    {
      RegisterUndo(target, "SysFont Font Name Change");
      texturable.AppleFontName = appleFontName;
    }

    //
    // AndroidFontName property
    //
    string fontName = EditorGUILayout.TextField("Android Font",
        texturable.AndroidFontName);
    if (fontName != texturable.AndroidFontName)
    {
      RegisterUndo(target, "SysFont Font Name Change");
      texturable.AndroidFontName = fontName;
    }
    LookLikeControls();

    GUILayout.BeginHorizontal();
    {
      LookLikeControls(60f);

      //
      // IsMultiLine property
      //
      bool isMultiLine = EditorGUILayout.Toggle("Multi Line",
          texturable.IsMultiLine, GUILayout.Width(80f));
      if (isMultiLine != texturable.IsMultiLine)
      {
        RegisterUndo(target, "SysFont Is Multi Line Change");
        texturable.IsMultiLine = isMultiLine;
      }

      LookLikeControls(65f);

      //
      // MaxWidthPixels property
      //
      int maxWidthPixels = EditorGUILayout.IntField("Max Width",
          texturable.MaxWidthPixels, GUILayout.Width(110f));
      if (maxWidthPixels != texturable.MaxWidthPixels)
      {
        RegisterUndo(target, "SysFont Max Width Pixels Change");
        texturable.MaxWidthPixels = maxWidthPixels;
      }

      //
      // MaxHeightPixels property
      //
      int maxHeightPixels = EditorGUILayout.IntField("Max Height",
          texturable.MaxHeightPixels, GUILayout.Width(110f));
      if (maxHeightPixels != texturable.MaxHeightPixels)
      {
        RegisterUndo(target, "SysFont Max Height Pixels Change");
        texturable.MaxHeightPixels = maxHeightPixels;
      }

      LookLikeControls();
    }
    GUILayout.EndHorizontal();
  }
}
