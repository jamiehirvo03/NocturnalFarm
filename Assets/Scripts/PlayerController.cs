using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isSprinting;

    private Vector2 movement;
    private float moveLimiter = 0.7f;
    public Rigidbody2D rb;
    private Vector2 movementDirection;
    private Transform playerLocation;

    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;

    [SerializeField] private float playerHealth;
    [SerializeField] private float maxHealth = 150f;
    public Slider healthBar;

    [SerializeField] private float playerStamina;
    [SerializeField] private float maxStamina = 100;
    public Slider staminaBar;

    public  float staminaDecreasePerFrame = 5;
    public float staminaIncreasePerFrame = 20;
    private float staminaRegenTimer = 0;
    public float staminaTimeToRegen = 3;

    public LayerMask enemyLayer;
    public float playerVisionDistance = 10;
    private GameObject currentTarget;

    public Animator TorsoAnimator;
    public Animator LegsAnimator;


    // Start is called before the first frame update
    void Start()
    {
        playerLocation = this.transform;

        isSprinting = false;
        movementSpeed = walkSpeed;

        playerHealth = maxHealth;
        playerStamina = maxStamina;

        healthBar.maxValue = maxHealth;
        UpdateHealthBar();

        staminaBar.maxValue = maxStamina;
        UpdateStaminaBar();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MeleeAttack();
        }

        TorsoAnimator.SetFloat("Horizontal", movement.x);
        TorsoAnimator.SetFloat("Vertical", movement.y);
        TorsoAnimator.SetFloat("Speed", movement.sqrMagnitude);

        LegsAnimator.SetFloat("Horizontal", movement.x);
        LegsAnimator.SetFloat("Vertical", movement.y);
        LegsAnimator.SetFloat("Speed", movement.sqrMagnitude);
    }
    private void FixedUpdate()
    {
        if (movement.x != 0 && movement.y != 0)
        {
            movement *= moveLimiter;
        }

        if (movement.x != 0 | movement.y != 0)
        {
            if (isSprinting)
            {
                playerStamina = Mathf.Clamp(playerStamina - (staminaDecreasePerFrame * Time.deltaTime), 0, maxStamina);
                UpdateStaminaBar();

                staminaRegenTimer = 0;
            }
        }

        if (movement.x == 0 && movement.y == 0)
        {
            if (playerStamina < maxStamina)
            {
                if (staminaRegenTimer >= staminaTimeToRegen)
                {
                    playerStamina = Mathf.Clamp(playerStamina + (staminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
                    UpdateStaminaBar();
                }
                else
                {
                    staminaRegenTimer += Time.deltaTime;
                }
            }
        }
        rb.velocity = movement * movementSpeed;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerLocation.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        playerLocation.rotation = rotation;

        Debug.DrawRay(transform.position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        
        if (hit)
        {
            if (currentTarget == null)
            {
                currentTarget = hit.transform.gameObject;
                OnRaycastEnter(currentTarget);
            }
            else if (currentTarget != hit.transform.gameObject)
            {
                OnRaycastExit(currentTarget);
                currentTarget = hit.transform.gameObject;
                OnRaycastEnter(currentTarget);
            }
        }
    }

    private void OnRaycastEnter(GameObject target)
    {
        if (target.tag == "EnemyScout" | target.tag == "EnemyRanged" | target.tag == "EnemyGrunt" | target.tag == "EnemyTank")
        {
            if (target.GetComponent<EnemyController>().inPlayerVision == true)
            {
                target.GetComponent<EnemyController>().isTargeted = true;

                Events.current.ShowEnemyUI();
            }
        } 
    }
    private void OnRaycastExit(GameObject target)
    {
        target.GetComponent<EnemyController>().isTargeted = false;

        Events.current.HideEnemyUI();
    }
    private void MeleeAttack()
    {
        TorsoAnimator.SetTrigger("Attack");

        Events.current.PlayerLightAttack();
    }

    public void TakeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthBar.value = playerHealth;
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = playerStamina;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyScout")
        {
            playerHealth -= 20;
            UpdateHealthBar();
        }
        if (other.gameObject.tag == "EnemyRanged")
        {
            playerHealth -= 10;
            UpdateHealthBar();
        }
        if (other.gameObject.tag == "EnemyGrunt")
        {
            playerHealth -= 50;
            UpdateHealthBar();
        }
        if (other.gameObject.tag == "EnemyTank")
        {
            playerHealth -= 100;
            UpdateHealthBar();
        }
    }
}
