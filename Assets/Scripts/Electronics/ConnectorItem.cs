using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorItem : Item
{

    // Is it combined with other connector
    bool combined = true;

    public bool isPrimary;

    Connector _connector;
    Connector connector { get { if (!_connector) _connector = GetComponent<Connector>(); return _connector; } }

    ConnectorItem _otherItem;
    ConnectorItem otherItem { get { if (!_otherItem) _otherItem = connector.otherConnector.GetComponent<ConnectorItem>(); return _otherItem; } }

    public override void OnTake()
    {
        if (!combined)
        {
            isPrimary = true;
            otherItem.isPrimary = false;
        }

        connector.Disconnect();
        connector.transform.parent = manager.transform;
    }

    public override void OnUsePrimaryDown()
    {
        if (!manager.HoverInteractable) return;

        if (manager.HoverInteractable is Socket)
        {
            Socket s = manager.HoverInteractable as Socket;

            Debug.Log("SOCKET!");

            if (combined)
            {
                connector.otherConnector.AttachToSocket(s);

                StartCoroutine(WaitAndEnableCollisions(otherItem));
                DeCombine();
                return;
            }
            else
            {
                connector.AttachToSocket(s);
                StartCoroutine(WaitAndEnableCollisions(this));
                DeCombine();
                manager.GetComponent<ItemManager>().DeRefItem();
                return;
            }
        }

        // When picking up other connector
        if (!combined)
            if (manager.HoverInteractable is ConnectorItem)
            {
                ConnectorItem otherConnectorItem = manager.HoverInteractable as ConnectorItem;

                if (otherConnectorItem.connector == connector.otherConnector)
                {
                    // Combine!
                    otherConnectorItem.SetPhysics(false);
                    otherConnectorItem.SetCollisions(false);

                    otherConnectorItem.connector.Disconnect();

                    otherConnectorItem.transform.parent = transform;
                    otherConnectorItem.transform.localPosition = -Vector3.up * 0.1f;
                    otherConnectorItem.transform.localRotation = Quaternion.identity;

                    Combine();
                }
            }
    }

    IEnumerator WaitAndEnableCollisions(Item item)
    {
        yield return null;

        item.SetCollisions(true);
    }

    void Combine()
    {
        combined = true;
        otherItem.combined = true;
    }

    void DeCombine()
    {
        combined = false;
        otherItem.combined = false;
    }
}
