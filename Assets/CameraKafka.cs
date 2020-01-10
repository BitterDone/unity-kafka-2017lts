using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CameraKafka : MonoBehaviour {

	public Text socketDisplay;

	// Use this for initialization
	void Start () {
		send();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void send()
	{
		try
		{

			// Establish the remote endpoint  
			// for the socket. This example  
			// uses port 11111 on the local  
			// computer. 
			IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress ipAddr = ipHost.AddressList[0];
			// IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("fe80::ddf6:3d0f:99c2:3e2e%10"), 11111); // 192.168.2.150 fe80::ddf6:3d0f:99c2:3e2e%10

			// Creation TCP/IP Socket using  
			// Socket Class Costructor 
			Socket sender = new Socket(ipAddr.AddressFamily,
					   SocketType.Stream, ProtocolType.Tcp);

			try
			{

				// Connect Socket to the remote  
				// endpoint using method Connect() 
				sender.Connect(localEndPoint);

				// We print EndPoint information  
				// that we are connected 
				Debug.Log(String.Format("Socket connected to -> {0} ", sender.RemoteEndPoint.ToString()));

				// Creation of messagge that 
				// we will send to Server 
				byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
				int byteSent = sender.Send(messageSent);

				// Data buffer 
				byte[] messageReceived = new byte[1024];

				// We receive the messagge using  
				// the method Receive(). This  
				// method returns number of bytes 
				// received, that we'll use to  
				// convert them to string 
				int byteRecv = sender.Receive(messageReceived);
				string msg = String.Format("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
				Debug.Log(msg);
				socketDisplay.text = msg;

				// Close Socket using  
				// the method Close() 
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
			}

			// Manage of Socket's Exceptions 
			catch (ArgumentNullException ane)
			{

				Debug.Log(String.Format("ArgumentNullException : {0}", ane.ToString()));
			}

			catch (SocketException se)
			{

				Debug.Log(String.Format("SocketException : {0}", se.ToString()));
			}

			catch (Exception e)
			{
				Debug.Log(String.Format("Unexpected exception : {0}", e.ToString()));
			}
		}

		catch (Exception e)
		{

			Debug.Log(e.ToString());
		}
	}
}
