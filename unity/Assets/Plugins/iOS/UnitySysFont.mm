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

#import <Foundation/Foundation.h>

#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
#import <CoreGraphics/CoreGraphics.h>
#import <OpenGLES/EAGL.h>
#import <OpenGLES/ES1/gl.h>

extern EAGLContext* _context;

#elif TARGET_OS_MAC
#import <ApplicationServices/ApplicationServices.h>
#include <OpenGL/gl.h>
#else
#error Unknown platform
#endif

#define UNITY_SYSFONT_UPDATE_QUEUE_CAPACITY 32

int nextPowerOfTwo(int n);

int nextPowerOfTwo(int n)
{
  --n;
  n |= n >> 1;
  n |= n >> 2;
  n |= n >> 4;
  n |= n >> 8;
  n |= n >> 16;
  ++n;
  return (n <= 0) ? 1 : n;
}

@interface UnitySysFontTextureUpdate : NSObject
{
  NSString *text;
  NSString *fontName;
  int fontSize;
  BOOL isBold;
  BOOL isItalic;
  int alignment;
  int maxWidthPixels;
  int maxHeightPixels;
  int textureID;

  BOOL ready;

#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
#elif TARGET_OS_MAC
  NSAttributedString *attributedString;
#endif

  @public int textWidth;
  @public int textHeight;
  @public int textureWidth;
  @public int textureHeight;
}

- (NSNumber *)textureID;
#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
- (UIFont *)font;
#elif TARGET_OS_MAC
- (NSFont *)font;
#endif
- (void)prepare;
- (void)render;
- (void)bindTextureWithFormat:(GLenum)format bitmapData:(void *)data;

@property (nonatomic, assign, getter=isReady) BOOL ready;
@end

@implementation UnitySysFontTextureUpdate

@synthesize ready;

- (id)initWithText:(const char *)_text fontName:(const char *)_fontName
fontSize:(int)_fontSize isBold:(BOOL)_isBold isItalic:(BOOL)_isItalic
alignment:(int)_alignment maxWidthPixels:(int)_maxWidthPixels
maxHeightPixels:(int)_maxHeightPixels textureID:(int)_textureID
{
  self = [super init];

  if (self != nil)
  {
    text = [[NSString stringWithUTF8String:((_text == NULL) ? "" : _text)]
      retain];
    fontName = [[NSString stringWithUTF8String:((_fontName == NULL) ? "" :
        _fontName)] retain];
    fontSize = _fontSize;
    isBold = _isBold;
    isItalic = _isItalic;
    alignment = _alignment;
    maxWidthPixels = _maxWidthPixels;
    maxHeightPixels = _maxHeightPixels;
    textureID = _textureID;
    ready = NO;
    [self prepare];
  }

  return self;
}

- (void)dealloc
{
#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
#elif TARGET_OS_MAC
  [attributedString release];
#endif
  [text release];
  [fontName release];
  [super dealloc];
}

- (NSNumber *)textureID
{
  return [NSNumber numberWithInt:textureID];
}

#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
- (UIFont *)font
{
  UIFont *font = nil;

  if (fontSize <= 0)
  {
    fontSize = (int)[UIFont systemFontSize];
  }

  if ([fontName length] > (NSUInteger)0)
  {
    font = [UIFont fontWithName:fontName size:fontSize];
  }
  
  if (font == nil)
  {
    if (isBold == YES)
    {
      font = [UIFont boldSystemFontOfSize:fontSize];
    }
    else if (isItalic == YES)
    {
      font = [UIFont italicSystemFontOfSize:fontSize];
    }
    else
    {
      font = [UIFont systemFontOfSize:fontSize];
    }
  }
  return font;
}
#elif TARGET_OS_MAC
- (NSFont *)font
{
  NSFont *font = nil;

  if (fontSize <= 0)
  {
    fontSize = (int)[NSFont systemFontSize];
  }

  if ([fontName length] > (NSUInteger)0)
  {
    font = [NSFont fontWithName:fontName size:fontSize];
  }

  if (font == nil)
  {
    if (isBold == YES)
    {
      font = [NSFont boldSystemFontOfSize:fontSize];
    }
    else
    {
      font = [NSFont systemFontOfSize:fontSize];
    }
  }

  return font;
}
#endif

