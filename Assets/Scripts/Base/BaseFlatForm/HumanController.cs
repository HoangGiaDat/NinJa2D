using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public float speed;
    public Animator animator;

    protected virtual void IdleAction()
    {
        rigidbody2D.velocity = Vector3.zero;
        animator.Play("Idle");
    }
    protected virtual void MoveLeft()
    {
        rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        transform.eulerAngles = new Vector3(0, 180, 0);
       
    }
    protected virtual void MoveRight()
    {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        transform.eulerAngles = new Vector3(0, 0, 0);
        
    }
    protected virtual void JumpAction()
    {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 8);
        animator.Play("JumpUp");
    }

    protected virtual void SlideAction()
    {
        animator.Play("Slide");
        if (transform.eulerAngles.y == 180)
            rigidbody2D.velocity = new Vector2(-2 * speed, rigidbody2D.velocity.y);
        else
            rigidbody2D.velocity = new Vector2(2 * speed, rigidbody2D.velocity.y);
    }
    protected virtual void Dead()
    {

    }    
}
