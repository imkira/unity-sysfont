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
using System.Runtime.InteropServices;

public class SysFont : MonoBehaviour
{
  public enum Alignment 
  {
    Left = 0,
    Center = 1,
    Right = 2
  }

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_IPHONE

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern void _SysFontQueueTexture(string text,
      string fontName, int fontSize, bool isBold, bool isItalic,
      Alignment alignment, int maxWidthPixels, int maxHeightPixels,
      int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern void _SysFontUpdateQueuedTexture(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern void _SysFontDequeueTexture(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern int _SysFontGetTextureWidth(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern int _SysFontGetTextureHeight(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern int _SysFontGetTextWidth(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
  [DllImport("SysFont")]
#else
  [DllImport("__Internal")]
#endif
  private static extern int _SysFontGetTextHeight(int textureID);

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
#else
  [DllImport("__Internal")]
  private static extern void _SysFontRender();
#endif

#elif UNITY_ANDROID

  private static AndroidJavaObject _unitySysFontInstance = null;
  private static AndroidJavaObject UnitySysFontInstance
  {
    get
    {
      if (_unitySysFontInstance == null)
      {
        AndroidJavaClass unitySysFontClass;

        unitySysFontClass =
          new AndroidJavaClass("com.github.imkira.unitysysfont.UnitySysFont");
        _unitySysFontInstance =
          unitySysFontClass.CallStatic<AndroidJavaObject>("getInstance");
      }
      return _unitySysFontInstance;
    }
  }

  private static void _SysFontQueueTexture(string text,
      string fontName, int fontSize, bool isBold, bool isItalic,
      Alignment alignment, int maxWidthPixels, int maxHeightPixels,
      int textureID)
  {
    UnitySysFontInstance.Call("queueTexture", text, fontName, fontSize,
        isBold, isItalic, (int)alignment, maxWidthPixels, maxHeightPixels,
        textureID);
  }

  private static void _SysFontUpdateQueuedTexture(int textureID)
  {
    UnitySysFontInstance.Call("updateQueuedTexture", textureID);
  }

  private static void _SysFontDequeueTexture(int textureID)
  {
    UnitySysFontInstance.Call("dequeueTexture", textureID);
  }

  private static int _SysFontGetTextureWidth(int textureID)
  {
    return UnitySysFontInstance.Call<int>("getTextureWidth", textureID);
  }

  private static int _SysFontGetTextureHeight(int textureID)
  {
    return UnitySysFontInstance.Call<int>("getTextureHeight", textureID);
  }

  private static int _SysFontGetTextWidth(int textureID)
  {
    return UnitySysFontInstance.Call<int>("getTextWidth", textureID);
  }

  private static int _SysFontGetTextHeight(int textureID)
  {
    return UnitySysFontInstance.Call<int>("getTextHeight", textureID);
  }

  private static void _SysFontRender()
  {
    UnitySysFontInstance.Call("processQueue");
  }

#else

  private static void _SysFontQueueTexture(string text,
      string fontName, int fontSize, bool isBold, bool isItalic,
      Alignment alignment, int maxWidthPixels, int maxHeightPixels,
      int textureID)
  {
    // dummy function: just don't fail the build
  }

  private static void _SysFontUpdateQueuedTexture(int textureID)
  {
    // dummy function: just don't fail the build
  }

  private static void _SysFontDequeueTexture(int textureID)
  {
    // dummy function: just don't fail the build
  }

  private static int _SysFontGetTextureWidth(int textureID)
  {
    // dummy function: just don't fail the build
    return 0;
  }

  private static int _SysFontGetTextureHeight(int textureID)
  {
    // dummy function: just don't fail the build
    return 0;
  }

  private static int _SysFontGetTextWidth(int textureID)
  {
    // dummy function: just don't fail the build
    return 0;
  }

  private static int _SysFontGetTextHeight(int textureID)
  {
    // dummy function: just don't fail the build
    return 0;
  }

  private static void _SysFontRender()
  {
    // dummy function: just don't fail the build
  }

#endif

  public static int GetTextureWidth(int textureID)
  {
    return Mathf.Max(_SysFontGetTextureWidth(textureID), 1);
  }

  public static int GetTextureHeight(int textureID)
  {
    return Mathf.Max(_SysFontGetTextureHeight(textureID), 1);
  }

  public static int GetTextWidth(int textureID)
  {
    return Mathf.Max(_SysFontGetTextWidth(textureID), 1);
  }

  public static int GetTextHeight(int textureID)
  {
    return Mathf.Max(_SysFontGetTextHeight(textureID), 1);
  }

  public static void QueueTexture(string text, string fontName,
      int fontSize, bool isBold, bool isItalic, Alignment alignment,
      bool isMultiLine, int maxWidthPixels, int maxHeightPixels, int textureID)
  {
    if (isMultiLine == false)
    {
      text = text.Replace("\r\n", "").Replace("\n", "");
    }
    _SysFontQueueTexture(text, fontName, fontSize, isBold, isItalic,
        alignment, maxWidthPixels, maxHeightPixels, textureID);
  }

  public static void UpdateQueuedTexture(int textureID)
  {
    _SysFontUpdateQueuedTexture(textureID);
#if UNITY_EDITOR || UNITY_STANDALONE_OSX
    GL.IssuePluginEvent(0);
#else
    _SysFontRender();
#endif
  }

  public static void DequeueTexture(int textureID)
  {
    _SysFontDequeueTexture(textureID);
  }

  public static void SafeDestroy(UnityEngine.Object obj)
  {
    if (obj != null)
    {
      if (Application.isEditor)
      {
        DestroyImmediate(obj);
      }
      else
      {
        Destroy(obj);
      }
    }
  }
}
