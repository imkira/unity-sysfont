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

[CustomEditor(typeof(SysFontTexture))]
public class SysFontTextureEditor : Editor
{
  protected SysFontTexture _texture;

  public override void OnInspectorGUI()
  {
    _texture = (SysFontTexture)target;

    LookLikeControls();

    //
    // Text property
    //
    string text = string.IsNullOrEmpty(_texture.Text) ? "" : _texture.Text;

    EditorGUILayout.LabelField("Text");
    GUI.skin.textArea.wordWrap = true;
    text = EditorGUILayout.TextArea(text, GUI.skin.textArea,
        GUILayout.Height(50f));

    if (text.Equals(_texture.Text) == false)
    {
      RegisterUndo("SysFont Text Change");
      _texture.Text = text;
    }

    GUILayout.BeginHorizontal();
    {
      LookLikeControls(60f);
      EditorGUILayout.PrefixLabel("Font Size");
      //
      // FontSize property
      //
      int fontSize = EditorGUILayout.IntField(_texture.FontSize,
          GUILayout.Width(30f));
      if (fontSize != _texture.FontSize)
      {
        RegisterUndo("SysFont Font Size Change");
        _texture.FontSize = fontSize;
      }

      LookLikeControls(30f);

      //
      // IsBold property
      //
      EditorGUILayout.PrefixLabel("Bold");
      bool isBold = EditorGUILayout.Toggle(_texture.IsBold,
          GUILayout.Width(30f));
      if (isBold != _texture.IsBold)
      {
        RegisterUndo("SysFont Style Change");
        _texture.IsBold = isBold;
      }

      //
      // IsItalic property
      //
      EditorGUILayout.PrefixLabel("Italic");
      bool isItalic = EditorGUILayout.Toggle(_texture.IsItalic,
          GUILayout.Width(30f));
      if (isItalic != _texture.IsItalic)
      {
        RegisterUndo("SysFont Style Change");
        _texture.IsItalic = isItalic;
      }

      LookLikeControls(60f);

      //
      // Pivot property
      //
      SysFont.Alignment alignment;
      alignment = (SysFont.Alignment)EditorGUILayout.EnumPopup("Alignment",
          _texture.Alignment, GUILayout.Width(120f));
      if (alignment != _texture.Alignment)
      {
        RegisterUndo("SysFont Alignment Change");
        _texture.Alignment = alignment;
      }
    }
    GUILayout.EndHorizontal();

    LookLikeControls(100f);
    //
    // AppleFontName property
    //
    string appleFontName = EditorGUILayout.TextField("iOS/MacOSX Font",
        _texture.AppleFontName);
    if (appleFontName != _texture.AppleFontName)
    {
      RegisterUndo("SysFont Font Name Change");
      _texture.AppleFontName = appleFontName;
    }

    //
    // AndroidFontName property
    //
    string fontName = EditorGUILayout.TextField("Android Font",
        _texture.AndroidFontName);
    if (fontName != _texture.AndroidFontName)
    {
      RegisterUndo("SysFont Font Name Change");
      _texture.AndroidFontName = fontName;
    }
    LookLikeControls();

    GUILayout.BeginHorizontal();
    {
      LookLikeControls(60f);

      //
      // IsMultiLine property
      //
      bool isMultiLine = EditorGUILayout.Toggle("Multi Line",
          _texture.IsMultiLine, GUILayout.Width(80f));
      if (isMultiLine != _texture.IsMultiLine)
      {
        RegisterUndo("SysFont Is Multi Line Change");
        _texture.IsMultiLine = isMultiLine;
      }

      LookLikeControls(65f);

      //
      // MaxWidthPixels property
      //
      int maxWidthPixels = EditorGUILayout.IntField("Max Width",
          _texture.MaxWidthPixels, GUILayout.Width(110f));
      if (maxWidthPixels != _texture.MaxWidthPixels)
      {
        RegisterUndo("SysFont Max Width Pixels Change");
        _texture.MaxWidthPixels = maxWidthPixels;
      }

      //
      // MaxHeightPixels property
      //
      int maxHeightPixels = EditorGUILayout.IntField("Max Height",
          _texture.MaxHeightPixels, GUILayout.Width(110f));
      if (maxHeightPixels != _texture.MaxHeightPixels)
      {
        RegisterUndo("SysFont Max Height Pixels Change");
        _texture.MaxHeightPixels = maxHeightPixels;
      }

      LookLikeControls();
    }
    GUILayout.EndHorizontal();
  }

  protected void LookLikeControls(float labelWidth = 70f)
  {
    EditorGUIUtility.LookLikeControls(labelWidth);
  }

  protected virtual void RegisterUndo(string name)
  {
    Undo.RegisterUndo(_texture, name);
    EditorUtility.SetDirty(_texture);
  }
}
