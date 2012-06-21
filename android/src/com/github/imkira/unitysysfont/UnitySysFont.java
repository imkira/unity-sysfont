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

import android.app.Activity;
import com.unity3d.player.UnityPlayer;
import java.util.Map;
import java.util.HashMap;
import java.util.Iterator;

public class UnitySysFont {
  private static UnitySysFont instance = null;
  private static final int UPDATE_QUEUE_CAPACITY = 32;

  private Map<Integer, TextureUpdate> updateQueue;

  public UnitySysFont() {
    updateQueue = new HashMap<Integer, TextureUpdate>(UPDATE_QUEUE_CAPACITY);
  }

  public static synchronized UnitySysFont getInstance() {
    if (instance == null) {
      instance = new UnitySysFont();
    }
    return instance;
  }

  public void queueTexture(String text, String fontName, int fontSize,
      boolean isBold, boolean isItalic, int maxWidthPixels,
      int maxHeightPixels, int textureID) {

    TextureUpdate update = new TextureUpdate(text, fontName, fontSize, isBold,
        isItalic, maxWidthPixels, maxHeightPixels, textureID);
    synchronized (updateQueue) {
      updateQueue.put(textureID, update);
    }
  }

  public void updateQueuedTexture(int textureID) {
    synchronized (updateQueue) {
      TextureUpdate update = updateQueue.get(textureID);
      if (update != null) {
        update.setReady();
      }
    }
  }

  public void dequeueTexture(int textureID) {
    synchronized (updateQueue) {
      updateQueue.remove(textureID);
    }
  }

  public int getTextureWidth(int textureID) {
    synchronized (updateQueue) {
      TextureUpdate update = updateQueue.get(textureID);
      if (update == null) {
        return -1;
      }
      return update.getTextureWidth();
    }
  }

  public int getTextureHeight(int textureID) {
    synchronized (updateQueue) {
      TextureUpdate update = updateQueue.get(textureID);
      if (update == null) {
        return -1;
      }
      return update.getTextureHeight();
    }
  }

  public int getTextWidth(int textureID) {
    synchronized (updateQueue) {
      TextureUpdate update = updateQueue.get(textureID);
      if (update == null) {
        return -1;
      }
      return update.getTextWidth();
    }
  }

  public int getTextHeight(int textureID) {
    synchronized (updateQueue) {
      TextureUpdate update = updateQueue.get(textureID);
      if (update == null) {
        return -1;
      }
      return update.getTextHeight();
    }
  }

  public void processQueue() {
    synchronized (updateQueue) {
      if (updateQueue.isEmpty() == false) {
        Iterator<Map.Entry<Integer, TextureUpdate>> iterator;
        iterator = updateQueue.entrySet().iterator();
        while (iterator.hasNext()) {
          TextureUpdate update = iterator.next().getValue();
          if (update.isReady()) {
            update.render();
            iterator.remove();
          }
        }
      }
    }
  }
}
