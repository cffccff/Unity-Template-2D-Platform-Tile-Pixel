using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header ("Hp Enemy")]
    [SerializeField] Slider slider;
    private float currentHealth;
    [SerializeField] float animationTime = 0.4f;
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage=1;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private CharacterMovement playerHealth;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject itemPrefabBonus;
    private Rigidbody2D rigidbody;
    private EnemyPatrol enemyPatrol;
    //  private EnemyPatrol enemyPatrol;
    //panel replay
    private  TextMeshProUGUI resultTxt;
    [SerializeField] GameObject replayPanel;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();
         enemyPatrol = GetComponentInParent<EnemyPatrol>();
        rigidbody = GetComponent<Rigidbody2D>();
        resultTxt = GameObject.Find("ResultTxt").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        slider.maxValue = 5;
        if (gameObject.name == "FinalBoss")
        {
            slider.maxValue = 16;

        }
        if (gameObject.name == "Boss")
        {
            slider.maxValue = 10;

        }
        slider.minValue = 0;
        currentHealth = slider.maxValue;
        slider.value = currentHealth;
       
    }
    private void Update()
    {
        if (slider.value > 0)
        {
            cooldownTimer += Time.deltaTime;

            //Attack only when player in sight?
            if (PlayerInSight())
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    anim.SetTrigger("Attack_1");
                }
            }

            if (enemyPatrol != null)
                enemyPatrol.enabled = !PlayerInSight();
        }
        
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<CharacterMovement>();

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }


    public void TakeDamage(int dmg)
    {

        currentHealth -= dmg;
        anim.SetTrigger("Hurt");
        Debug.Log(gameObject.name);
        DOTween.To(() => slider.value, x => slider.value = x, currentHealth, animationTime);
        if (currentHealth <= 0)
        {
            rigidbody.simulated = false;
            Die();
        }
    }
    public void Die()
    {
       
        anim.SetTrigger("Death");
        if (gameObject.name == "Boss")
        {

            Instantiate(itemPrefabBonus, new Vector3(transform.position.x + 4, transform.position.y, transform.position.z), Quaternion.identity);
        }
        if (gameObject.name != "FinalBoss")
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            replayPanel.transform.DOScale(new Vector3(1, 1, 1), 1f);
            resultTxt.text = "Victory !";
        }        
       // Invoke("ScaleCorp", 1f);
        Invoke("DestroySelf", 2f);
        Invoke("PauseGame", 3f);


    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void ScaleCorp()
    {
        transform.DOScale(new Vector3(0, 0, 0), 1f);
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
}
