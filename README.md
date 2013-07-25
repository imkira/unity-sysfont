This page is for Unity 4.x. If you are looking for Unity 3.x please
[check here](https://github.com/imkira/unity-sysfont).

unity-sysfont
=============

unity-sysfont is a [Unity 3D](http://unity3d.com) plugin for displaying
dynamic text using the fonts available on the mobile device/computer.

## Features

* Multi-platform: iOS/Android/MacOSX support.
* Unity Editor support: customize the text within the editor and confirm the
  results immediately.
* Customizable: change the font family, size, style, color, alignment, and more.
* Optimized: Alpha-only textures and dynamic texture size adjustment.
* Programmable: the same fields available in the editor are exposed as
  properties that you can change programatically.
* Third-party: integrates well with NGUI scroll lists and other widgets that
  require clipping.

## Requirements

* Unity 4.x Pro.

## Download & Installation

The following files are for Unity 4.x. If you are looking for Unity 3.x please
[check here](https://github.com/imkira/unity-sysfont).

* [unity-sysfont plugin (v1.0.4)](http://dl.bintray.com/content/imkira/unity-sysfont/unity4x-core/unity-sysfont-1_0_4.unitypackage?direct)
* [NGUI compatibility package (v1.0.5)](http://dl.bintray.com/content/imkira/unity-sysfont/unity4x-ngui/unity-sysfont-ngui-1_0_5.unitypackage?direct)
* [standalone demo (v1.0.1)](http://dl.bintray.com/content/imkira/unity-sysfont/unity4x-demo/unity-sysfont-demo-1_0_1.unitypackage?direct)

## Change Log

The [change log](https://github.com/imkira/unity-sysfont/blob/unity4x/CHANGES.md)
contains the list of changes and latest version information of each package.

## Demonstration

Here is a screenshot of unity-sysfont demo running on Unity Editor.
![Editor](http://dl.bintray.com/content/imkira/unity-sysfont/unity3x-core/screenshots/editor-1_0_2.png?direct)

And here is a picture of the demo (find link above) running on and Android and an iPhone.
![Android and iPhone](http://dl.bintray.com/content/imkira/unity-sysfont/unity3x-core/screenshots/android_iphone-1_0_2.png?direct)

## Creating dynamic text

* Make sure your camera is will configured (normally you will want an
  orthographic camera).
* Create an empty GameObject: ```GameObject -> Create Empty```
* Add a SysFontText component: ```Component -> SysFont -> Text```

## Understanding the Inspector

The following is an explanation for each field of SysFontText component.

### Text

Type in anything you want (multiline text is OK).

### Font Size

The font size used to display the text in all platforms.
If 0, the system's default font size will be chosen during runtime.

### Bold

Check to use bold.

Note 1: On iOS/MacOSX you can only use this if you leave the font name blank or
the font doesn't exist.

Note 2: On Android, you can only use this if the font supports it (if it does
not, it will be ignored).

### Italic

Check to use italic.

Note 1: On iOS/MacOSX you can only use this if you leave the font name blank or
the font doesn't exist.

Note 2: On MacOSX, if both bold and italic are checked, italic will be ignored.

Note 3: On Android, you can only use this if the font supports it (if it does
not, it will be ignored).

### Alignment

Choose the desired text alignment: left, center, or right.

### iOS/MacOSX/Editor Font

The font name to be used on iOS/MacOSX.
Leave this field blank to use the system's default font.

Note: In order to safely choose a font name that works equally well for both
iOS and MacOSX/Editor, type in the font's "PostScript Name". You can, confirm
this by opening ```/Applications/Font Book```, selecting the desired font,
and then finally choosing ```Preview -> Show Font Info``` for displaying
the "PostScript Name" of the font.

### Android Font

The font name to be used on Android (eg. Droid Sans, Droid Sans Fallback, etc.).
Leave this field blank to use the system's default font.

### Multi Line

Check this field to enable multiline text. If you uncheck this and your text
contains newlines, the whole text will be displayed as a single line.

### Max Width

unity-sysfont generates a texture for each dynamic text component you use.
If the text you want to show is wider in pixels than this value,
the text will be wrapped.

Note: the default value is 2048 pixels. You don't need to worry about
choosing a smaller value, because unity-sysfont will do that automatically
for you in runtime: the size of the generated textures will always be
the minimum size necessary to contain your text, except if it is bigger than
this value.

### Max Height

unity-sysfont generates a texture for each dynamic text component you use.
If the text you want to show is taller in pixels than this value,
the text will be cropped.

Note: the default value is 2048 pixels. You don't need to worry about
choosing a smaller value, because unity-sysfont will do that automatically
for you in runtime: the size of the generated textures will always be
the minimum size necessary to contain your text, except if it is bigger than
this value.

### Font Color

Choose the color you want for your text.

### Pivot

Choose the pivot point for the mesh that is textured with your dynamic text.

## Pixel Perfect Text

For displaying pixel perfect text, you can freely use and modify the script
[PixelPerfectResizer.cs](http://github.com/imkira/unity-sysfont/blob/master/unity/Assets/SysFont/Demo/PixelPerfectResizer.cs)
and use it like in the demo (find link above).

## NGUI Compatibility

If you wish to use unity-sysfont text with clipping support, you can download
the NGUI compatibility package (find link above).

After downloading the package, import it to your project.
To add a UISysFontLabel widget to your scene access the menu
```NGUI -> Create a SysFont Label```.

Here is a screenshot of unity-sysfont-ngui.
![NGUI](http://dl.bintray.com/content/imkira/unity-sysfont/unity3x-core/screenshots/unity-sysfont_ngui-clipping-1_0_2.jpg?direct)

## Known Problems

On the Unity Editor, sometimes the contents of the textures that are used for
displaying the dynamic text get "wiped out". Replaying the scene will most
times fix this glitch.
This problem does not happen on iOS/Android, so I believe it may be related
to the multi-threaded rendering feature that is used on Unity.
I am investigating this problem, and I hope to fix it ASAP.

## Limitations

* unity-sysfont is not currently supported on Windows (I have plans to support
  it in the near future). Keep in touch!
* Due to the limitations on the webplayer platform, unity-sysfont may never be
  possible on such platform.

## Contribute

* Found a bug?
* Want to contribute and add a new feature?

Please fork this project and send me a pull request!

## License

unity-sysfont is licensed under the MIT license:

www.opensource.org/licenses/MIT

## Copyright

Copyright (c) 2012 Mario Freitas. See
[LICENSE.txt](http://github.com/imkira/unity-sysfont/blob/master/LICENSE.txt)
for further details.
