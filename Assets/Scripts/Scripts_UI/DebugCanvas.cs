using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    void Start()
    {
        if (FindObjectOfType<EventSystem>() != null)
        {
            Debug.Log("EventSystem found in scene.");
        }
        else
        {
            Debug.LogError("No EventSystem found in scene.");
        }

        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                Debug.Log("GraphicRaycaster found on Canvas.");
            }
            else
            {
                Debug.LogError("No GraphicRaycaster found on Canvas.");
            }
        }
        else
        {
            Debug.LogError("No Canvas found in parent.");
        }
    }
}
