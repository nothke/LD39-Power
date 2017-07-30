using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    new public string name;

    [HideInInspector]
    public InteractionController manager;

    public virtual void Use(InteractionController im)
    {
        manager = im;
    }

    public virtual void StartHold() { }
    public virtual void EndHold() { }
    public virtual void UseHold() { }
}
