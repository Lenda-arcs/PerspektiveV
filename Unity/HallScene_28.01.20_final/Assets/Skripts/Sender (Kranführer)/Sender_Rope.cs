using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Sender_Rope : MonoBehaviour
{
    private bool initialized = false;
    private Socket sender;
    private IPEndPoint remoteEndpoint;

    private void InitClient()
    {
        
        //Verbindung mit geparster Adresse
        IPAddress ipAddr = IPAddress.Parse("134.100.10.75");
        //Verbindung mit festgelegtem Port
        remoteEndpoint = new IPEndPoint(ipAddr, 11115);
    /**
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        localEndPoint = new IPEndPoint(ipAddr, 11111);
    */
        // Creation TCP/IP Socket using
        // Socket Class Costructor
        sender = new Socket(SocketType.Stream, ProtocolType.Tcp);

        // True, wenn keine Exception aufgetreten ist
        initialized = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
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

        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        // Initialisierten der Positions- und Rotationsvariablen
        string x_pos = transform.position.x.ToString(culture);
        string y_pos = transform.position.y.ToString(culture);
        string z_pos = transform.position.z.ToString(culture);

        Vector3 scale = transform.localScale;

        string x_sca = scale.x.ToString(culture);
        string y_sca = scale.y.ToString(culture);
        string z_sca = scale.z.ToString(culture);


        // Stringliste der Eigenschaften und dem Endmarker
        String[] properties = {x_pos, y_pos, z_pos, x_sca, y_sca, z_sca, "<EOF>"};
        // Liste wird zu String konvertiert, Trennzeichen ist '$'
        string msg = string.Join("$", properties);
        Debug.Log(msg);
        byte[] messageSent = Encoding.ASCII.GetBytes(msg);

        if (!sender.Connected && initialized)
        {
            sender.Connect(remoteEndpoint);
        }

        int byteSent = sender.Send(messageSent);
    }
}
