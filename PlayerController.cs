using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeedMultiplier = 2f;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float rotationClamp = 90f;

    public bool mouseIsLocked = true;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        uiManager.ToggleUIElements(false);
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        SetMouseLock();
        HandleSprinting();
        ToggleTimeFreeze();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W)) //forward Movement
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) //Backward movement
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) //Left movement
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) //Right movement
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        if (mouseIsLocked)
        {
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -rotationClamp, rotationClamp);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }
    }
    private void SetMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (mouseIsLocked)
            {
                mouseIsLocked = false;
                UnlockMouse();//Cursor.lockState = CursorLockMode.None;
                uiManager.ToggleUIElements(true);
            }
            else
            {
                mouseIsLocked = true;
                LockMouse();//Cursor.lockState = CursorLockMode.Locked;
                uiManager.ToggleUIElements(false);
            }
            
        }    
    }

    private void HandleSprinting()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= sprintSpeedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= sprintSpeedMultiplier;
        }

    }

    private void ToggleTimeFreeze()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uiManager.SetTimeFreezeAll();
        }
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
