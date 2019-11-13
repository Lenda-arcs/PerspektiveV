using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

public class Reciever : MonoBehaviour
{
    private TextMesh textObj;
    private Socket listener;
    Socket clientSocket;

    // Use this for initialization
    void Start()
    {
        //Verbindung mit geparster Adresse
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        //Verbindung mit festgelegtem Port
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

        listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(localEndPoint);
        listener.Listen(10);

    }

    // Update is called once per frame
    void Update()
    {
        if (!listener.Connected)
        {
            new Thread(() =>
            {
                clientSocket = listener.Accept();
            }).Start();
        }

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
                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }

                if (!data.Equals(""))
                {
                    //Wenn diesen Frame Daten empfangen wurden, wird die Position angepasst.
                    String[] properties = data.Split('$');
                    transform.position = new Vector3(float.Parse(properties[0]), float.Parse(properties[1]), float.Parse(properties[2]));
                    transform.rotation = Quaternion.Euler(float.Parse(properties[3]), float.Parse(properties[4]), float.Parse(properties[5]));
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket Errorcode" + e.ErrorCode);
        }
    }
}