- (void)prepare
{
  CGSize maxSize = CGSizeMake(maxWidthPixels, maxHeightPixels);
  CGSize boundsSize;

#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
  boundsSize = [text sizeWithFont:[self font] constrainedToSize:maxSize
    lineBreakMode:UILineBreakModeWordWrap];
#elif TARGET_OS_MAC

  NSTextAlignment _alignment = NSLeftTextAlignment;

  if (alignment == 1)
  {
    _alignment = NSCenterTextAlignment;
  }
  else if (alignment == 2)
  {
    _alignment = NSRightTextAlignment;
  }

  NSMutableParagraphStyle *parStyle = [[NSMutableParagraphStyle alloc] init];
  [parStyle setAlignment:_alignment];
  [parStyle setLineBreakMode:NSLineBreakByWordWrapping];

  NSDictionary *attributes = [NSDictionary dictionaryWithObjectsAndKeys:
    [self font], NSFontAttributeName,
    [NSColor whiteColor], NSForegroundColorAttributeName,
    [NSColor clearColor], NSBackgroundColorAttributeName,
    parStyle, NSParagraphStyleAttributeName, nil];

  attributedString = [[NSAttributedString alloc] 
    initWithString:text attributes:attributes];

  boundsSize = NSSizeToCGSize([attributedString
      boundingRectWithSize:NSSizeFromCGSize(maxSize)
      options:NSStringDrawingUsesLineFragmentOrigin].size);
#endif

  textWidth = (int)ceilf(boundsSize.width);
  if (textWidth > maxWidthPixels)
  {
    textWidth = maxWidthPixels;
  }
  else if (textWidth <= 0)
  {
    textWidth = 1;
  }
  textHeight = (int)ceilf(boundsSize.height);
  if (textHeight > maxHeightPixels)
  {
    textHeight = maxHeightPixels;
  }
  else if (textHeight <= 0)
  {
    textHeight = 1;
  }

  textureWidth = nextPowerOfTwo(textWidth);
  textureHeight = nextPowerOfTwo(textHeight);
}

#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
- (void)render
{
  void *bitmapData = calloc(textureHeight, textureWidth);
  CGContextRef context = CGBitmapContextCreate(bitmapData, textureWidth,
      textureHeight, 8, textureWidth, NULL, kCGImageAlphaOnly);

  if (context == NULL)
  {
    free(bitmapData);
    return;
  }

  UIGraphicsPushContext(context);

  CGContextSetAlpha(context, 1.f);

  CGRect drawRect = CGRectMake(0.f, (float)(textureHeight - textHeight),
      textWidth, textHeight);

  UITextAlignment _alignment = UITextAlignmentLeft;

  if (alignment == 1)
  {
    _alignment = UITextAlignmentCenter;
  }
  else if (alignment == 2)
  {
    _alignment = UITextAlignmentRight;
  }

  [text drawInRect:drawRect withFont:[self font]
    lineBreakMode:UILineBreakModeWordWrap alignment:_alignment];

  UIGraphicsPopContext();

  [self bindTextureWithFormat:GL_ALPHA bitmapData:bitmapData];

  CGContextRelease(context); 
  free(bitmapData);
}
#elif TARGET_OS_MAC
- (void)render
{
  NSBitmapImageRep *bitmap = [[NSBitmapImageRep alloc]
    initWithBitmapDataPlanes:NULL pixelsWide:textureWidth
    pixelsHigh:textureHeight bitsPerSample:8 samplesPerPixel:1 hasAlpha:NO
    isPlanar:NO colorSpaceName:NSCalibratedWhiteColorSpace bitmapFormat:0
    bytesPerRow:textureWidth bitsPerPixel:8];

  if (bitmap == nil)
  {
    return;
  }

  NSGraphicsContext *context = [NSGraphicsContext
    graphicsContextWithBitmapImageRep:bitmap];
  [NSGraphicsContext saveGraphicsState];
  [NSGraphicsContext setCurrentContext:context];

  NSAffineTransform *transform = [NSAffineTransform transform];
  [transform translateXBy:0.f yBy:textureHeight];
  [transform scaleXBy:1.f yBy:-1.f];
  [transform concat];

  NSRect textureRect = NSMakeRect(0.f, 0.f, textureWidth, textureHeight);
  NSRect drawRect = NSMakeRect(0.f, 0.f, textWidth, textHeight);

  [[NSColor clearColor] setFill];
  NSRectFill(textureRect);

  [[NSColor whiteColor] set];
  [attributedString drawWithRect:drawRect
    options:NSStringDrawingUsesLineFragmentOrigin];

  [NSGraphicsContext restoreGraphicsState];

  [self bindTextureWithFormat:GL_ALPHA bitmapData:[bitmap bitmapData]];

  [bitmap release];
}
#endif

- (void)bindTextureWithFormat:(GLenum)format bitmapData:(void *)data
{
  glBindTexture(GL_TEXTURE_2D, textureID);
  glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
  glTexImage2D(GL_TEXTURE_2D, 0, GL_ALPHA, textureWidth, textureHeight, 0,
      format, GL_UNSIGNED_BYTE, data);
}
@end

@interface UnitySysFontTextureManager : NSObject
{
  NSMutableDictionary *updateQueue;
}

+ (UnitySysFontTextureManager *)sharedInstance;
- (id)initWithCapacity:(NSUInteger)numItems;
- (UnitySysFontTextureUpdate *)updateHavingTextureID:(int)textureID;
- (void)queueUpdate:(UnitySysFontTextureUpdate *)update;
- (void)dequeueUpdate:(NSNumber *)textureID;
@end

@implementation UnitySysFontTextureManager 
static UnitySysFontTextureManager *sharedInstance;

