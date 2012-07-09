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
using System;

[System.Serializable]
public class SysFontTexture : ISysFontTexturable
{
  [SerializeField]
  protected string _text = "";

  [SerializeField]
  protected string _appleFontName = "";

  [SerializeField]
  protected string _androidFontName = "";

  [SerializeField]
  protected int _fontSize = 0;

  [SerializeField]
  protected bool _isBold = false;

  [SerializeField]
  protected bool _isItalic = false;

  [SerializeField]
  protected SysFont.Alignment _alignment = SysFont.Alignment.Left;

  [SerializeField]
  protected bool _isMultiLine = true;

  [SerializeField]
  protected int _maxWidthPixels = 2048;

  [SerializeField]
  protected int _maxHeightPixels = 2048;

  protected string _lastText;
  public string Text
  {
    get
    {
      return _text;
    }
    set
    {
      if (_text != value)
      {
        _text = value;
      }
    }
  }

  public string AppleFontName
  {
    get
    {
      return _appleFontName;
    }
    set
    {
      if (_appleFontName != value)
      {
        _appleFontName = value;
      }
    }
  }

  public string AndroidFontName 
  {
    get
    {
      return _androidFontName;
    }
    set
    {
      if (_androidFontName != value)
      {
        _androidFontName = value;
      }
    }
  }

  protected string _lastFontName;
  public string FontName
  {
    get
    {
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_IPHONE
      return AppleFontName;
#elif UNITY_ANDROID
      return AndroidFontName;
#else
      // just don't fail the build
      return AppleFontName;
#endif
    }
    set
    {
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_IPHONE
      AppleFontName = value;
#elif UNITY_ANDROID
      AndroidFontName = value;
#else
      // just don't fail the build
      AppleFontName = value;
#endif
    }
  }

  protected int _lastFontSize;
  public int FontSize
  {
    get
    {
      return _fontSize;
    }
    set
    {
      if (_fontSize != value)
      {
        _fontSize = value;
      }
    }
  }

  protected bool _lastIsBold;
  public bool IsBold
  {
    get
    {
      return _isBold;
    }
    set
    {
      if (_isBold != value)
      {
        _isBold = value;
      }
    }
  }

  protected bool _lastIsItalic;
  public bool IsItalic
  {
    get
    {
      return _isItalic;
    }
    set
    {
      if (_isItalic != value)
      {
        _isItalic = value;
      }
    }
  }

  protected SysFont.Alignment _lastAlignment;
  public SysFont.Alignment Alignment
  {
    get
    {
      return _alignment;
    }
    set
    {
      if (_alignment != value)
      {
        _alignment = value;
      }
    }
  }

  protected bool _lastIsMultiLine;
  public bool IsMultiLine
  {
    get
    {
      return _isMultiLine;
    }
    set
    {
      if (_isMultiLine != value)
      {
        _isMultiLine = value;
      }
    }
  }

  protected int _lastMaxWidthPixels;
  public int MaxWidthPixels
  {
    get
    {
      return _maxWidthPixels;
    }
    set
    {
      if (_maxWidthPixels != value)
      {
        _maxWidthPixels = value;
      }
    }
  }

  protected int _lastMaxHeightPixels;
  public int MaxHeightPixels
  {
    get
    {
      return _maxHeightPixels;
    }
    set
    {
      if (_maxHeightPixels != value)
      {
        _maxHeightPixels = value;
      }
    }
  }

  protected int _widthPixels = 1;
  public int WidthPixels 
  {
    get
    {
      return _widthPixels;
    }
  }

  protected int _heightPixels = 1;
  public int HeightPixels 
  {
    get
    {
      return _heightPixels;
    }
  }

  protected int _textWidthPixels;
  public int TextWidthPixels 
  {
    get
    {
      return _textWidthPixels;
    }
  }

  protected int _textHeightPixels;
  public int TextHeightPixels 
  {
    get
    {
      return _textHeightPixels;
    }
  }

  protected Texture _texture = null;
  public Texture Texture
  {
    get
    {
      return _texture;
    }
  }

  public bool NeedsRedraw
  {
    get
    {
      return (_text != _lastText) ||
        (FontName != _lastFontName) ||
        (_fontSize != _lastFontSize) ||
        (_isBold != _lastIsBold) ||
        (_isItalic != _lastIsItalic) ||
        (_alignment != _lastAlignment) ||
        (_isMultiLine != _lastIsMultiLine) ||
        (_maxWidthPixels != _lastMaxWidthPixels) ||
        (_maxHeightPixels != _lastMaxHeightPixels);
    }
  }

  public void Update()
  {
    if (_texture == null)
    {
      _texture = new Texture2D(1, 1, TextureFormat.Alpha8, false);
      _texture.hideFlags = HideFlags.HideInInspector | HideFlags.DontSave;
      _texture.filterMode = FilterMode.Point;
      _texture.wrapMode = TextureWrapMode.Clamp;
      //Debug.Log("Texture2D creation: " + _texture.GetNativeTextureID());
    }

    int textureID = _texture.GetNativeTextureID();

    SysFont.QueueTexture(_text, FontName, _fontSize, _isBold,
        _isItalic, _alignment, _isMultiLine, _maxWidthPixels,
        _maxHeightPixels, textureID);

    _textWidthPixels = SysFont.GetTextWidth(textureID);
    _textHeightPixels = SysFont.GetTextHeight(textureID);
    _widthPixels = SysFont.GetTextureWidth(textureID);
    _heightPixels = SysFont.GetTextureHeight(textureID);

    SysFont.UpdateQueuedTexture(textureID);

    _lastText = _text;
    _lastFontName = FontName;
    _lastFontSize = _fontSize;
    _lastIsBold = _isBold;
    _lastIsItalic = _isItalic;
    _lastAlignment = _alignment;
    _lastIsMultiLine = _isMultiLine;
    _lastMaxWidthPixels = _maxWidthPixels;
    _lastMaxHeightPixels = _maxHeightPixels;
  }

  public void Destroy()
  {
    if (_texture != null)
    {
      //Debug.Log("Texture2D destruction: " + _texture.GetNativeTextureID());
      if (_texture != null)
      {
        SysFont.DequeueTexture(_texture.GetNativeTextureID());
        SysFont.SafeDestroy(_texture);
        _texture = null;
      }
    }
  }
}
