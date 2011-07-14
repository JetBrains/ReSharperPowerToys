using System;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public static class ReflectorConstants
  {
    public static readonly TimeSpan WAIT_AFTER_LAUNCH = TimeSpan.FromSeconds(2);
    public const int DEFAULT_PRIORITY = 20;
    public const string ID = "reflector";
    public const string LOCAL_PIPE = @"\\.\PIPE\" + PIPE_NAME;
    public const string PIPE_NAME = "REFLECTOR-BRIDGE-45F6-48AE-82E0-E4DB319FD716";
    public const string PRESENTABLE_SHORT_NAME = "Reflector";
  }
}