+ (void)initialize
{
  static BOOL initialized = NO;

  if (!initialized)
  {
    initialized = YES;
    sharedInstance = [[UnitySysFontTextureManager alloc]
      initWithCapacity:UNITY_SYSFONT_UPDATE_QUEUE_CAPACITY];
  }
}

+ (UnitySysFontTextureManager *)sharedInstance
{
  return sharedInstance;
}

- (id)initWithCapacity:(NSUInteger)numItems
{
  self = [super init];

  if (self != nil)
  {
    updateQueue = [[NSMutableDictionary alloc] initWithCapacity:numItems];
  }

  return self;
}

- (void)dealloc
{
  [updateQueue release];
  [super dealloc];
}

- (UnitySysFontTextureUpdate *)updateHavingTextureID:(int)textureID
{
  return [updateQueue objectForKey:[NSNumber numberWithInt:textureID]];
}

- (void)queueUpdate:(UnitySysFontTextureUpdate *)update
{
  NSNumber *textureID = [update textureID];
  [self dequeueUpdate:textureID];
  [updateQueue setObject:update forKey:textureID];
}

- (void)dequeueUpdate:(NSNumber *)textureID
{
  UnitySysFontTextureUpdate *existingUpdate;
  existingUpdate = [updateQueue objectForKey:textureID];
  if (existingUpdate != nil)
  {
    [updateQueue removeObjectForKey:textureID];
    [existingUpdate release];
  }
}

- (void)processQueue
{
  if ([updateQueue count] > (NSUInteger)0)
  {
#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
    EAGLContext *oldContext = [EAGLContext currentContext];
    // change to Unity's default OpenGL context
    if (oldContext != _context)
    {
      [EAGLContext setCurrentContext:_context];
    }
#endif
    for (NSNumber *textureID in [updateQueue allKeys])
    {
      UnitySysFontTextureUpdate *update = [updateQueue objectForKey:textureID];
      if ([update isReady])
      {
        [update render];
        [updateQueue removeObjectForKey:textureID];
        [update release];
      }
    }
#if TARGET_OS_IPHONE || TARGET_IPHONE_SIMULATOR
    // revert to non-default OpenGL context?
    if (oldContext != _context)
    {
      [EAGLContext setCurrentContext:oldContext];
    }
#endif
  }
}
@end

extern "C"
{
  void _SysFontQueueTexture(const char *text, const char *fontName,
      int fontSize, BOOL isBold, BOOL isItalic, int alignment,
      int maxWidthPixels, int maxHeightPixels, int textureID);

  int _SysFontGetTextureWidth(int textureID);

  int _SysFontGetTextureHeight(int textureID);

  int _SysFontGetTextWidth(int textureID);

  int _SysFontGetTextHeight(int textureID);

  void _SysFontUpdateQueuedTexture(int textureID);

  void _SysFontRender();

  void _SysFontDequeueTexture(int textureID);

  void UnityRenderEvent(int eventID);
}

void _SysFontQueueTexture(const char *text, const char *fontName,
    int fontSize, BOOL isBold, BOOL isItalic, int alignment,
    int maxWidthPixels, int maxHeightPixels, int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;

  update = [[UnitySysFontTextureUpdate alloc] initWithText:text
    fontName:fontName fontSize:fontSize isBold:isBold isItalic:isItalic
    alignment:alignment maxWidthPixels:maxWidthPixels
    maxHeightPixels:maxHeightPixels textureID:textureID];

  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    [instance queueUpdate:update];
  }
}

int _SysFontGetTextureWidth(int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    update = [instance updateHavingTextureID:textureID];
    if (update == nil)
    {
      return -1;
    }
    return update->textureWidth;
  }
}

int _SysFontGetTextureHeight(int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    update = [instance updateHavingTextureID:textureID];
    if (update == nil)
    {
      return -1;
    }
    return update->textureHeight;
  }
}

int _SysFontGetTextWidth(int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    update = [instance updateHavingTextureID:textureID];
    if (update == nil)
    {
      return -1;
    }
    return update->textWidth;
  }
}

int _SysFontGetTextHeight(int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    update = [instance updateHavingTextureID:textureID];
    if (update == nil)
    {
      return -1;
    }
    return update->textHeight;
  }
}

void _SysFontUpdateQueuedTexture(int textureID)
{
  UnitySysFontTextureManager *instance;
  UnitySysFontTextureUpdate *update;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    update = [instance updateHavingTextureID:textureID];
    if (update != nil)
    {
      [update setReady:YES];
    }
  }
}

void _SysFontRender()
{
  UnitySysFontTextureManager *instance;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    [instance processQueue];
  }
}

void _SysFontDequeueTexture(int textureID)
{
  UnitySysFontTextureManager *instance;
  instance = [UnitySysFontTextureManager sharedInstance];
  @synchronized(instance)
  {
    [instance dequeueUpdate:[NSNumber numberWithInt:textureID]];
  }
}

void UnityRenderEvent(int eventID)
{
  NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
  _SysFontRender();
  [pool drain];
}
