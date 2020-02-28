using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Reciever_Rope : MonoBehaviour
{
    private TextMesh textObj;
    private Socket listener;
    Socket clientSocket;
    Thread socket_thread;

    // Use this for initialization
    void Start()
    {
        /**
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
        */
        
        //Verbindung mit geparster Adresse
        IPAddress ipAddr = IPAddress.Parse("134.100.10.75");
        //Verbindung mit festgelegtem Port
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11115);
        
        listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(localEndPoint);
        listener.Listen(10);

        socket_thread = new Thread(() =>
        {
            clientSocket = listener.Accept();
        });
        socket_thread.Start();

    }

    // Update is called once per frame
    void Update()
    {
        

        try
        {
            if (clientSocket != null)
            {
                byte[] bytes = new Byte[1024];
                string data = null;

                //Empfangen der Daten
                while (true)
                {
                    int numByte = clientSocket.Receive(bytes);

                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    //Empfang stopt, wenn <EOF> gelesen wird
                    if (data.IndexOf("<EOF>") > -1 || !listener.Connected)
                        break;
                }
                if (!data.Equals(""))
                {
                    //Wenn diesen Frame Daten empfangen wurden, wird die Position angepasst.
                    String[] properties = data.Split('$');
                    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

                    transform.position = new Vector3(float.Parse(properties[0], culture),
                        float.Parse(properties[1], culture), float.Parse(properties[2], culture));

                    transform.localScale = new Vector3(float.Parse(properties[3], culture),
                        float.Parse(properties[4], culture), float.Parse(properties[5], culture));
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Errorcode" + e.ErrorCode);
        }
    }
}


