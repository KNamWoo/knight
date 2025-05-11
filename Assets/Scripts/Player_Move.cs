using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float jumpPower;
    public float speed;
    float h, v;

    bool jumpAble;

    public string[] animClip;
    KeyCode jumpKey = KeyCode.Space;

    Rigidbody2D rbody;
    Animation anim;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        jumpAble = true;
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
            anim.Play("Player_Walk");
        }else{
            anim.Play("Player_Idle");
        }
    }
}
