using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFPSController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Rotación del jugador (horizontal)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotación de la cámara (vertical)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Movimiento (solo direccional)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;
        Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        rb.linearVelocity = velocity;

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Detectar si está en el suelo
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
