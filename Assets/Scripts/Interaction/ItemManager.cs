using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform handPivot;
    Item itemInHands;

    public void Take(Item item)
    {
        itemInHands = item;
        item.PlaceAt(handPivot, item.selectionSettings.customHandPosition, Quaternion.identity);
    }

    public void DropFromHands()
    {
        if (!itemInHands) return;

        itemInHands.SetCollisions(true);
        itemInHands.SetPhysics(true);
        itemInHands.transform.parent = null;
        itemInHands = null;
    }

    public void DeRefItem()
    {
        if (!itemInHands) return;
        
        itemInHands.transform.parent = null;
        itemInHands = null;
    }

    private void Update()
    {
        if (!itemInHands) return;

        if (Input.GetMouseButtonDown(0))
            itemInHands.OnUsePrimaryDown();

        if (Input.GetMouseButtonUp(0))
            itemInHands.OnUsePrimaryUp();

        if (Input.GetMouseButton(0))
            itemInHands.OnUsePrimary();

        if (Input.GetMouseButtonDown(1))
            itemInHands.OnUseSecondaryDown();

        if (Input.GetMouseButtonUp(1))
            itemInHands.OnUseSecondaryUp();

        if (Input.GetMouseButton(1))
            itemInHands.OnUseSecondary();

        if (Input.GetKeyDown(KeyCode.F))
            DropFromHands();
    }
}
