using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionTest : ElectronicComponent
{

    private void OnDrawGizmos()
    {
        Gizmos.color = powered ? Color.green : Color.gray;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        foreach (var c in connected)
        {
            Gizmos.DrawLine(c.transform.position, transform.position);
        }
    }
}
