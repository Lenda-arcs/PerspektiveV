using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Sender : MonoBehaviour
{
    private bool connected = false;
    private Transform cube;
    private Socket sender;
    private IPEndPoint localEndPoint;

    private void InitClient()
    {
        //Verbindung mit geparster Adresse
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        //Verbindung mit festgelegtem Port
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

        // Creation TCP/IP Socket using  
        // Socket Class Costructor 
        sender = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);

        // True, wenn keine Exception aufgetreten ist
        connected = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!connected)
        {
            try 
            { 
                InitClient(); 
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ErrorCode);
            }
            
        }
        
        // Initialisierten der Positions- und Rotationsvariablen
        string x_pos = transform.position.x.ToString();
        string y_pos = transform.position.y.ToString();
        string z_pos = transform.position.z.ToString();
        string x_rot = transform.rotation.x.ToString();
        string y_rot = transform.rotation.y.ToString();
        string z_rot = transform.rotation.z.ToString();

        // Stringliste der Eigenschaften und dem Endmarker
        String[] properties = {x_pos, y_pos, z_pos, x_rot, y_rot, z_rot, "<EOF>"};
        // Liste wird zu String konvertiert, Trennzeichen ist '$'
        string msg = string.Join("$", properties);
        byte[] messageSent = Encoding.ASCII.GetBytes(msg);

        if (!sender.Connected)
        {
            sender.Connect(localEndPoint);
        }

        int byteSent = sender.Send(messageSent);
    }
}
