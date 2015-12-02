# An unreal plugin

This is an example of linking 3rd party external libraries to the unreal engine.

It is based off the 'blank' plugin in the UE4 source.

## TLDR

This: https://github.com/shadowmint/ue4-static-plugin/blob/master/Source/TestPlugin/TestPlugin.Build.cs

Notice you must have at least 1 .cpp file in your plugin or the UBT will not compile it.

NB. That a project using this plugin should explicitly mark it as a dependency, as per https://github.com/shadowmint/ue4-sample-project/blob/master/Source/HelloWorld/HelloWorld.Build.cs

## Getting started

- Initialize submodules
- Build 3rdparty/libnpp/ in the build/ folder using cmake (see below)
- Build 3rdparty/rust-extern using cargo as normal.

### Cmake

    mkdir build
    cd build
    cmake ..
    make

Use VS, MSYS obviously wont work when you link.

### Rust

If you don't want to use the rust library, remove it from TestPlugin.Build.cs

Otherwise, use `cargo build` in the rust-extern folder.

Obviously you have to initialize the submodule first.

Notice that the TestPlugin.Build.cs looks in the target/debug folder for
compiled libraries, not the target/release. Change it if you're building in
release mode.

## Notes

### Having trouble with editor crashes?

Try the command line on for size. Here are some helpers scripts:

- https://github.com/shadowmint/ue4-bash-scripts

NB. These are Mac specific; but you can easily adapt them for other
(eg linux) things; look in the UnrealEngine/Engine/Build/BatchFiles
folder.

### No source code? No plugin.

Do not attempt to copy this plugin and remove the source from Source/TestPlugin
and depend entirely on a 3rd party library without any 'local' plugin code.

This won't work. The plugin builds a dynamic library in Binaries/ that is
linked to your project. This dynamic library will contain your static symbols;
the main target (eg. HelloWorldEditor) will link to this, but not contain the
symbols.

&tldr: Having at least one local .cpp file is mandatory for plugins.

### Duplicate build output

You might see build output like:

    TestPlugin: Added include path: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/libnpp/src
    TestPlugin: Added static library: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/libnpp/build/libnpp.a
    TestPlugin: Added include path: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/rust-extern/include
    TestPlugin: Added static library: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/rust-extern/target/debug/libexterntest-0771709c94325fc4.a
    TestPlugin: Added include path: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/libnpp/src
    TestPlugin: Added static library: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/libnpp/build/libnpp.a
    TestPlugin: Added include path: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/rust-extern/include
    TestPlugin: Added static library: /Users/doug/dev/unreal/projects/HelloWorld/Plugins/TestPlugin/3rdparty/rust-extern/target/debug/libexterntest-0771709c94325fc4.a

Notice how it all appears twice. No idea why; I presume the UBT runs in multiple passes.

### Missing symbols? Try extern...

Notice that the header file for rust-extern specifically does not have the required
'extern "C" { }' for the functions it exports.

It should; it should do this:

    #ifdef __cplusplus
    extern "C" {
    #endif
    ...
    #ifdef __cplusplus
    }
    #endif

There's a lot more information on this in http://stackoverflow.com/questions/16087451/where-is-the-best-place-to-put-the-ifdef-cplusplus-extern-c-endif
and around on SO, but this is explicitly used in the files that import extern (TestPlugin.cpp, Foo.cpp)
to make this requirement obvious.

### Out of sync?

Making updates to the static libraries and not seeing them reflected in the editor?

The hot reload in UE4 is somewhat flakey at the moment. Your best bet is to force a manual
rebuild; delete the files in Binaries/ and restart the editor.

It should prompt you to rebuild the plugin and editor targets.
