using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : Interactable
{
    public static List<Socket> sockets;

    public ElectronicComponent device;

    public Connector pluggedBy;

    private void Awake()
    {
        if (sockets == null) sockets = new List<Socket>();
        sockets.Add(this);
    }

    public void Connect(ElectronicComponent ec)
    {
        if (!device) return;

        device.ConnectTo(ec);
    }

    public void Disconnect(ElectronicComponent ec)
    {
        if (!device) return;

        device.DisconnectFrom(ec);
    }
}
