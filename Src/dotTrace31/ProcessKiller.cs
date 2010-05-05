using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JetBrains.dotTrace.Integration.Utils
{
  /// <summary>
  /// Summary description for ProcessKiller.
  /// </summary>
  public static class ProcessKiller
  {
    private const int TH32CS_SNAPPROCESS = 0x00000002;

    [DllImport("KERNEL32.dll")]
    public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);

    [DllImport("KERNEL32.dll")]
    private static extern int CloseHandle(IntPtr handle);

    [DllImport("KERNEL32.dll")]
    private static extern int Process32First(IntPtr handle, byte[] pe);

    [DllImport("KERNEL32.dll")]
    private static extern int Process32Next(IntPtr handle, byte[] pe);

    private static IEnumerable<PROCESSENTRY32> CollectProcesses()
    {
      var res = new List<PROCESSENTRY32>();

      IntPtr handle = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

      if ((int) handle > 0)
      {
        try
        {
          byte[] bytes = PROCESSENTRY32.GetByteArray();

          int retval = Process32First(handle, bytes);
          while (retval == 1)
          {
            var current = new PROCESSENTRY32(bytes);
            res.Add(current);
            retval = Process32Next(handle, bytes);
          }
        }
        finally
        {
          CloseHandle(handle);
        }
      }
      else
        throw new Exception("handle > 0");

      return res;
    }

    public static void KillProcessTree(int processId)
    {
      KillProcessTree(processId, CollectProcesses());
    }

    private static void KillProcessTree(int processId, IEnumerable<PROCESSENTRY32> context)
    {
      foreach (PROCESSENTRY32 process in context)
        if (process.ParentProcessID == processId)
          KillProcessTree((int) process.ProcessID, context);

      Process.GetProcessById(processId).Kill();
    }

    #region Nested type: PROCESSENTRY32

    private class PROCESSENTRY32
    {
      private const int SizeFieldOffset = 0;
      private const int ProcessIDFiledOffset = 8;
      private const int ParentProcessIDFiledOffset = 24;
      private const int Size = 564; // the whole size of the structure

      public readonly uint ParentProcessID;
      public readonly uint ProcessID;

      public PROCESSENTRY32(byte[] aData)
      {
        ProcessID = BitConverter.ToUInt32(aData, ProcessIDFiledOffset);
        ParentProcessID = BitConverter.ToUInt32(aData, ParentProcessIDFiledOffset);
      }

      public static byte[] GetByteArray()
      {
        var aData = new byte[Size];
        byte[] buint = BitConverter.GetBytes(Size);
        Buffer.BlockCopy(buint, 0, aData, SizeFieldOffset, buint.Length);
        return aData;
      }
    }

    #endregion
  }
}