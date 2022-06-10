using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable IDE1006

// the code with the warning


public class CharacterMovement1 : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rigid;
    private BoxCollider2D coll;
    private float dirX = 0f;
    [SerializeField] private LayerMask jumpAbleGround;
    private float speed = 0f;
    private bool isAttacking = false;
    private Sprite sprite;
    private Slider slider;
    private  SpriteRenderer rend;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public int PlayerDamage = 1;
  
    private enum MovementState { Idle, Jump, Fall, Attack_1, Death }

    private void Awake()
    {

        coll = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        dirX = Input.GetAxisRaw("Horizontal");
       
       
        if (dirX != 0)
        {
            animator.SetTrigger("Walk");
            rigid.velocity = new Vector2(dirX * speed * 2, rigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftShift) && dirX != 0)
        {
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Run");
            speed = 5f;
        }
        
        else
        {
            speed = 0;
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            animator.SetTrigger("Jump_1");
            rigid.velocity = new Vector3(0, 15f, 0);
        }
        if (Input.GetMouseButtonDown(0)&&dirX==0)
        {
            isAttacking = true;
            
        }
        
        FlipSpriterigid(dirX);
      // UpdateState();

    }
    private void LateUpdate()
    {
        isAttacking = false;
    }

    private void Attacking()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit: " + enemy.name);
         //   enemy.GetComponent<Enemy>().TakeDamage(PlayerDamage);
        }
    }
    private void FlipSpriterigid(float horizontalInput)
    {
        if (horizontalInput > 0.01f)
        {
            rend.flipX = false;
        }

        else if (horizontalInput < -0.01f)
        {
            rend.flipX = true;
        }
    }
    private void UpdateState()
    {



        MovementState State = MovementState.Idle;
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
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpAbleGround);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

       

    }



    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
#pragma warning restore IDE1006