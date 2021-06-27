using LTAUnityBase.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
public class TeamNinjaController : WariorController
{
   
    [SerializeField]
    GameObject glide, climb;
    [SerializeField]
    ButtonController BtnClimb, BtnGlide;
    protected override void Start()
    {
        base.Start();
        BtnClimb.OnClick((ButtonController btn) => {
            JoystickInput.Climb = true;
            ClimbAction();
            return;
        });
       
        BtnGlide.OnClick((ButtonController btn) =>
        {
            JoystickInput.Glide = true;

        });
    }
    protected override void ClimbAction()
    {
        currentState = State.CLIMB;
        rigidbody2D.gravityScale = 0;
        animator.Play("Climb");
        boxCollider2D.isTrigger = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            if (currentState != State.CLIMB)
                climb.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            if (currentState != State.CLIMB)
                climb.SetActive(false);
            else
            {
                climb.SetActive(false);
                JoystickInput.Climb = false;
            }

    }
    void JumpEndClimb()
    {
        currentState = State.JUMP;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 4);
        animator.Play("JumpUp");
        glide.SetActive(true);
    }
    protected override void JumpAction()
    {
        currentState = State.JUMP;
        base.JumpAction();
        glide.SetActive(true);
    }
    protected override void IdleAction()
    {
        base.IdleAction();
        currentState = State.IDLE;
        glide.SetActive(false);
    }
   
    protected override void Jump()
    {
        base.Jump();
        if (JoystickInput.Attack == true)
        {
            currentState = State.JUMPATTACK;
            animator.Play("JumpAttack");
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
        if (JoystickInput.Throw == true)
        {
            currentState = State.JUMPTHROW;
            animator.Play("JumpThrow");
            return;
        }
    }
    protected override void JumpAttack()
    {
        if (JoystickInput.Attack == false)
        {
            currentState = State.JUMP;
            animator.Play("JumpUp");
        }
    }

    protected override void Climb()
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
    
    protected override void JumpThrow()
    {
        if (JoystickInput.Throw == false)
        {
            currentState = State.JUMP;
            animator.Play("JumpUp");
        }
    }
    protected override void Glide()
    {
        if (JoystickInput.Glide == false)
            IdleAction();
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
    protected override void EndClimb()
    {
        Landing();
    }
}
