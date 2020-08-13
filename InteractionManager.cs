using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";

    [SerializeField] UIManager uiManager = null;
    [SerializeField] Material selectionMaterial = null;

    GameObject selectedObject = null;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectObjects();

        }

    }

    private void SelectObjects()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //Blocks RayCasting through UI
            {
                if (hit.transform.CompareTag(selectableTag))
                {
                    if (selectedObject != hit.collider.gameObject && selectedObject != null) //checks if the clicked object is the same object and Deselects the old one
                    {
                        selectedObject.GetComponent<MovableObject>().DeselectObject();
                    }
                    selectedObject = hit.collider.gameObject;
                    uiManager.SetSelection(selectedObject);
                    selectedObject.GetComponent<MeshRenderer>().material = selectionMaterial; //Careful, might be null
                }
                else
                {
                    selectedObject.GetComponent<MovableObject>().DeselectObject();
                    selectedObject = null;
                    uiManager.SetSelection(selectedObject);
                }
            }

        }
    }
}
