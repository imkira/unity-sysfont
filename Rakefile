#!/usr/bin/env ruby
# -*- coding: UTF-8 -*-

require 'fileutils'

task :default => [:build]

bundle_name = 'SysFont.bundle'
macosx_dir = File.expand_path(File.join(File.dirname(__FILE__), 'macosx'))

jar_name = 'SysFont.jar'
android_dir = File.expand_path(File.join(File.dirname(__FILE__), 'android'))
android_classes = "/Applications/Unity/Unity.app/Contents/PlaybackEngines/AndroidPlayer/bin/classes.jar"

namespace :build do
  file "#{macosx_dir}/build/Release/#{bundle_name}" do
    Dir.chdir(macosx_dir) do
      system 'xcodebuild clean build'
    end
  end

  desc 'Build plugin for MacOSX'
  task :macosx => "#{macosx_dir}/build/Release/#{bundle_name}" do
  end

  file "#{android_dir}/bin/#{jar_name}" do
    Dir.chdir(android_dir) do
      system 'android update project -p .'
      system 'mkdir -p libs'
      FileUtils.cp(android_classes, 'libs')
      system 'ant release'
      FileUtils.mv('bin/classes.jar', "bin/#{jar_name}", :verbose => true)
    end
  end

  desc 'Build plugin for Android'
  task :android => "#{android_dir}/bin/#{jar_name}" do
  end

  desc 'Clean any builds'
  task :clean do
    Dir.chdir(macosx_dir) do
      system 'xcodebuild clean'
      FileUtils.rm_rf('build', :verbose => true)
    end
    Dir.chdir(android_dir) do
      system 'android update project -p .'
      system 'ant clean'
      FileUtils.rm_rf('libs', :verbose => true)
    end
  end
end

desc 'Build plugin for MacOSX/Android'
task :build => ['build:macosx', 'build:android']
