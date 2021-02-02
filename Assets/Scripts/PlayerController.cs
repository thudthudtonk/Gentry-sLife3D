using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float speed = 20.0f;
    private Rigidbody playerRb;
    public bool isOnGround = true;
    public bool playerActive = false;
    public float jumpForce = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Player movement left, right, up, down
        // Consider making it so that you only move forward and rotate around to change what forward is, and then have
        // the mop on spacebar go in whatever forward is at the time
        
        if (playerActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            transform.Rotate(Vector3.right * Time.deltaTime * speed * horizontalInput);
            transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                // Maybe put in a "mop" that swings when you press space which you have to do to collect shit?
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShitPile"))
        {
            Destroy(other);

            // Reduce the number of shit piles in some variable on the game controller

        }

    }

    public void setActive(bool state)
    {
        playerActive = state;
    }
}
