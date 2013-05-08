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

using UnityEditor;
using UnityEngine;

public static class PluginBuilder
{
  public static void PackageCore()
  {
    string[] assetPaths = new string[]
    {
      "Assets/Plugins/iOS",
      "Assets/Plugins/SysFont",
      "Assets/Plugins/SysFont.bundle",
      "Assets/Plugins/Android",
      "Assets/SysFont/Editor",
      "Assets/SysFont/Resources"
    };

    string packagePath = "unity-sysfont.unitypackage";
    ExportPackageOptions options = ExportPackageOptions.Recurse;
    AssetDatabase.ExportPackage(assetPaths, packagePath, options);
  }

  public static void PackageDemo()
  {
    string[] assetPaths = new string[]
    {
      "Assets/SysFont/Demo"
    };

    string packagePath = "unity-sysfont-demo.unitypackage";
    ExportPackageOptions options = ExportPackageOptions.Recurse;
    AssetDatabase.ExportPackage(assetPaths, packagePath, options);
  }

  public static void PackageCompatNGUI()
  {
    string[] assetPaths = new string[]
    {
      "Assets/SysFont/Compatibility"
    };

    string packagePath = "unity-sysfont-ngui.unitypackage";
    ExportPackageOptions options = ExportPackageOptions.Recurse;
    AssetDatabase.ExportPackage(assetPaths, packagePath, options);
  }
}
