ReSharper PowerToys Pack — Source Code
----

[![obsolete JetBrains project](http://jb.gg/badges/obsolete-flat-square.svg)](https://confluence.jetbrains.com/display/ALL/JetBrains+on+GitHub)

Note: this file is provided for a quick reference only. 

The JetBrains ReSharper PowerToys Pack comprises a set of sample projects that illustrate basic techniques for ReSharper plugin development.
The samples are intended as a quickstart basis for building third-party ReSharper plugins, especially the plugin installers. 
Their source code may be freely used under the BSD License.


1. Folders.

    * Bin — the output folder for the binaries built out of the source code, and for the installers. The latter also use it as input folder for the plugin binaries to be included with the installation package.
    * Build — the main build file.
    * Doc — documentation on the PowerToys and ReSharper.
    * Obj — intermediate directory for the build process temporary files (relocated from under the projects).
    * Setup — the common infrastructure for the PowerToy installers.
    * Src — the source code for the PowerToy sample plugins.
    * Lib - libraries required to build PowerToys

1. Before you compile.

    To set up the freshly downloaded set of sources for local compilation, you should copy to Lib/ReSharper files from Bin folder of local R# installation. From this point on, you may open the solution in Visual Studio.

    Then, download and install a WiX version that is compatible with the samples, and correct the WiX location paths (see .Targets file that is included into each Setup project). 

2. Building.

    The Build/Build.proj file is the central build facility for both the source code and the installers. See the file itself for more detailed instructions. This file is an MSBuild build script.

3. Writing own R# Plugins.

    There are no special requirements for the plugin projects or DLLs.

    Building an installer for a plugin takes no more than setting up a few properties in the build file, provided that it's based on the infrastructure given by the PowerToys Pack; see setup build files for a self-descriptive example.

