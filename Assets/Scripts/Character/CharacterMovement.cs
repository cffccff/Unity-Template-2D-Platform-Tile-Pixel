using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
#pragma warning disable IDE1006

// the code with the warning


public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Slider slider;
    public static float playerHP=10;
    private float animationTime =0.5f;

    public Animator animator;
    private Rigidbody2D rigid;
    private BoxCollider2D coll;
    private float dirX = 0f;
    [SerializeField] private LayerMask jumpAbleGround;
    private float speed = 0f;
    private bool isAttacking = false;
    private Sprite sprite;
    private  SpriteRenderer rend;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public static int playerDamage;
    public static int jumpPower;
    private bool isHurt = false;
    private  TextMeshProUGUI resultTxt;
    [SerializeField] GameObject replayPanel;
    private enum MovementState { Idle, Jump, Fall, Attack_1, Death,Hurt }
    MovementState State;
    private void Awake()
    {
        playerDamage = 1;
        jumpPower = 10;
        slider.maxValue = playerHP;
        slider.minValue = 0;
        slider.value = playerHP;
        coll = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        resultTxt = GameObject.Find("ResultTxt").GetComponent<TextMeshProUGUI>();
        replayPanel.transform.localScale= new Vector3(0, 0, 0);

    }
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
     
        dirX = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(dirX * speed * 2, rigid.velocity.y);
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rigid.velocity = new Vector3(0, jumpPower, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift) && dirX != 0)
        {

            speed = 5f;
        }
        else
        {
            speed = 0;
        }

        if (Input.GetMouseButtonDown(0) && dirX == 0)
        {
            isAttacking = true;

        }
        UpdateState();
        FlipSpriterigid(dirX);
      

    }
    private void LateUpdate()
    {

        isAttacking = false;
         Invoke("ResetHurt", 1f);
        if (slider.value <= 0)
        {
            rigid.simulated = false;
            Invoke("PauseAnimation", 1.6f);
            Invoke("DisplayPanelWithLost", 1f);
        }

    }
    private void PauseAnimation()
    {
        animator.enabled = false;
    }
    private void Attacking()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit: " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
        }
    }
    private void FlipSpriterigid(float horizontalInput)
    {
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
           
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
           
        }
    }
    private void UpdateState()
    {

        MovementState State = MovementState.Idle;
        if (slider.value <= 0)
        {
            State = MovementState.Death;
        }
        if (isHurt ==true)
        {
            Debug.Log("Hurt");
            State = MovementState.Hurt;
        }

        if (rigid.velocity.y > 0.1f)
        {

            State = MovementState.Jump;
        }
        else if (rigid.velocity.y < -0.1f)
        {
            State = MovementState.Fall;
        }

        if (Mathf.Abs(dirX) > 0f && speed <= 2f)
        {

            speed = 2f;
            animator.SetFloat("Speed", speed);
        }
        else if (speed > 2f)
        {
            speed = 5f;
            animator.SetFloat("Speed", speed);
        }
        else
        {
            speed = 0f;
            animator.SetFloat("Speed", speed);
        }

        if (isAttacking == true)
        {

            State = MovementState.Attack_1;
        }


        animator.SetInteger("State", (int)State);



    }
    private void ResetHurt()
    {
        isHurt = false;
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpAbleGround);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Water"))
        {

            slider.value = 0;
            
        }


    }

    private void DisplayPanelWithLost()
    {
        replayPanel.transform.DOScale(new Vector3(1, 1, 1), 1f);
        resultTxt.text = "You are lost";
        resultTxt.color = Color.red;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void TakeDamage(int damage)
    {
        slider.value -= damage;

        DOTween.To(() => slider.value, x => slider.value = x, slider.value, animationTime);
        isHurt = true;
    }
   
}
#pragma warning restore IDE1006