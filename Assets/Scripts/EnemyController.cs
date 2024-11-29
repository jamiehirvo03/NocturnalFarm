using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int listIndex;
    public List<string> EnemyType = new List<string> {"EnemyScout", "EnemyGrunt", "EnemyRanged", "EnemyTank" };
    public List<float> EnemyHealth = new List<float> {50, 150, 100, 300 };
    public List<float> EnemyMoveSpeed = new List<float> {6, 4, 5, 2 };
    public List<float> EnemyAttackDelay = new List<float> { 1, 2, 3, 4 };
    public List<float> EnemyAttackDamage = new List<float> { 10, 50, 30, 100 };

    public string thisEnemyType;
    public float thisEnemyHealth;
    public float thisEnemyMoveSpeed;
    public float thisEnemyAttackDelay;
    public float thisEnemyAttackDamage;

    private float attackCooldownTimer = 0;

    public GameObject EnemyUI;
    public Slider EnemyHealthBar;
    public TextMeshProUGUI EnemyName;

    public GameObject Player;

    [SerializeField] public bool inPlayerVision;
    [SerializeField] private bool inPlayerMeleeRange;

    public enum EnemyState
    {
        Idle,
        Pursuing,
        Attacking
    }
    public EnemyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        Events.current.onShowEnemyUI += ShowEnemyUI;
        Events.current.onHideEnemyUI += HideEnemyUI;

        EnemyUI.SetActive(false);

        for (int i = 0; i < EnemyType.Count; i++)
        {
            if (EnemyType[i] == this.gameObject.tag)
            {
                listIndex = i;

                thisEnemyType = EnemyType[listIndex];
                thisEnemyHealth = EnemyHealth[listIndex];
                thisEnemyMoveSpeed = EnemyMoveSpeed[listIndex];
                thisEnemyAttackDelay = EnemyAttackDelay[listIndex];
                thisEnemyAttackDamage = EnemyAttackDamage[listIndex];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (thisEnemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
       switch (currentState)
        {
            case EnemyState.Idle:

                attackCooldownTimer = 0;


                break;
            case EnemyState.Pursuing:

                attackCooldownTimer = 0;

                Vector2 direction = Player.transform.position - this.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                this.gameObject.transform.rotation = rotation;

                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, thisEnemyMoveSpeed * Time.deltaTime);

                break;
            case EnemyState.Attacking:

                if (attackCooldownTimer >= thisEnemyAttackDelay)
                {
                    Player.GetComponent<PlayerController>().TakeDamage(thisEnemyAttackDamage);

                    Debug.Log($"{thisEnemyType} attacks for {thisEnemyAttackDamage} damage!");

                    attackCooldownTimer = 0;
                }
                else
                {
                    attackCooldownTimer += Time.deltaTime;
                }
                
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeleeDetection")
        {
            inPlayerMeleeRange = true;
            Events.current.onPlayerLightAttack += OnPlayerLightAttack;
            Events.current.onPlayerHeavyAttack += OnPlayerHeavyAttack;
        }
        if (other.gameObject.tag == "PlayerVision")
        {
            inPlayerVision = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeleeDetection")
        {
            inPlayerMeleeRange = false;
            Events.current.onPlayerLightAttack -= OnPlayerLightAttack;
            Events.current.onPlayerHeavyAttack -= OnPlayerHeavyAttack;
        }
        if (other.gameObject.tag == "PlayerVision")
        {
            inPlayerVision = false;
        }
    }

    private void OnPlayerLightAttack()
    {
        if (inPlayerMeleeRange)
        {
            thisEnemyHealth -= 50;
        }
    }
    private void OnPlayerHeavyAttack()
    {
        if (inPlayerMeleeRange)
        {
            thisEnemyHealth -= 100;
        }
    }
    public void UpdateEnemyHealth()
    {
        float healthPercentage;

        healthPercentage = (thisEnemyHealth / (EnemyHealth[listIndex])) * 100f;

        EnemyHealthBar.value = healthPercentage;

        if (EnemyName.text != $"{thisEnemyType}")
        {
            EnemyName.text = $"{thisEnemyType}";
        }
    }
    public void ShowEnemyUI()
    {
        if (EnemyUI.gameObject.activeSelf == false)
        {
            EnemyUI.SetActive(true);
        }
    }
    public void HideEnemyUI()
    {
        if (EnemyUI.gameObject.activeSelf == true)
        {
            EnemyUI.SetActive(false);
        }
    }
}
