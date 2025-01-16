using UnityEngine;
using System.IO.Ports;

public class ListSerialPorts : MonoBehaviour
{
    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("Available Serial Ports:");
        foreach (string port in ports)
        {
            Debug.Log(port);
        }
    }
}
