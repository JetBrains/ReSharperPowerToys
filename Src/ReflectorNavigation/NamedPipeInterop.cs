using System;
using System.Runtime.InteropServices;
using System.Security;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  [SuppressUnmanagedCodeSecurity]
  public static class NamedPipeInterop
  {
    // #defines related to named pipe processing
    public const uint CREATE_ALWAYS = 2;
    public const uint CREATE_NEW = 1;
    public const uint ERROR_MORE_DATA = 234;
    public const uint ERROR_NO_DATA = 232;
    public const uint ERROR_PIPE_BUSY = 231;
    public const uint ERROR_PIPE_CONNECTED = 535;
    public const uint ERROR_PIPE_LISTENING = 536;
    public const uint ERROR_PIPE_NOT_CONNECTED = 233;
    public const uint GENERIC_ALL = (0x10000000);
    public const uint GENERIC_EXECUTE = (0x20000000);
    public const uint GENERIC_READ = (0x80000000);
    public const uint GENERIC_WRITE = (0x40000000);
    public const int INVALID_HANDLE_VALUE = -1;
    public const uint NMPWAIT_NOWAIT = 0x00000001;
    public const uint NMPWAIT_USE_DEFAULT_WAIT = 0x00000000;
    public const uint NMPWAIT_WAIT_FOREVER = 0xffffffff;
    public const uint OPEN_ALWAYS = 4;
    public const uint OPEN_EXISTING = 3;
    public const uint PIPE_ACCESS_DUPLEX = 0x00000003;
    public const uint PIPE_ACCESS_INBOUND = 0x00000001;
    public const uint PIPE_ACCESS_OUTBOUND = 0x00000002;
    public const uint PIPE_CLIENT_END = 0x00000000;

    public const uint PIPE_NOWAIT = 0x00000001;
    public const uint PIPE_READMODE_BYTE = 0x00000000;
    public const uint PIPE_READMODE_MESSAGE = 0x00000002;
    public const uint PIPE_SERVER_END = 0x00000001;
    public const uint PIPE_TYPE_BYTE = 0x00000000;
    public const uint PIPE_TYPE_MESSAGE = 0x00000004;

    public const uint PIPE_UNLIMITED_INSTANCES = 255;
    public const uint PIPE_WAIT = 0x00000000;

    public const uint TRUNCATE_EXISTING = 5;

    public static int GetLastError()
    {
      return Marshal.GetLastWin32Error();
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CallNamedPipe(
      string lpNamedPipeName,
      byte[] lpInBuffer,
      uint nInBufferSize,
      byte[] lpOutBuffer,
      uint nOutBufferSize,
      byte[] lpBytesRead,
      uint nTimeOut);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(int hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ConnectNamedPipe(
      int hNamedPipe, // handle to named pipe
      IntPtr lpOverlapped // overlapped structure
      );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int CreateNamedPipe(
      String lpName, // pipe name
      uint dwOpenMode, // pipe open mode
      uint dwPipeMode, // pipe-specific modes
      uint nMaxInstances, // maximum number of instances
      uint nOutBufferSize, // output buffer size
      uint nInBufferSize, // input buffer size
      uint nDefaultTimeOut, // time-out interval
      //SecurityAttributes attr     
      IntPtr pipeSecurityDescriptor // security descriptor
      );


    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int CreatePipe(
      int hReadPipe,
      int hWritePipe,
      IntPtr lpPipeAttributes,
      uint nSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int CreateFile(
      String lpFileName, // file name
      uint dwDesiredAccess, // access mode
      uint dwShareMode, // share mode
      IntPtr attr, // security descriptor
      uint dwCreationDisposition, // how to create
      uint dwFlagsAndAttributes, // file attributes
      uint hTemplateFile); // handle to template file

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool DisconnectNamedPipe(int hNamedPipe);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FlushFileBuffers(int hFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetNamedPipeHandleState(
      int hNamedPipe,
      IntPtr lpState,
      IntPtr lpCurInstances,
      IntPtr lpMaxCollectionCount,
      IntPtr lpCollectDataTimeout,
      string lpUserName,
      uint nMaxUserNameSize);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    public static extern bool GetNamedPipeInfo(
      int hNamedPipe,
      out uint lpFlags,
      out uint lpOutBufferSize,
      out uint lpInBufferSize,
      out uint lpMaxInstances);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    public static extern bool PeekNamedPipe(
      int hNamedPipe,
      byte[] lpBuffer,
      uint nBufferSize,
      byte[] lpBytesRead,
      out uint lpTotalBytesAvail,
      out uint lpBytesLeftThisMessage);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    public static extern bool SetNamedPipeHandleState(
      int hNamedPipe,
      ref int lpMode,
      IntPtr lpMaxCollectionCount,
      IntPtr lpCollectDataTimeout);

    [DllImport("KERNEL32.DLL", SetLastError = true)]
    public static extern bool TransactNamedPipe(
      int hNamedPipe,
      byte[] lpInBuffer,
      uint nInBufferSize,
      [Out] byte[] lpOutBuffer,
      uint nOutBufferSize,
      IntPtr lpBytesRead,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WaitNamedPipe(
      string name,
      uint timeout);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadFile(
      int hFile, // handle to file
      byte[] lpBuffer, // data buffer
      uint nNumberOfBytesToRead, // number of bytes to read
      out int lpNumberOfBytesRead, // number of bytes read
      uint lpOverlapped // overlapped buffer
      );


    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteFile(
      int hFile, // handle to file
      byte[] lpBuffer, // data buffer
      uint nNumberOfBytesToWrite, // number of bytes to write
      out int lpNumberOfBytesWritten, // number of bytes written
      uint lpOverlapped // overlapped buffer
      );
  }
}