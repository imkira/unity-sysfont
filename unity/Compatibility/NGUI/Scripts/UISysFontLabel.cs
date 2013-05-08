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

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/SysFont Label")]
public class UISysFontLabel : UIWidget, ISysFontTexturable
{
  [SerializeField]
  protected SysFontTexture _texture = new SysFontTexture();

  #region ISysFontTexturable properties
  public string Text
  {
    get
    {
      return _texture.Text;
    }
    set
    {
      _texture.Text = value;
    }
  }

  public string AppleFontName
  {
    get
    {
      return _texture.AppleFontName;
    }
    set
    {
      _texture.AppleFontName = value;
    }
  }

  public string AndroidFontName
  {
    get
    {
      return _texture.AndroidFontName;
    }
    set
    {
      _texture.AndroidFontName = value;
    }
  }

  public string FontName
  {
    get
    {
      return _texture.FontName;
    }
    set
    {
      _texture.FontName = value;
    }
  }

  public int FontSize
  {
    get
    {
      return _texture.FontSize;
    }
    set
    {
      _texture.FontSize = value;
    }
  }

  public bool IsBold
  {
    get
    {
      return _texture.IsBold;
    }
    set
    {
      _texture.IsBold = value;
    }
  }

  public bool IsItalic
  {
    get
    {
      return _texture.IsItalic;
    }
    set
    {
      _texture.IsItalic = value;
    }
  }

  public SysFont.Alignment Alignment
  {
    get
    {
      return _texture.Alignment;
    }
    set
    {
      _texture.Alignment = value;
    }
  }

  public bool IsMultiLine
  {
    get
    {
      return _texture.IsMultiLine;
    }
    set
    {
      _texture.IsMultiLine = value;
    }
  }

  public int MaxWidthPixels
  {
    get
    {
      return _texture.MaxWidthPixels;
    }
    set
    {
      _texture.MaxWidthPixels = value;
    }
  }

  public int MaxHeightPixels
  {
    get
    {
      return _texture.MaxHeightPixels;
    }
    set
    {
      _texture.MaxHeightPixels = value;
    }
  }

  public int WidthPixels 
  {
    get
    {
      return _texture.WidthPixels;
    }
  }

  public int HeightPixels 
  {
    get
    {
      return _texture.HeightPixels;
    }
  }

  public int TextWidthPixels 
  {
    get
    {
      return _texture.TextWidthPixels;
    }
  }

  public int TextHeightPixels 
  {
    get
    {
      return _texture.TextHeightPixels;
    }
  }

  public Texture Texture
  {
    get
    {
      return _texture.Texture;
    }
  }
  #endregion
  static protected Shader _shader = null;
  protected Material _createdMaterial = null;
  protected Vector3[] _vertices = null;
  protected Vector2 _uv;

  #region UIWidget
	public override bool keepMaterial
  {
    get
    {
      return true;
    }
  }

  public override bool OnUpdate()
  {
    if (_texture.NeedsRedraw)
    {
      _texture.Update();
      _uv = new Vector2(_texture.TextWidthPixels /
          (float)_texture.WidthPixels, _texture.TextHeightPixels /
          (float)_texture.HeightPixels);
      return true;
    }
    return false;
  }

  override public void MakePixelPerfect()
  {
    Vector3 scale = cachedTransform.localScale;
    if (_texture.TextWidthPixels <= 1)
    {
      scale.x = 1f;
    }
    else
    {
      scale.x = _texture.TextWidthPixels;
    }
    if (_texture.TextHeightPixels <= 1)
    {
      scale.y = 1f;
    }
    else
    {
      scale.y = _texture.TextHeightPixels;
    }
    cachedTransform.localScale = scale;

    base.MakePixelPerfect();
  }

#if (UNITY_3_4 || UNITY_3_5_0 || UNITY_3_5_1 || UNITY_3_5_2 || UNITY_3_5_3 || UNITY_3_5_4)
  override public void OnFill(BetterList<Vector3> verts,
      BetterList<Vector2> uvs, BetterList<Color> cols)
#else
  override public void OnFill(BetterList<Vector3> verts,
      BetterList<Vector2> uvs, BetterList<Color32> cols)
#endif
  {
    if (_vertices == null)
    {
      _vertices = new Vector3[4];
      _vertices[0] = new Vector3(1f,  0f, 0f);
      _vertices[1] = new Vector3(1f, -1f, 0f);
      _vertices[2] = new Vector3(0f, -1f, 0f);
      _vertices[3] = new Vector3(0f,  0f, 0f);
    }

    verts.Add(_vertices[0]);
    verts.Add(_vertices[1]);
    verts.Add(_vertices[2]);
    verts.Add(_vertices[3]);

    uvs.Add(_uv);
    uvs.Add(new Vector2(_uv.x, 0f));
    uvs.Add(Vector2.zero);
    uvs.Add(new Vector2(0f, _uv.y));

    cols.Add(color);
    cols.Add(color);
    cols.Add(color);
    cols.Add(color);

    MakePixelPerfect();

    if (material.mainTexture != _texture.Texture)
    {
      material.mainTexture = _texture.Texture;
    }
  }
  #endregion

  #region MonoBehaviour methods
  protected override void OnEnable()
  {
    if (_shader == null)
    {
      _shader = Shader.Find("Unlit/Transparent Colored (Packed)");
    }

    if (_createdMaterial == null)
    {
      _createdMaterial = new Material(_shader);
      _createdMaterial.hideFlags =
        HideFlags.HideInInspector | HideFlags.DontSave;
      _createdMaterial.mainTexture = _texture.Texture;
      material = _createdMaterial;
    }
  }

  protected void OnDestroy()
  {
    material = null;
    SysFont.SafeDestroy(_createdMaterial);
    if (_texture != null)
    {
      _texture.Destroy();
      _texture = null;
    }
	}
  #endregion
}
