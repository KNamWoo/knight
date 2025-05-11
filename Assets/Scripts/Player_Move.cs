using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float jumpPower;
    public float speed;
    public float h, v;

    public bool jumpAble;
    public bool jumping;

    public AnimationClip[] animClip;
    KeyCode jumpKey = KeyCode.Space;

    Rigidbody2D rbody;
    public Animation anim;
    Animator animator;
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        jumpAble = true;
        jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Motion();
    }

    void Move(){
        h = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(jumpKey) && jumpAble == true){
            jumpAble = false;
            jumping = true;
            rbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }else{
            v = 0;
            jumpAble = true;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, 0, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Motion(){
        if(h > 0){
            transform.localScale = new Vector3(1, 1, 1);
        }else if(h < 0){
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(jumping == true){
            animator.Play("Player_Jump");
        }else if(h != 0){
            animator.Play("Player_Walk");
        }else{
            animator.Play("Player_Idle");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground"){
            jumping = false;
        }
    }
}
