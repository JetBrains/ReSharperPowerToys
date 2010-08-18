msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.0.0 /p:VsVersion="9.0" /t:Clean
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.0.0 /p:VsVersion="9.0" /t:Build
msbuild Build.proj /p:ReSharperVersion2=%1 /p:ProductVersion=%1.0.0 /p:VsVersion="10.0" /t:Build
