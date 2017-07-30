using UnityEngine;
using System.Collections;

public class Item : Interactable
{

    public class SelectionSettings
    {
        public Vector3 customHandPosition = new Vector3(0.15f, -0.15f, 0.1f);
        //public Vector3 customHandRotation = new Vector3(180, 0, 0);

        public Collider[] collidersToDisable;
        public Rigidbody[] rbsToDisable;
    }

    public SelectionSettings selectionSettings;

    public virtual void OnUsePrimary() { }
    public virtual void OnUsePrimaryDown() { }
    public virtual void OnUsePrimaryUp() { }
    public virtual void OnUseSecondary() { }
    public virtual void OnUseSecondaryDown() { }
    public virtual void OnUseSecondaryUp() { }

    public virtual void OnTake() { }

    public sealed override void Use(InteractionController im)
    {
        base.Use(im);

        ItemManager itemManager = im.GetComponent<ItemManager>();

        if (itemManager)
            itemManager.Take(this);
        else
            Debug.LogWarning("Item manager is missing");

        Debug.Log("Took: " + name);

        OnTake();
    }

    public void SetPhysics(bool enable) // every item should have rigidbody.... right?
    {
        Rigidbody itemRB = GetComponent<Rigidbody>();

        itemRB.isKinematic = !enable;

        if (enable)
            itemRB.interpolation = RigidbodyInterpolation.Interpolate;
        else
            itemRB.interpolation = RigidbodyInterpolation.None;

        foreach (Rigidbody rb in selectionSettings.rbsToDisable)
        {
            rb.isKinematic = !enable;

            if (enable)
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            else
                rb.interpolation = RigidbodyInterpolation.None;
        }
    }

    public void SetCollisions(bool enable)
    {
        if (GetComponent<Collider>())
            GetComponent<Collider>().enabled = enable;

        foreach (Collider col in selectionSettings.collidersToDisable)
            col.enabled = enable;
    }

    public void PlaceAt(Transform parent, Vector3 localPosition, Quaternion localRotation, bool enableColliders = false, bool enablePhysics = false)
    {
        transform.parent = parent;
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;

        SetCollisions(enableColliders);
        SetPhysics(enablePhysics);

        Debug.Log("Placed " + name + " at " + parent.name);
    }
}