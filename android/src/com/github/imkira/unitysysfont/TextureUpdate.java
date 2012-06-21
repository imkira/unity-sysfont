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

package com.github.imkira.unitysysfont;

import android.opengl.GLES10;
import android.opengl.GLUtils;

import android.graphics.Typeface;
import android.graphics.Bitmap;
import android.graphics.Canvas;

import android.text.Layout;
import android.text.StaticLayout;
import android.text.TextPaint;

public class TextureUpdate {
  private String text;
  private String[] textLines;
  private String fontName;
  private int fontSize;
  private boolean isBold;
  private boolean isItalic;
  private int maxWidthPixels;
  private int maxHeightPixels;
  private int textureID;

  private boolean isReady;

  private int textWidth;
  private int textHeight;
  private int textureWidth;
  private int textureHeight;
  private TextPaint paint;
  private StaticLayout layout;
  
  public TextureUpdate(String text, String fontName, int fontSize,
      boolean isBold, boolean isItalic, int maxWidthPixels,
      int maxHeightPixels, int textureID) {
    this.text = text;
    this.fontName = fontName;
    this.fontSize = fontSize;
    this.isBold = isBold;
    this.isItalic = isItalic;
    this.maxWidthPixels = maxWidthPixels;
    this.maxHeightPixels = maxHeightPixels;
    this.textureID = textureID;
    
    isReady = false;
    prepare();
  }

  public int getTextureWidth() {
    return textureWidth;
  }

  public int getTextureHeight() {
    return textureHeight;
  }

  public int getTextWidth() {
    return textWidth;
  }

  public int getTextHeight() {
    return textHeight;
  }

  public boolean isReady() {
    return isReady;
  }

  public void setReady() {
    isReady = true;
  }

  private Typeface getTypeface() {
    Typeface typeface = null;
    int style = Typeface.NORMAL;

    if (isBold == true) {
      style |= Typeface.BOLD;
    }
    if (isItalic == true) {
      style |= Typeface.ITALIC;
    }

    if (fontName.length() > 0) {
      typeface = Typeface.create(fontName, style);
    }
    if (typeface == null) {
      typeface = Typeface.defaultFromStyle(style);
    }
    return typeface;
  }

  private static int getNextPowerOfTwo(int n) {
    --n;
    n |= n >> 1;
    n |= n >> 2;
    n |= n >> 4;
    n |= n >> 8;
    n |= n >> 16;
    ++n;
    return (n <= 0) ? 1 : n;
  }

  private void prepareLayout() {
    float desiredWidth = Layout.getDesiredWidth(text, paint);

    textWidth = (int)Math.ceil(desiredWidth);
    if (textWidth > maxWidthPixels) {
      textWidth = maxWidthPixels;
    }
    else if (textWidth <= 0) {
      textWidth = 1;
    }

    layout = new StaticLayout(text, paint, textWidth,
        Layout.Alignment.ALIGN_NORMAL, 1, 0, false);

    textHeight = (int)Math.ceil(layout.getHeight());
    if (textHeight > maxHeightPixels) {
      textHeight = maxHeightPixels;
    }
    else if (textHeight <= 0) {
      textHeight = 1;
    }
  }

  private void prepare() {
    paint = new TextPaint();

    Typeface typeface = getTypeface();
    if (typeface != null) {
      paint.setTypeface(typeface);
    }
    if (fontSize > 0) {
      paint.setTextSize(fontSize);
    }
    paint.setAntiAlias(true);
    paint.setAlpha(255);

    prepareLayout();

    textureWidth = getNextPowerOfTwo(textWidth);
    textureHeight = getNextPowerOfTwo(textHeight);
  }

  public void render() {
    Bitmap bitmap = Bitmap.createBitmap(textureWidth, textureHeight,
        Bitmap.Config.ALPHA_8);

    Canvas canvas = new Canvas(bitmap);
    canvas.translate(0, textHeight);
    canvas.scale(1, -1);
    layout.draw(canvas);

    GLES10.glBindTexture(GLES10.GL_TEXTURE_2D, textureID);
    GLES10.glPixelStorei(GLES10.GL_UNPACK_ALIGNMENT, 1);
    GLES10.glTexParameterf(GLES10.GL_TEXTURE_2D, GLES10.GL_TEXTURE_MIN_FILTER,
        GLES10.GL_LINEAR);
    GLES10.glTexParameterf(GLES10.GL_TEXTURE_2D, GLES10.GL_TEXTURE_MAG_FILTER,
        GLES10.GL_LINEAR);
    GLES10.glTexParameterf(GLES10.GL_TEXTURE_2D, GLES10.GL_TEXTURE_WRAP_S,
        GLES10.GL_CLAMP_TO_EDGE);
    GLES10.glTexParameterf(GLES10.GL_TEXTURE_2D, GLES10.GL_TEXTURE_WRAP_T,
        GLES10.GL_CLAMP_TO_EDGE);
    GLUtils.texImage2D(GLES10.GL_TEXTURE_2D, 0, bitmap, 0);
    bitmap.recycle();
  }
}
