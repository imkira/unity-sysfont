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
[AddComponentMenu("SysFont/Text")]
public class SysFontText : SysFontTexture
{
  [SerializeField]
  protected Color _fontColor = Color.white;

  [SerializeField]
  protected Alignment _pivot = Alignment.Center;

  protected Color _lastFontColor;
  public Color FontColor
  {
    get
    {
      return _fontColor;
    }
    set
    {
      if (_fontColor != value)
      {
        _fontColor = value;
      }
    }
  }

  protected Alignment _lastPivot;
  public Alignment Pivot 
  {
    get
    {
      return _pivot;
    }
    set
    {
      if (_pivot != value)
      {
        _pivot = value;
      }
    }
  }

  protected Transform _transform;

  protected Material _createdMaterial = null;
  protected Material _material = null;
  protected Vector3[] _vertices = null;
  protected Vector2[] _uv = null;
  protected int[] _triangles = null;
  protected Mesh _mesh = null;
  protected MeshFilter _filter = null;
  protected MeshRenderer _renderer = null;

  static protected Shader _shader = null;

  protected virtual void Awake()
  {
    _transform = transform;
  }

  protected override void Update()
  {
    base.Update();

    if ((_fontColor != _lastFontColor) && (_material != null))
    {
      _material.color = _fontColor;
      _lastFontColor = _fontColor;
    }

    if (_lastPivot != _pivot)
    {
      UpdatePivot();
    }
  }

  protected void UpdateMesh()
  {
    if (_filter == null)
    {
      _filter = gameObject.GetComponent<MeshFilter>();
      if (_filter == null)
      {
        _filter = gameObject.AddComponent<MeshFilter>();
        _filter.hideFlags = HideFlags.HideInInspector;
      }
    }

    if (_renderer == null)
    {
      _renderer = gameObject.GetComponent<MeshRenderer>();
      if (_renderer == null)
      {
        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.hideFlags = HideFlags.HideInInspector;
      }

      if (_shader == null)
      {
        _shader = Shader.Find("SysFont/Unlit Transparent");
      }

      if (_createdMaterial == null)
      {
        _createdMaterial = new Material(_shader);
      }
      _createdMaterial.hideFlags = HideFlags.HideInInspector | HideFlags.DontSave;
      _material = _createdMaterial;
      _renderer.sharedMaterial = _material;
    }

    _material.color = _fontColor;
    _lastFontColor = _fontColor;

    if (_uv == null)
    {
      _uv = new Vector2[4];
      _triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
    }

    Vector2 uv = new Vector2(_textWidthPixels / (float)_widthPixels,
        _textHeightPixels / (float)_heightPixels);

    _uv[0] = Vector2.zero; 
    _uv[1] = new Vector2(uv.x, 0f);
    _uv[2] = new Vector2(0f, uv.y);
    _uv[3] = uv;

    UpdatePivot();
    UpdateScale();
  }

  protected void UpdatePivot()
  {
    if (_vertices == null)
    {
      _vertices = new Vector3[4];
      _vertices[0] = Vector3.zero;
      _vertices[1] = Vector3.zero;
      _vertices[2] = Vector3.zero;
      _vertices[3] = Vector3.zero;
    }

    // horizontal
    if ((_pivot == Alignment.TopLeft) || (_pivot == Alignment.Left) ||
        (_pivot == Alignment.BottomLeft))
    {
      _vertices[0].x = _vertices[2].x = 0f;
      _vertices[1].x = _vertices[3].x = 1f;
    }
    else if ((_pivot == Alignment.TopRight) || (_pivot == Alignment.Right) ||
        (_pivot == Alignment.BottomRight))
    {
      _vertices[0].x = _vertices[2].x = -1f;
      _vertices[1].x = _vertices[3].x = 0f;
    }
    else
    {
      _vertices[0].x = _vertices[2].x = -0.5f;
      _vertices[1].x = _vertices[3].x = 0.5f;
    }

    // vertical
    if ((_pivot == Alignment.TopLeft) || (_pivot == Alignment.Top) ||
        (_pivot == Alignment.TopRight))
    {
      _vertices[0].y = _vertices[1].y = -1f;
      _vertices[2].y = _vertices[3].y = 0f;
    }
    else if ((_pivot == Alignment.BottomLeft) || (_pivot == Alignment.Bottom) ||
        (_pivot == Alignment.BottomRight))
    {
      _vertices[0].y = _vertices[1].y = 0f;
      _vertices[2].y = _vertices[3].y = 1f;
    }
    else
    {
      _vertices[0].y = _vertices[1].y = -0.5f;
      _vertices[2].y = _vertices[3].y = 0.5f;
    }

    if (_mesh == null)
    {
      _mesh = new Mesh();
      _mesh.name = "SysFontTextMesh";
      _mesh.hideFlags = HideFlags.DontSave | HideFlags.DontSave;
    }
    _mesh.vertices = _vertices;
    _mesh.uv = _uv;
    _mesh.triangles = _triangles;
    _mesh.RecalculateBounds();
    _filter.mesh = _mesh;

    _lastPivot = _pivot;
  }

  public void UpdateScale()
  {
    Vector3 scale = _transform.localScale;
    scale.x = (float)_textWidthPixels;
    scale.y = (float)_textHeightPixels;
    _transform.localScale = scale;
  }

  protected override void OnUpdated()
  {
    base.OnUpdated();

    UpdateMesh();

    _material.mainTexture = Texture;
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    _Destroy(_mesh);
    _Destroy(_createdMaterial);
    _createdMaterial = null;
    _material = null;
    _vertices = null;
    _uv = null;
    _triangles = null;
    _mesh = null;
    _filter = null;
    _renderer = null;
  }
}
