using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    private Rigidbody2D rb;

    private Camera cam;
    private float screenHeight;
    private float screenWidth;

    private float playerHeight;
    private float playerWidth;

    private Vector2 movementDirection;
    private float movementSpeed;

    public GameManager gameManager;

    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        // Movement speed
        movementSpeed = 5f;
        jumpForce = 8f;

        // Control the bounds of the game using the maera
        cam = Camera.main;
        screenHeight = 2f * cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;
        playerHeight = 25f / 40f;
        playerWidth = 35f / 40f;

        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

    }

    private void PlayerMovement()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.W) && !animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        transform.transform.position = CheckBounds(rb.position);

        // Flip 
        if (dirX > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); 
        }
        else if (dirX < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); 
        }

    }
    private Vector3 CheckBounds(Vector3 position)
    {
        // Calculate the half-size of the player sprite
        float halfPlayerHeight = playerHeight / 2f;
        float halfPlayerWidth = playerWidth / 2f;

        // Calculate the screen boundaries considering the player sprite size
        float screenBoundaryTop = screenHeight / 2f - halfPlayerHeight;
        float screenBoundaryBottom = -screenHeight / 2f + halfPlayerHeight;
        float screenBoundaryRight = screenWidth / 2f - halfPlayerWidth;
        float screenBoundaryLeft = -screenWidth / 2f + halfPlayerWidth;

        // Limit position within screen borders
        position.x = Mathf.Clamp(position.x, screenBoundaryLeft, screenBoundaryRight);
        position.y = Mathf.Clamp(position.y, screenBoundaryBottom, screenBoundaryTop);

        // Return the position
        return position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Chest")
        {
            gameManager.GameOver();
        }

        // Check collision with floor to make transition back to walk animation
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if Player Hits an Enemy
        if (collision.gameObject.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            gameManager.IncreaseScore();
        }
    }
}
