using PurrNet;
using PurrNet.Transports;
using UnityEngine;

public class NetworkDriver : NetworkIdentity
{
    private string ip;
    public string port;
    public NetworkManager networkmngr;
    public UDPTransport transport;

    void Start()
    {
        ip = Connection_Menu.IPAddr;
        port = Connection_Menu.PortAddr;
        string mode = Connection_Menu.startMode;

        if (string.IsNullOrEmpty(ip))
        {
            Debug.LogWarning("No IP specified, defaulting to localhost");
            ip = "127.0.0.1";
        }

        
        transport.address = ip;
        transport.serverPort = ushort.Parse(port);

        if (mode == "Host")
        {
            networkmngr.StartHost();
        }
        else if (mode == "Client")
        {
            networkmngr.StartClient();
        }

        Debug.Log(ip + ":" + port);
    }

}
