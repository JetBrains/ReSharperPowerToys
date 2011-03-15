using System;
using System.IO;
using JetBrains.Annotations;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  public class NamedPipeClient : IDisposable
  {
    private const uint BUSY_PIPE_WAIT_TIME_MS = 2000;
    private readonly string myPipeName;
    private int myHandle = NamedPipeInterop.INVALID_HANDLE_VALUE;
    private uint myResponseBufferSize = 1024;

    public NamedPipeClient(string pipeName)
    {
      myPipeName = pipeName;
      Connect();
    }

    public int Handle
    {
      get { return myHandle; }
    }

    public string PipeName
    {
      get { return myPipeName; }
    }

    public uint ResponseBufferSize
    {
      get { return myResponseBufferSize; }
      set { myResponseBufferSize = value; }
    }

    public bool Connected
    {
      get { return myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE; }
    }

    #region IDisposable Members

    public void Dispose()
    {
      if (myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE)
      {
        NamedPipeInterop.CloseHandle(myHandle);
        myHandle = NamedPipeInterop.INVALID_HANDLE_VALUE;
      }
      GC.SuppressFinalize(this);
    }

    #endregion

    ~NamedPipeClient()
    {
      Dispose();
    }

    public void Close()
    {
      Dispose();
    }

    private bool Connect()
    {
      if (myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE)
        throw new InvalidOperationException("Pipe is already connected!");

      while (true)
      {
        myHandle = NamedPipeInterop.CreateFile(
          myPipeName, NamedPipeInterop.GENERIC_READ | NamedPipeInterop.GENERIC_WRITE,
          0, IntPtr.Zero, NamedPipeInterop.OPEN_EXISTING, 0, 0);

        if (myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE)
          break;

        if (NamedPipeInterop.GetLastError() != NamedPipeInterop.ERROR_PIPE_BUSY)
        {
          // TODO Better error handling?
          return false;
        }

        if (!NamedPipeInterop.WaitNamedPipe(myPipeName, BUSY_PIPE_WAIT_TIME_MS))
          return false;
      }

      // The pipe connected; change to message-read mode. 
      var mode = (int) NamedPipeInterop.PIPE_READMODE_MESSAGE;

      bool success = NamedPipeInterop.SetNamedPipeHandleState(myHandle, ref mode, IntPtr.Zero, IntPtr.Zero);
      if (!success)
      {
        Dispose();
        return false;
      }

      return true;
    }

    [CanBeNull]
    public byte[] WriteMessageReadResponse(byte[] message)
    {
      int written;

      bool success = NamedPipeInterop.WriteFile(myHandle, message, (uint) message.Length,
                                                out written, 0);
      if (!success)
        return null;

      var responseBuffer = new byte[myResponseBufferSize];
      int size = Convert.ToInt32(myResponseBufferSize);
      var fullBuffer = new MemoryStream(size);
      do
      {
        int readBytes;
        success = NamedPipeInterop.ReadFile(myHandle, responseBuffer, myResponseBufferSize,
                                            out readBytes, 0);

        if (!success && NamedPipeInterop.GetLastError() != NamedPipeInterop.ERROR_MORE_DATA)
          return null;

        fullBuffer.Write(responseBuffer, 0, readBytes);
      } while (!success);

      return fullBuffer.ToArray();
    }
  }
}