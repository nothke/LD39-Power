using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class InteractionMode
{
    public Texture2D cursorPointer;
    public Vector2 cursorHotspot;
}

public class InteractionController : MonoBehaviour
{
    Transform cam;

    public Text centerText;

    //Item currentItem;

    public float interactDistance = 2;

    Interactable interactable;
    public Interactable Interactable { get { return interactable; } }

    void Start()
    {
        cam = transform;

        Dehover();
    }

    [HideInInspector]
    public Vector3 startScreenPosition;

    [System.NonSerialized]
    public RaycastHit hit;

    bool LMB { get { return Input.GetButtonDown("Fire1"); } }
    bool LMBup { get { return Input.GetButtonUp("Fire1"); } }

    void Update()
    {
        // WHEN HAVING NOTHING

        Ray ray;

        ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out hit, interactDistance))
            OnRayHit();
        else
            Dehover();

        if (interactable)
        {
            if (LMBup)
            {
                interactable.EndHold();
                interactable = null;
            }
        }
    }

    Interactable hoverInteractable;
    public Interactable HoverInteractable { get { return hoverInteractable; } }
    Interactable prevInteractable;

    public Sprite crosshairSprite;
    public Sprite interactSprite;

    void OnRayHit()
    {
        GameObject hito = hit.collider.gameObject;

        // Interactable
        hoverInteractable = hito.GetComponent<Interactable>();

        if (hoverInteractable && !interactable)
        {
            // HOVER

            Hover("LMB to Interact");
            SetCrosshair(interactSprite);

            // ONCLICK

            if (LMB)
            {
                interactable = hito.GetComponent<Interactable>();

                interactable.Use(this);
                interactable.StartHold();

                SetCrosshair(interactSprite);
            }

            startScreenPosition = Input.mousePosition;
        }
    }

    public Image crosshair;

    void SetCrosshair(Sprite tex)
    {
        if (crosshair)
            crosshair.sprite = tex;
    }

    Vector2 GetMouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    // Trinput is mouse X, mouse Y and scroll in one Vector3
    Vector3 GetTrinput()
    {
        return new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse ScrollWheel"));
    }

    void Hover(string text)
    {
        //hoverInteractable.gameObject.layer = LayerMask.NameToLayer("Hover");
        if (hoverInteractable != prevInteractable)
            prevInteractable = hoverInteractable;

        if (!centerText)
            return;

        centerText.enabled = true;
        centerText.text = text;
    }

    void Hover(Item item)
    {
        if (!centerText)
            return;

        centerText.enabled = true;
        centerText.text = item.name + "\n" + "Rightclick to take\n";// +"- - - -\n" + item.description;
    }

    void Dehover()
    {
        if (!interactable)
            SetCrosshair(crosshairSprite);

        if (prevInteractable)
            prevInteractable.gameObject.layer = LayerMask.NameToLayer("Default");

        if (!centerText)
            return;

        centerText.enabled = false;
    }

    void OnGUI()
    {
        // Only for debug:
        Rect debugRect = new Rect((Screen.width / 2), (Screen.height / 2), 1000, 400);
        if (hoverInteractable)
            GUI.Label(debugRect, hoverInteractable.name);
    }
}
