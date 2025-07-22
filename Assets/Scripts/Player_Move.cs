using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public GameObject[] players;
    private List<GameObject> player = new List<GameObject>();
    public List<GameObject> Player => player;
    public static Player_Move instance; //{ get; private set; }

    public bool start = false;

    int HP;
    public int currentHash;

    public float jumpPower;
    public float speed;

    float curSpeed;
    float h;

    bool eSkill;
    bool defendSkill;
    bool qSkill;
    bool jumpAble;
    bool jumping;
    bool isCooldown;
    public bool canPlay;
    public bool canMove;
    public bool defending;
    public bool buttonUp;
    public bool skilling;
    public bool gamePaused;

    KeyCode jumpKey = KeyCode.Space;
    KeyCode runKey = KeyCode.LeftShift;
    MouseButton but = MouseButton.Right;

    public AnimationClip[] animClip;
    public Animation anim;

    Rigidbody2D rbody;
    public Animator animator;

    public LayerMask enemyLayer;
    public Vector2 attackOffset;
    public Vector2 attackSize;

    public HashSet<int> blockedStates;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        gameManager = GameManager.instance;

        blockedStates = new HashSet<int>
        {
            Animator.StringToHash("Player_Attack1"),
            Animator.StringToHash("Player_Attack2"),
            Animator.StringToHash("Player_Attack3"),
            //Animator.StringToHash("Player_Defend"),
            Animator.StringToHash("Player_Death")
        };
    }

    void Start()
    {
        /*SaveData saveData = LoadSystem.LoadGameData();
        if (saveData != null)
        {
            Debug.Log("파일을 찾음");
            foreach (Vector3 pos in saveData.playerData.positions)
            {
                this.transform.position = pos;
            }
        }*/

        StartCoroutine(EnableInputAfterDelay(0.3f));
        attackOffset = new Vector2(1.5f, -0.45f);
        attackSize = new Vector2(1.2f, 2f);
        jumpAble = true;
        jumping = false;
        canMove = true;
        canPlay = true;
        defending = false;
        buttonUp = true;
        eSkill = true;
        qSkill = true;
        defendSkill = true;
        isCooldown = false;
        skilling = false;
        HP = 100;

        players = GameObject.FindGameObjectsWithTag("Player");
        player.AddRange(players);
        //player.Add(this.gameObject);
        
        Debug.Log("플레이어 위치설정");
        
        PlayerPosLoad();
    }
    
    public void PlayerPosLoad() {
        gameManager.PlayerPosSet();
        this.transform.position = gameManager.playerPosition;
        this.transform.localScale = gameManager.playerScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.currentPause) {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            currentHash = stateInfo.shortNameHash;
            
            if (buttonUp == true && defending == false) {
                canPlay = true;
            }
            
            if (!blockedStates.Contains(currentHash)) {
                if (start) {
                    if (canPlay) {
                        Move();
                        Motion();
                    }
                    //Skill();
                }
            }
            
            if (HP <= 0) {
                Debug.Log("Player_Dead");
                animator.Play("Player_Dead");
            }
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
            return;
        }

        if (h != 0)
        {
            if (curSpeed > speed)
            {
                animator.Play("Player_Run");
            }
            else
            {
                animator.Play("Player_Walk");
            }
            return;
        }

        if (defending)
        {
            return;
        }

        if (skilling)
        {
            return;
        }

        animator.Play("Player_Idle");
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
        start = true;
        Debug.Log("플레이 가능");
    }

    public void NormalAttack()
    {
        skilling = true;
        defending = false;
        canPlay = true;
        animator.Play("Player_Attack1");
        buttonUp = true;
    }

    public void ESkill()
    {
        skilling = true;
        defending = false;
        canPlay = true;
        animator.Play("Player_Attack2");
        buttonUp = true;
    }

    public void QSkill()
    {
        skilling = true;
        defending = false;
        canPlay = true;
        animator.Play("Player_Attack3");
        buttonUp = true;
    }

    public void DefendSkill(int currentDefend)
    {
        if (currentDefend == 0)
        {//스킬을 실행하지 않은 상태
            //
        }
        else if (currentDefend == 1)
        {// 우클릭을 누름
            buttonUp = false;
            animator.Play("Player_Defend");
            canPlay = false;
            Debug.Log("Down");
        }
        else if (currentDefend == 2)
        {// 우클릭을 놓음
            buttonUp = true;
            Debug.Log("Up");
        }
        else
        {
            //
        }
    }

    IEnumerator ESkillCool(float cool)
    {
        Debug.Log("E스킬 쿨");
        eSkill = false;
        yield return new WaitForSeconds(cool);
        eSkill = true;
        Debug.Log("E스킬 준비");
    }

    IEnumerator QSkillCool(float cool)
    {
        Debug.Log("Q스킬 쿨");
        qSkill = false;
        yield return new WaitForSeconds(cool);
        qSkill = true;
        Debug.Log("Q스킬 준비");
    }

    public static void SkillTest()
    {
        Debug.Log("연동완료");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PickupItem") {
            ItemBase item = other.GetComponent<ItemBase>();
            InventorySys inv = InventorySys.instance;
            inv.AddItem(item.ItemName);
            Destroy(other.gameObject);
        }
    }
}