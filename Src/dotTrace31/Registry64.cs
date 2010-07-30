using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace JetBrains.ReSharper.PowerToys.dotTrace31
{
  /// <summary>
  /// Provides access to 64-bit registry for 32-bit managed applications
  /// </summary>
  public static class Registry64
  {
    private const int KEY_QUERY_VALUE = 0x0001;
    private const int KEY_ENUMERATE_SUB_KEYS = 0x0008;
    private const int KEY_WOW64_64KEY = 0x0100;

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int sam, out IntPtr phkResult);

    public static RegistryKey OpenSubKey(RegistryKey root, string subKey)
    {
      Type type = typeof(RegistryKey);
      var info = type.GetField("hkey", BindingFlags.NonPublic | BindingFlags.Instance);
      var remoteKeyHandle = (SafeHandle)info.GetValue(root);
      
      IntPtr hRemoteKey = remoteKeyHandle.DangerousGetHandle();
      IntPtr hTargetKey;
      RegOpenKeyEx(hRemoteKey, subKey, 0, KEY_QUERY_VALUE | KEY_WOW64_64KEY | KEY_ENUMERATE_SUB_KEYS, out hTargetKey);
      
      Assembly ass = typeof(SafeHandle).Assembly;
      Type type1 = ass.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
      var sh1 = (SafeHandle)Activator.CreateInstance(type1, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new object[] { hTargetKey, true }, null);

      try
      {
        return (RegistryKey)Activator.CreateInstance(typeof(RegistryKey), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { sh1, true }, null);
      }
      catch (MissingMethodException)
      {
        var registryViewType = ass.GetType("Microsoft.Win32.RegistryView");
        var registryView = Enum.ToObject(registryViewType, 0x100);

        // So, it's probably .NET 4.0 where we shouldn't have used all this stuff, but have so since we can't reference it directly)
        return (RegistryKey)Activator.CreateInstance(typeof(RegistryKey), BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { sh1, true, registryView }, null);
      }
    }
  }
}