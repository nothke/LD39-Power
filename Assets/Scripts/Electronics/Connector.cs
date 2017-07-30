using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Connector : MonoBehaviour
{
    public Connector otherConnector;
    public Socket pluggedInto;

    Cable _cable;
    Cable cable { get { if (!_cable) _cable = GetComponent<Cable>(); return _cable; } }

    Cable Cable { get { return cable ? cable : otherConnector.cable; } }

    void Start()
    {
        if (otherConnector)
        {
            Connect(otherConnector);
        }
    }

    void Connect(Connector to)
    {
        // TODO: make sure there are no multiple connections
        otherConnector = to;
        otherConnector.otherConnector = this;
    }

    private void OnDestroy()
    {
        if (pluggedInto && otherConnector.pluggedInto)
            pluggedInto.Disconnect(otherConnector.pluggedInto.device);

        if (cable)
            Destroy(Cable);

        Destroy(otherConnector);
    }

    public void UpdateMovement()
    {
        if (cable)
            cable.UpdateCable();
        else
            if (otherConnector)
            if (otherConnector.cable)
                otherConnector.cable.UpdateCable();

        CheckObstructions();
    }

    void CheckObstructions()
    {

    }

    public bool IsCableObstructed()
    {
        Cable cable = this.cable ? this.cable : otherConnector.cable;

        LayerMask layerMask = ~(1 << 8);

        RaycastHit hit;

        float distance = Vector3.Distance(cable.from.position, cable.to.position);
        if (Physics.SphereCast(cable.from.position, 0.1f, cable.to.position - cable.from.position, out hit, distance, layerMask))
        {
            Debug.DrawLine(cable.from.position, cable.to.position, Color.red);
            return true;
        }

        return false;
    }

    public Socket GetClosestSocket(float threshold = 1)
    {
        var sockets = Socket.sockets.Where(x => ((x.transform.position - transform.position).sqrMagnitude < threshold * threshold));

        if (sockets.Count() == 0) return null;

        return sockets.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).First();

        //return Socket.sockets.FirstOrDefault(x => ((x.transform.position - transform.position).sqrMagnitude < threshold * threshold));
    }

    public void AttachToClosestSocket(float threshold = 1)
    {
        Socket s = GetClosestSocket(threshold);

        if (s) AttachToSocket(s);
    }

    public ElectronicComponent GetDevice()
    {
        if (pluggedInto)
            return pluggedInto.device;

        return null;
    }

    public ElectronicComponent GetDeviceFromOtherSide()
    {
        return otherConnector.GetDevice();
    }

    public bool AttachToSocketIfNotObstructed(Socket s)
    {
        // TODO: Assert if s exists
        if (s.pluggedBy) return false;

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        transform.parent = s.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (IsCableObstructed())
        {
            transform.parent = null;
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            return false;
        }

        pluggedInto = s;
        s.pluggedBy = this;

        var device = GetDeviceFromOtherSide();

        if (device)
            pluggedInto.Connect(device);

        UpdateMovement();

        return true;
    }

    public void AttachToSocket(Socket s)
    {
        // TODO: Assert if s exists
        if (s.pluggedBy) return;

        pluggedInto = s;
        s.pluggedBy = this;

        var device = GetDeviceFromOtherSide();

        if (device)
            pluggedInto.Connect(device);

        transform.parent = s.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        UpdateMovement();
    }

    public void Disconnect()
    {
        if (!pluggedInto) return;

        if (GetDeviceFromOtherSide())
            pluggedInto.Disconnect(GetDeviceFromOtherSide());

        pluggedInto.pluggedBy = null;
        pluggedInto = null;

        transform.parent = null;
    }


    private void OnDrawGizmos()
    {
        if (otherConnector)
        {
            Gizmos.color = Color.yellow * 0.2f;
            Gizmos.DrawLine(transform.position, otherConnector.transform.position);
        }
    }
}
