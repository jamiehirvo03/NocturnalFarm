using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int listIndex;
    public List<string> EnemyType = new List<string> {"EnemyScout", "EnemyGrunt", "EnemyRanged", "EnemyTank" };
    public List<float> EnemyHealth = new List<float> {50, 150, 100, 300 };
    public List<float> EnemySpeed = new List<float> {6, 4, 5, 2 };

    public string thisEnemyType;
    public float thisEnemyHealth;
    public float thisEnemySpeed;

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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeleeDetection")
        {
            inPlayerMeleeRange = true;
            Events.current.onPlayerMeleeAttack += OnPlayerMeleeAttack;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeleeDetection")
        {
            inPlayerMeleeRange = false;
            Events.current.onPlayerMeleeAttack -= OnPlayerMeleeAttack;
        }
    }

    private void OnPlayerMeleeAttack()
    {

    }
}
