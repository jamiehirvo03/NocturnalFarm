using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int listIndex;
    public List<string> EnemyType = new List<string> {"EnemyScout", "EnemyGrunt", "EnemyRanged", "EnemyTank" };
    public List<float> EnemyHealth = new List<float> {50, 150, 100, 300 };
    public List<float> EnemySpeed = new List<float> {6, 4, 5, 2 };

    public string thisEnemyType;
    public float thisEnemyHealth;
    public float thisEnemySpeed;

    public Slider EnemyHealthBar;

    [SerializeField] private bool inPlayerMeleeRange;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < EnemyType.Count; i++)
        {
            if (EnemyType[i] == this.gameObject.tag)
            {
                listIndex = i;

                thisEnemyType = EnemyType[listIndex];
                thisEnemyHealth = EnemyHealth[listIndex];
                thisEnemySpeed = EnemySpeed[listIndex];
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeleeDetection")
        {
            inPlayerMeleeRange = true;
            Events.current.onPlayerLightAttack += OnPlayerLightAttack;
            Events.current.onPlayerHeavyAttack += OnPlayerHeavyAttack;
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
    }

    private void OnPlayerLightAttack()
    {
        if (inPlayerMeleeRange)
        {
            thisEnemyHealth -= 50;

            UpdateEnemyHealth();
        }
    }

    private void OnPlayerHeavyAttack()
    {
        if (inPlayerMeleeRange)
        {
            thisEnemyHealth -= 100;
            
            UpdateEnemyHealth();
        }
    }

    private void UpdateEnemyHealth()
    {
        float healthPercentage;

        healthPercentage = (thisEnemyHealth / (EnemyHealth[listIndex])) * 100f;

        EnemyHealthBar.value = healthPercentage;
    }

}
