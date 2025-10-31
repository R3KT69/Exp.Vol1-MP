using UnityEngine;
using TMPro;
using PurrNet;

public class PlayerMovementNet : NetworkIdentity
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpHeight = 1.8f;
    public float gravity = -30f;
    public float mouseSensi = 1f;

    [Header("Animation")]
    public Animator animator; // assign your Animator here

    private TMP_InputField inputField;
    private CharacterController characterController;
    private Vector3 velocityVector;
    private bool isGrounded;

    public float normalizedVelocity { get; private set; }

    void Start()
    {
        inputField = GameObject.Find("Input")?.GetComponent<TMP_InputField>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        if (!isOwner) return;
        if (inputField != null && inputField.isFocused) return;

        // Ground check
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocityVector.y < 0)
            velocityVector.y = -2f;

        // Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movement vector
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Only normalize if diagonal
        if (move.magnitude > 1f)
            move.Normalize();

        // Apply movement
        characterController.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocityVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        velocityVector.y += gravity * Time.deltaTime;
        characterController.Move(velocityVector * Time.deltaTime);

        // Mouse rotation
        float mouseHorizontal = Input.GetAxis("Mouse X") * mouseSensi;
        transform.Rotate(0f, mouseHorizontal, 0f);

        // Animation Part
        float inputMagnitude = new Vector2(moveHorizontal, moveVertical).magnitude;
        normalizedVelocity = Mathf.Clamp01(inputMagnitude); // 0 = idle, 1 = full input
        if (animator != null)
            animator.SetFloat("Velocity", normalizedVelocity);
    }
}
