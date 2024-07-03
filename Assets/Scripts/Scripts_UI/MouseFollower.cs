using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InventoryItem item;

    private InputAction _mousePositionAction;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas component not found in the root GameObject.");
        }

        item = GetComponentInChildren<InventoryItem>();
        if (item == null)
        {
            Debug.LogError("InventoryItem component not found in child GameObjects.");
        }
    }

    private void Start()
    {
        var playerInput = GetComponentInParent<PlayerInput>(); // First, check in parent
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>(); // If not found, broaden the search
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput component not found on the GameObject or anywhere in the scene.");
                this.enabled = false; // Optionally disable the script to prevent further errors.
                return;
            }
        }

        _mousePositionAction = playerInput.actions.FindAction("MousePosition");
        if (_mousePositionAction == null)
        {
            Debug.LogError("Mouse position action not found in the Input Action asset.");
            return;
        }

        _mousePositionAction.Enable();
        Debug.Log("Mouse position action enabled");
    }

    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }

    private void Update()
    {
        if (_mousePositionAction != null)
        {
            Vector2 mousePosition = _mousePositionAction.ReadValue<Vector2>();
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, mousePosition, canvas.worldCamera, out position);
            transform.position = canvas.transform.TransformPoint(position);
            Debug.Log($"Mouse position updated to: {transform.position}");
        }
        else
        {
            Debug.LogWarning("Mouse position action is not initialized.");
        }
    }

    public void Toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
}
