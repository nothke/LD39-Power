using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device_ObjectToggler : ElectronicComponent
{
    public GameObject enableOnPowered;
    public GameObject enableOnShutdown;

    public override void OnPowered()
    {
        if (enableOnPowered)
            enableOnPowered.SetActive(true);

        if (enableOnShutdown)
            enableOnShutdown.SetActive(false);
    }

    public override void OnShutDown()
    {
        if (enableOnPowered)
            enableOnPowered.SetActive(false);

        if (enableOnShutdown)
            enableOnShutdown.SetActive(true);
    }
}