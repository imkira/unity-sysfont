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

[CustomEditor(typeof(UISysFontLabel))]
public class UISysFontLabelEditor : UIWidgetInspector
{
  protected UISysFontLabel _label;

  protected override bool OnDrawProperties()
  {
    _label = (UISysFontLabel)target;
    ISysFontTexturableEditor.DrawInspectorGUI(_label);
    return true;
  }

  [MenuItem("NGUI/Create a SysFont Label")]
  static public void AddLabel()
  {
    GameObject go = NGUIMenu.SelectedRoot();

    if (NGUIEditorTools.WillLosePrefab(go))
    {
      NGUIEditorTools.RegisterUndo("Add a SysFont Label", go);

      GameObject child = new GameObject("UISysFontLabel");
			child.layer = go.layer;
      child.transform.parent = go.transform;

      UISysFontLabel label = child.AddComponent<UISysFontLabel>();
      label.MakePixelPerfect();
      Vector3 pos = label.transform.localPosition;
      pos.z = -1f;
      label.transform.localPosition = pos;
      label.Text = "Hello World";
      Selection.activeGameObject = child;
    }
  }
}
