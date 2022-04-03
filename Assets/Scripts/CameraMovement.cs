using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private float mouseValue;
    [SerializeField]
    private float scrollFactor = 30f;
    private float moveFactor = 0.1f;
    private float rotateFactor = 0.4f;
    private Vector2 lastMousePosition = Vector2.zero;
    private Transform parent;
    private Transform myTransform;
    private Vector2 currentMousePosition;
    
    private void Awake()
    {
        parent = transform.parent;
        myTransform = transform;
    }

    void Update()
    {
        mouseValue = Mouse.current.scroll.ReadValue().y;
        currentMousePosition = Mouse.current.position.ReadValue();
        
        if (mouseValue != 0)
        {
            myTransform.position += mouseValue * scrollFactor * myTransform.forward;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (lastMousePosition != Vector2.zero)
            {
                parent.localPosition += (lastMousePosition.x - currentMousePosition.x) * moveFactor *
                                        parent.right;
                parent.localPosition += (lastMousePosition.y - currentMousePosition.y) * moveFactor *
                                        parent.forward;
            }

            lastMousePosition = currentMousePosition;
        }
        else if (Mouse.current.middleButton.isPressed)
        {
            if (lastMousePosition != Vector2.zero)
            {
                parent.localEulerAngles +=
                    new Vector3(0, -(lastMousePosition.x - currentMousePosition.x) * rotateFactor, 0);
            }

            lastMousePosition = currentMousePosition;
        }
        else
        {
            lastMousePosition = Vector2.zero;
        }
    }
}