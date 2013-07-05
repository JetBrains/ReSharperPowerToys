msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.1.0 /p:VsVersion="8.0" /t:Clean
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.1.0 /p:VsVersion="8.0" /t:PrepareBinaries
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.1.0 /p:VsVersion="8.0" /t:Build
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.1.0 /p:VsVersion="9.0" /t:Build
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.1.0 /p:VsVersion="10.0" /t:Build
