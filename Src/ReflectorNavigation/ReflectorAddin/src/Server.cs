using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin.Utils;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class Server
  {
    #region Delegates

    public delegate string OnDecompileDelegate(IDictionary<string, string> arguments);

    #endregion

    private readonly OnDecompileDelegate myOnDecompile;

    public Server(OnDecompileDelegate onDecompile)
    {
      myOnDecompile = onDecompile;
      var serveThread = new Thread(ServeThread) {IsBackground = true};
      serveThread.Start();
    }

    private void ServeThread()
    {
      while (true)
      {
        try
        {
          using (var server = new NamedPipeServer(null, ReflectorConstants.PIPE_NAME, OnMessageReceived))
          {
            if (server.CreatePipe())
              while (server.HandleClient()) ;
          }
        }
        catch
        {
        }

        Thread.Sleep(500);
      }
    }

    private MemoryStream OnMessageReceived(MemoryStream message)
    {
      string argumentsString = Encoding.UTF8.GetString(message.ToArray());
      IDictionary<string, string> arguments = DeserializeDictionary(argumentsString);

      string responseString = myOnDecompile(arguments);
      return new MemoryStream(Encoding.UTF8.GetBytes(responseString));
    }

    private static IDictionary<string, string> DeserializeDictionary(string str)
    {
      if (string.IsNullOrEmpty(str))
        return new Dictionary<string, string>();

      var serializer = new XmlSerializer(typeof (SerializableDictionary<string, string>));

      using (var sr = new StringReader(str))
        return (IDictionary<string, string>) serializer.Deserialize(sr);
    }
  }
}