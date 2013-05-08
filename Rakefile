#!/usr/bin/env ruby
# -*- coding: UTF-8 -*-

task :default => [:package]

# main config
unity = File.expand_path(ENV['UNITY'] || '/Applications/Unity/Unity.app')
cur_dir = File.expand_path(File.dirname(__FILE__))
proj_path = File.join(cur_dir, 'unity')

# macosx config
macosx_dir = File.join(cur_dir, 'macosx')
macosx_bundle = File.join(macosx_dir, 'build/Release/SysFont.bundle')

# android config
android_jar_name = 'SysFont.jar'
android_dir = File.join(cur_dir, 'android')
android_classes = File.join(unity, 'Contents/PlaybackEngines/AndroidPlayer/bin/classes.jar')
android_jar = File.join(android_dir, "bin/#{android_jar_name}")

# package config
package_dir = File.join(cur_dir, 'packages')
package_core_dir = File.join(package_dir, 'core')
package_core = File.join(package_dir, 'unity-sysfont.unitypackage')
package_demo = File.join(package_dir, 'unity-sysfont-demo.unitypackage')
package_ngui_dir = File.join(package_dir, 'ngui')
package_ngui = File.join(package_dir, 'unity-sysfont-ngui.unitypackage')

namespace :build do
  file macosx_bundle do
    Dir.chdir(macosx_dir) do
      sh 'xcodebuild clean build'
    end
  end

  desc 'Build plugin for MacOSX'
  task :macosx => [macosx_bundle] do
  end

  directory File.join(android_dir, 'libs')

  file android_jar => File.join(android_dir, 'libs') do
    Dir.chdir(android_dir) do
      sh 'android update project -p .'
      cp_r(android_classes, 'libs')
      sh 'ant release'
      mv('bin/classes.jar', "bin/#{android_jar_name}")
    end
  end

  desc 'Build plugin for Android'
  task :android => android_jar do
  end

  desc 'Clean any builds'
  task :clean do
    Dir.chdir(macosx_dir) do
      sh 'xcodebuild clean'
      rm_rf 'build'
    end
    Dir.chdir(android_dir) do
      sh 'android update project -p .'
      sh 'ant clean'
      rm_rf 'libs'
    end
  end
end

namespace :package do
  def unity_batch(unity, proj_path, method)
    unity_bin = File.join(unity, 'Contents', 'MacOS', 'Unity')
    sh %Q["#{unity_bin}" -projectPath "#{proj_path}" -batchmode -quit -executeMethod "#{method}"]
  end

  directory package_core_dir => package_dir do
    cp_r(proj_path, package_core_dir)
    mkdir_p(File.join(package_core_dir, 'Assets/Plugins/Android'))
  end

  file package_core => [package_core_dir, macosx_bundle, android_jar] do
    cp_r(macosx_bundle, File.join(package_core_dir, 'Assets/Plugins'))
    cp_r(android_jar, File.join(package_core_dir, 'Assets/Plugins/Android'))
    unity_batch(unity, package_core_dir, 'PluginBuilder.PackageCore')
    mv(File.join(package_core_dir, File.basename(package_core)), package_core)
  end

  desc 'Create plugin package'
  task :core => package_core do
  end

  file package_demo => [package_core_dir, macosx_bundle, android_jar] do
    cp_r(macosx_bundle, File.join(package_core_dir, 'Assets/Plugins'))
    cp_r(android_jar, File.join(package_core_dir, 'Assets/Plugins/Android'))
    unity_batch(unity, package_core_dir, 'PluginBuilder.PackageDemo')
    mv(File.join(package_core_dir, File.basename(package_demo)), package_demo)
  end

  desc 'Create demo package'
  task :demo => package_demo do
  end

  directory package_ngui_dir => package_dir do
    cp_r(proj_path, package_ngui_dir)
  end

  file package_ngui => [package_ngui_dir, macosx_bundle, android_jar] do
    cp_r(macosx_bundle, File.join(package_ngui_dir, 'Assets/Plugins'))
    cp_r(android_jar, File.join(package_ngui_dir, 'Assets/Plugins/Android'))
    Dir.chdir(package_ngui_dir) do
      mv('Compatibility', 'Assets/SysFont')
    end
    unity_batch(unity, package_ngui_dir, 'PluginBuilder.PackageCompatNGUI')
    mv(File.join(package_ngui_dir, File.basename(package_ngui)), package_ngui)
  end

  desc 'Create NGUI compatibility'
  task :ngui => package_ngui do
  end

  desc 'Clean any packages'
  task :clean do
    rm_rf package_dir
  end
end

desc 'Export plugin and compatibility packages'
task :package => ['package:core', 'package:demo', 'package:ngui']

desc 'Build plugin for MacOSX/Android'
task :build => ['build:macosx', 'build:android']
