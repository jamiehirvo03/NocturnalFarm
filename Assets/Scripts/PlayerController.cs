using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isSprinting;

    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    public Rigidbody2D rb;

    private Vector2 movementDirection;

    private Transform playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        playerLocation = this.transform;

        isSprinting = false;
        movementSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        movementDirection = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting)
        {
            movementSpeed = sprintSpeed;

            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = walkSpeed;

            isSprinting = false;
        }

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerLocation.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        playerLocation.rotation = rotation;
    }
    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            movementDirection *= moveLimiter;
        }

        rb.velocity = movementDirection * movementSpeed;
    }
}
