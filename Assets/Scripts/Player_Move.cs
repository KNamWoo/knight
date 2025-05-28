using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private bool canPlay = false;

    int HP;

    public float jumpPower;
    public float speed;

    float curSpeed;
    float h;

    public bool jumpAble;
    public bool jumping;

    KeyCode jumpKey = KeyCode.Space;
    KeyCode runKey = KeyCode.LeftShift;

    public AnimationClip[] animClip;
    public Animation anim;

    Rigidbody2D rbody;
    Animator animator;

    public LayerMask enemyLayer;
    public Vector2 attackOffset;
    public Vector2 attackSize;

    HashSet<int> blockedStates;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();

        blockedStates = new HashSet<int>
        {
            Animator.StringToHash("Player_Attack1"),
            Animator.StringToHash("Player_Attack2"),
            Animator.StringToHash("Player_Attack3"),
            Animator.StringToHash("Player_Death")
        };
    }

    void Start()
    {
        StartCoroutine(EnableInputAfterDelay(0.3f));
        attackOffset = new Vector2(1.5f, -0.45f);
        attackSize = new Vector2(1.2f, 2f);
        jumpAble = true;
        jumping = false;
        HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay)
        {
            return;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        int currentHash = stateInfo.shortNameHash;

        if (!blockedStates.Contains(currentHash))
        {
            Move();
            Motion();
            Skill();
        }

        if (HP <= 0)
        {
            Debug.Log("Player_Dead");
            animator.Play("Player_Dead");
        }
    }

    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(runKey))
        {
            curSpeed = speed * 1.5f;
        }
        else
        {
            curSpeed = speed;
        }
        
        if (Input.GetKeyDown(jumpKey) && jumpAble == true)
        {
            jumpAble = false;
            jumping = true;
            rbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, 0, 0) * curSpeed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Motion()
    {
        if (h > 0)
        {
            attackOffset = new Vector2(1.5f, -0.45f);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (h < 0)
        {
            attackOffset = new Vector2(-1.5f, -0.45f);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (jumping == true)
        {
            animator.Play("Player_Jump");
        }
        else if (h != 0)
        {
            if (curSpeed > speed)
            {
                animator.Play("Player_Run");
            }
            else
            {
                animator.Play("Player_Walk");
            }
        }
        else
        {
            animator.Play("Player_Idle");
        }
    }

    void Skill()
    {
        Vector2 attackPos = (Vector2)transform.position + attackOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyLayer);

        /*foreach (var hit in hits)
        {
            Debug.Log("적 감지됨: " + hit.name);
            hit.GetComponent<Enemy>()?.TakeDamage(1);
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            animator.Play("Player_Attack1");
            //StartCoroutine(Attack1(0.05f));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("Player_Attack2");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.Play("Player_Attack3");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            animator.Play("Player_Defend");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackOffset;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumping = false;
            jumpAble = true;
        }
    }

    IEnumerator EnableInputAfterDelay(float cool)
    {
        yield return new WaitForSeconds(cool);
        canPlay = true;
        Debug.Log("플레이 가능");
    }
}