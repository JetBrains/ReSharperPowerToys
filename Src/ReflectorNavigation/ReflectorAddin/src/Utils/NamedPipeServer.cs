using System;
using System.IO;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin.Utils
{
  /// <summary>
  /// NamedPipeServer - An implementation of a synchronous, message-based, named pipe server
  /// Taken from C# cookbook by O'Reilly
  /// </summary>
  public class NamedPipeServer : IDisposable
  {
    #region Delegates

    public delegate MemoryStream MessageReceivedDelegate(MemoryStream message);

    #endregion

    private const int PIPE_SERVER_BUFFER_SIZE = 8192;
    private const uint RECEIVE_BUFFER_SIZE = 1024;
    private readonly string myMachineName;
    private readonly MessageReceivedDelegate myMessageReceivedDelegate;
    private readonly string myPipeName;
    private int myHandle = NamedPipeInterop.INVALID_HANDLE_VALUE;

    public NamedPipeServer(string machineName,
                           string pipeBaseName,
                           MessageReceivedDelegate msgReceivedDelegate)
    {
      myMessageReceivedDelegate = msgReceivedDelegate;
      myMachineName = machineName ?? ".";
      myPipeName = "\\\\" + myMachineName + "\\PIPE\\" + pipeBaseName;
    }

    public string PipeName
    {
      get { return myPipeName; }
    }

    #region IDisposable Members

    public void Dispose()
    {
      // if we have a pipe handle, disconnect and clean up
      if (myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE)
      {
        NamedPipeInterop.DisconnectNamedPipe(myHandle);
        NamedPipeInterop.CloseHandle(myHandle);
        myHandle = NamedPipeInterop.INVALID_HANDLE_VALUE;
      }
    }

    #endregion

    public void Close()
    {
      Dispose();
    }

    public bool CreatePipe()
    {
      myHandle = NamedPipeInterop.CreateNamedPipe(myPipeName,
                                                  NamedPipeInterop.PIPE_ACCESS_DUPLEX,
                                                  NamedPipeInterop.PIPE_TYPE_MESSAGE |
                                                  NamedPipeInterop.PIPE_READMODE_MESSAGE |
                                                  NamedPipeInterop.PIPE_WAIT,
                                                  NamedPipeInterop.PIPE_UNLIMITED_INSTANCES,
                                                  PIPE_SERVER_BUFFER_SIZE,
                                                  PIPE_SERVER_BUFFER_SIZE,
                                                  NamedPipeInterop.NMPWAIT_WAIT_FOREVER,
                                                  IntPtr.Zero);

      // TODO Better error handling?
      return myHandle != NamedPipeInterop.INVALID_HANDLE_VALUE;
    }

    public bool HandleClient()
    {
      bool success = NamedPipeInterop.ConnectNamedPipe(myHandle, IntPtr.Zero);
      if (!success)
        return false;

      try
      {
        while (WaitForMessage()) ;
      }
      finally
      {
        NamedPipeInterop.DisconnectNamedPipe(myHandle);
      }

      return true;
    }

    public bool WaitForMessage()
    {
      bool success;

      var message = new MemoryStream();
      var buffer = new byte[RECEIVE_BUFFER_SIZE];

      do
      {
        int numberOfBytesRead;
        success = NamedPipeInterop.ReadFile(myHandle, buffer, RECEIVE_BUFFER_SIZE,
                                            out numberOfBytesRead, 0);

        if (!success && (NamedPipeInterop.GetLastError() != NamedPipeInterop.ERROR_MORE_DATA))
          return false;

        message.Write(buffer, 0, numberOfBytesRead);
      } while (!success);

      MemoryStream response = myMessageReceivedDelegate(message);
      if (response != null)
      {
        byte[] responseBytes = response.ToArray();
        int written;
        NamedPipeInterop.WriteFile(myHandle, responseBytes,
                                   (uint) responseBytes.Length, out written, 0);
      }

      return true;
    }
  }
}