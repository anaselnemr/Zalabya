using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 5f;

    // Rotation speed
    public float rotationSpeed = 5f;

    // Character controller component
    private CharacterController characterController;

    // Input values
    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        // Get the character controller component
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input values
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Normalize the movement direction
        movementDirection = movementDirection.normalized;

        // Calculate the movement speed
        float movementSpeed = moveSpeed * Time.deltaTime;

        // Move the character
        characterController.Move(movementDirection * movementSpeed);

        // Calculate rotation
        float yRotation = Input.GetAxis("Mouse X") * rotationSpeed;

        // Rotate the character
        transform.Rotate(0f, yRotation, 0f);
    }
}
