using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientObj : MonoBehaviour
{

    private Transform cube;
    private Socket sender;
    private IPEndPoint localEndPoint;

    private void InitClient()
    {
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        localEndPoint = new IPEndPoint(ipAddr, 11111);

        // Creation TCP/IP Socket using  
        // Socket Class Costructor 
        sender = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
        // Connect Socket to the remote  
        // endpoint using method Connect() 

    }

    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("PositionCube").GetComponent<Transform>();
        InitClient();
    }

    // Update is called once per frame
    void Update()
    {      
        int cube_x = Convert.ToInt32(Math.Ceiling(cube.position.x));
        int cube_y = Convert.ToInt32(Math.Ceiling(cube.position.y));
        int cube_z = Convert.ToInt32(Math.Ceiling(cube.position.z));

        // Creation of messagge that 
        // we will send to Server 
        string msg = "x:" + cube_x + ",y:" + cube_y + ",z:" + cube_z + "<EOF>";
        byte[] messageSent = Encoding.ASCII.GetBytes(msg);
        new Thread(() =>
        {
            if (!sender.Connected)
            {
                sender.Connect(localEndPoint);
            }
            int byteSent = sender.Send(messageSent);
        }).Start();
    }
}
