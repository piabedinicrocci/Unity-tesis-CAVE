using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private Vector3 moveDirection = Vector3.zero;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("No CharacterController found on the camera.");
        }

        LockCursor();
    }

    void Update()
    {
        // Permitir desbloquear el cursor presionando la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Rotación de la cámara con el mouse
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Limitar el pitch para evitar que la cámara gire completamente sobre sí misma
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            // Movimiento de la cámara
            if (characterController.isGrounded)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                moveDirection = (forward * vertical + right * horizontal).normalized * speed;

                if (Input.GetKey(KeyCode.Space))
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            // Aplicar gravedad
            moveDirection.y -= gravity * Time.deltaTime;

            // Mover el CharacterController
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}