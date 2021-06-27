using LTAUnityBase.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalController : HumanController
{
    SpriteRenderer spriteRenderer;
    public CircleCollider2D boxCollider2D;
    //public ButtonController BtnSlide;

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
    public State currentState = State.IDLE;

    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<CircleCollider2D>();
        //BtnSlide.OnClick((ButtonController btn) =>
        //{
        //    JoystickInput.Slide = true;
        //});
    }

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



    protected override void JumpAction()
    {
        currentState = State.JUMP;
        base.JumpAction();
    }


    protected override void SlideAction()
    {
        currentState = State.SLIDE;
        base.SlideAction();

        StartCoroutine(StopSliding());
    }
    protected override void IdleAction()
    {
        base.IdleAction();
        currentState = State.IDLE;
    }
    public void Landing()
    {
        if (rigidbody2D.velocity.y == 0)
        {
            JoystickInput.Glide = false;
            IdleAction();
            rigidbody2D.gravityScale = 1f;
        }
    }
    public void EndAttack()
    {
        JoystickInput.Attack = false;
    }

    public void EndThrow()
    {
        JoystickInput.Throw = false;
    }


    IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(0.5f);
        JoystickInput.Slide = false;
        IdleAction();
    }


    protected virtual void Idle()
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
       


    }
    protected virtual void Run()
    {
        
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
            animator.Play("Run");
            return;
        }
        if (JoystickInput.Left)
        {
            MoveLeft();
            animator.Play("Run");
            return;
        }
        IdleAction();
    }
    protected virtual void Jump()
    {

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
    protected virtual void Slide()
    {
        if (JoystickInput.Slide == false)
            IdleAction();
    }

    protected virtual void Attack()
    {
       
    }

    protected virtual void JumpAttack()
    {
       
    }
    protected virtual void EndClimb()
    {

    }
    protected virtual void Glide()
    {

    }
    protected virtual void Climb()
    {

    }
    protected virtual void Throw()
    {

    }

    protected virtual void JumpThrow()
    {

    }
    protected virtual void ClimbAction()
    {

    }

}
