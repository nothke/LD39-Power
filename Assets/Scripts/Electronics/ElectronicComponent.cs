using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ElectronicComponent : MonoBehaviour
{
    //[System.NonSerialized]
    public bool powered;

    void Start()
    {
        OnShutDown();
    }

    public void Power(bool p)
    {
        powered = p;

        if (powered) OnPowered();
        else OnShutDown();
    }

    public virtual void OnPowered() { }
    public virtual void OnShutDown() { }

    // Connections:

    public List<ElectronicComponent> connected = new List<ElectronicComponent>();


    // GetConnectionTree will recursively add connections:

    public HashSet<ElectronicComponent> GetConnectionTree()
    {
        var set = new HashSet<ElectronicComponent>();

        set.Add(this);

        GetConnectionTree(set);

        return set;
    }

    public HashSet<ElectronicComponent> GetConnectionTree(HashSet<ElectronicComponent> set)
    {
        set.Add(this);

        foreach (var c in connected)
        {
            if (!set.Contains(c))
                set.UnionWith(c.GetConnectionTree(set));
        }

        return set;
    }

    public virtual void ConnectTo(ElectronicComponent ec)
    {
        if (!ec) return;

        if (!ec.connected.Contains(this))
            ec.connected.Add(this);

        if (!connected.Contains(ec))
            connected.Add(ec);

        UpdatePowerState();
    }

    [ContextMenu("Disconnect All")]
    public void DisconnectAll()
    {
        for (int i = connected.Count - 1; i >= 0; i--)
        {
            DisconnectFrom(connected[i]);
        }
    }

    public void DisconnectFrom(ElectronicComponent ec)
    {
        ec.connected.Remove(this);
        ec.UpdatePowerState();

        connected.Remove(ec);
        UpdatePowerState();
    }

    [ContextMenu("Update Power State")]
    public void UpdatePowerState()
    {
        var set = GetConnectionTree();

        bool isPowered = HasActiveSources(set);

        foreach (var c in set)
        {
            if (c.powered != isPowered)
            {
                c.powered = isPowered;

                if (isPowered) c.OnPowered();
                else c.OnShutDown();
            }
        }
    }

    public List<PowerSource> FindSources()
    {
        var set = GetConnectionTree();

        return FindSources(set);
    }

    public List<PowerSource> FindSources(HashSet<ElectronicComponent> set)
    {
        List<PowerSource> powerSources = new List<PowerSource>();

        foreach (var c in set)
        {
            PowerSource ps = c.GetComponent<PowerSource>();
            if (ps) powerSources.Add(ps);
        }

        return powerSources;
    }

    public bool HasActiveSources(HashSet<ElectronicComponent> set)
    {
        foreach (var c in set)
        {
            if (c.GetComponent<PowerSource>())
                return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        DisconnectAll();
    }

    // DEBUG:

    [ContextMenu("Debug Connections")]
    public void DebugConnections()
    {
        var set = GetConnectionTree();

        Debug.Log("Network has " + set.Count + " components");

        foreach (var c1 in set)
            foreach (var c2 in c1.connected)
                Debug.DrawLine(c1.transform.position, c2.transform.position, Color.red, 1);
    }

    [ContextMenu("Debug Sources")]
    public void DebugSources()
    {
        var powerSources = FindSources();

        Debug.Log("Number of power sources: " + powerSources.Count);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var c in connected)
        {
            if (c == null) continue;

            Gizmos.color = powered ? Color.green : Color.gray;
            Gizmos.DrawLine(c.transform.position, transform.position);
        }
    }
}
