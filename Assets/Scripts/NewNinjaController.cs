using LTAUnityBase.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
public class NewNinjaController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CircleCollider2D boxCollider2D;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject kunai;
    [SerializeField]
    Transform kunaiPos;
    [SerializeField]
    ButtonController BtnAttack, BtnThrow, BtnSlide, BtnGlide, BtnClimb;
    [SerializeField]
    GameObject glide, climb;
    public enum State
    {
        IDLE,
        RUN,
        JUMP,
        SLIDE,
        CLIMB,
        GLIDE,
        ATTACK,
        THROW,
        JUMPATTACK,
        JUMPTHROW,
        ENDCLIMB,
    }
    State currentState = State.IDLE;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<CircleCollider2D>();

        BtnAttack.OnClick((ButtonController btn) =>
        {
            JoystickInput.Attack = true;
        });
        BtnThrow.OnClick((ButtonController btn) =>
        {
            JoystickInput.Throw = true;
        });
        BtnSlide.OnClick((ButtonController btn) =>
        {
            JoystickInput.Slide = true;
        });
        BtnGlide.OnClick((ButtonController btn) =>
        {
            JoystickInput.Glide = true;

        });
        BtnClimb.OnClick((ButtonController btn) => {
            JoystickInput.Climb = true;
            currentState = State.CLIMB;
            rigidbody2D.gravityScale = 0;
            animator.Play("Climb");
            boxCollider2D.isTrigger = true;
            return;
        });
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.RUN:
                Run();
                break;
            case State.JUMP:
                Jump();
                break;
            case State.SLIDE:
                Slide();
                break;
            case State.GLIDE:
                Glide();
                break;
            case State.CLIMB:
                Climb();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.JUMPATTACK:
                JumpAttack();
                break;
            case State.THROW:
                Throw();
                break;
            case State.JUMPTHROW:
                JumpThrow();
                break;
            case State.ENDCLIMB:
                EndClimb();
                break;


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            if (currentState != State.CLIMB)
                climb.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            if (currentState != State.CLIMB)
                climb.SetActive(false);
            else
                JoystickInput.Climb = false;
        
    }
    void MoveLeft()
    {
        rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
    void MoveRight()
    {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
    void JumpAction()
    {
        currentState = State.JUMP;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 8);
        animator.Play("JumpUp");
        glide.SetActive(true);
    }
    void JumpEndClimb()
    {
        currentState = State.JUMP;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 4);
        animator.Play("JumpUp");
        glide.SetActive(true);
    }
   
    void SlideAction()
    {
        currentState = State.SLIDE;
        animator.Play("Slide");
        if (transform.eulerAngles.y == 180)
            rigidbody2D.velocity = new Vector2(-2 * speed, rigidbody2D.velocity.y);
        else
            rigidbody2D.velocity = new Vector2(2 * speed, rigidbody2D.velocity.y);

        StartCoroutine(StopSliding());
    }    
    void ReturnIdle()
    {
        currentState = State.IDLE;
        rigidbody2D.velocity = Vector3.zero;
        animator.Play("Idle");
        glide.SetActive(false);
    }
    void Landing()
    {
        if (rigidbody2D.velocity.y == 0)
        {
            JoystickInput.Glide = false;
            ReturnIdle();
            rigidbody2D.gravityScale = 1f;
        }
    }
    public void EndAttack()
    {
        JoystickInput.Attack = false;
       
    }
    public void ThrowKunai()
    {
        Instantiate(kunai, kunaiPos.position, kunaiPos.rotation);
    }
    public void EndThrow()
    {
        JoystickInput.Throw = false;
        
    }
    public void EndClimb()
    {
        Landing();
    }
    IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(0.5f);
        JoystickInput.Slide = false;
        ReturnIdle();
    }
   
   
    void Idle()
    {
        if (JoystickInput.Left)
        {
            currentState = State.RUN;
            MoveRight();
            animator.Play("Run");
            return;
        }
        if (JoystickInput.Right)
        {
            currentState = State.RUN;
            MoveLeft();
            animator.Play("Run");
            return;
        }

        if (JoystickInput.Up)
        {
            JumpAction();
            return;
        }
        if (JoystickInput.Slide == true)
        {
            SlideAction();
            return;
        }
        if (JoystickInput.Attack == true)
        {
            currentState = State.ATTACK;
            animator.Play("Attack");
            return;
        }
        if (JoystickInput.Throw == true)
        {
            currentState = State.THROW;
            animator.Play("Throw");
            return;
        }
        
    }
    void Run()
    {
        if (JoystickInput.Attack == true)
        {
            currentState = State.ATTACK;
            animator.Play("Attack");
            rigidbody2D.velocity = Vector2.zero;
            return;
        }
        if (JoystickInput.Slide == true)
        {
            SlideAction();
            return;
        }
        if (JoystickInput.Up)
        {
            JumpAction();
            return;
        }
        if (JoystickInput.Right)
        {
            MoveRight();
            return;
        }
        if (JoystickInput.Left)
        {
            MoveLeft(); 
            return;
        }
        ReturnIdle();
    }
    void Jump()
    {
        if (JoystickInput.Attack == true)
        {
            currentState = State.JUMPATTACK;
            animator.Play("JumpAttack");
            return;
        }
        if (JoystickInput.Throw == true)
        {
            currentState = State.JUMPTHROW;
            animator.Play("JumpThrow");
            return;
        }
        if (JoystickInput.Glide == true)
        {
            glide.SetActive(false);
            currentState = State.GLIDE;
            rigidbody2D.gravityScale = 0.5f;
            animator.Play("Glide");
            return;
        }
        if (rigidbody2D.velocity.y < 0)
        {
            animator.Play("JumpDown");
        }
        Landing();

        if (JoystickInput.Right)
        {
            MoveRight();
            return;
        }
        if (JoystickInput.Left)
        {
            MoveLeft();
            return;
        }
       
    }
    void Slide()
    {
        
        if (JoystickInput.Slide == false)
            ReturnIdle();
    }
    void Glide()
    {
        if (JoystickInput.Glide == false)
            ReturnIdle();
        Landing();
        if (JoystickInput.Right)
        {
            MoveRight();
            return;
        }
        if (JoystickInput.Left)
        {
            MoveLeft();
            return;
        }
    }
    void Climb()
    {
        if (JoystickInput.Climb == false)
        {
            currentState = State.ENDCLIMB;
            rigidbody2D.gravityScale = 1;
            boxCollider2D.isTrigger = false;
            JumpEndClimb();
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody2D.velocity = Vector2.up;
            animator.StopPlayback();
            return;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody2D.velocity = Vector2.down;
            animator.StopPlayback();
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2D.velocity = Vector2.left;
            animator.StopPlayback();
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2D.velocity = Vector2.right;
            animator.StopPlayback();
            return;
        }
       
        rigidbody2D.velocity = Vector2.zero;
        animator.StartPlayback();
    }
    void Attack()
    {
        if (JoystickInput.Attack == false)
        {

               ReturnIdle();
        }
    }

    void JumpAttack()
    {
        if (JoystickInput.Attack == false)
        {
              currentState = State.JUMP;
              animator.Play("JumpUp");
        }
    }

    public void Throw()
    {
        if(JoystickInput.Throw == false)
            ReturnIdle();
        
    }

    public void JumpThrow()
    {
        if (JoystickInput.Throw == false)
        {
            currentState = State.JUMP;
            animator.Play("JumpUp");
        }
    }
}
public class Player : SingletonMonoBehaviour<NewNinjaController>
{

}