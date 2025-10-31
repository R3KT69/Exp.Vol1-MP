using UnityEngine;
using TMPro;

public class PlayerMovementNet : MonoBehaviour
{
    public float speed = 5f;
    public TMP_InputField inputField;
    public float mouseSensi = 1f;
    
    
    void Start()
    {
        inputField = GameObject.Find("Input").GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputField.isFocused) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Move character forward/back/left/right
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);

        
         // Mouse horizontal input
        float mouseHorizontal = Input.GetAxis("Mouse X") * mouseSensi;

        // Rotate player around Y-axis
        transform.Rotate(0f, mouseHorizontal, 0f);
    }


}
