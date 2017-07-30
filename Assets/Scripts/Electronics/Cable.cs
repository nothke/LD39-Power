using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public float width = 0.1f;
    public Material material;

    LineRenderer lr;

    private void Start()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        lr.material = material;

        UpdateCable();
    }

    private void Update()
    {
        UpdateCable();
    }

    public void UpdateCable()
    {
        if (!from || !to) Kill();

        lr.SetPositions(new Vector3[] { from.position, to.position });
    }

    public void Kill()
    {
        Destroy(lr);
        Destroy(this);
    }

    public void OnDestroy()
    {
        Destroy(lr);
    }
}
