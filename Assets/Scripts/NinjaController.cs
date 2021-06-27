using LTAUnityBase.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CircleCollider2D boxCollider2D;
    public enum State
    {
        RUN,
        JUMP,
        SLIDE,
        CLIMB,
        GLIDE,
        ATTACK,
        THROW
    }
    State currentState = State.RUN;
    [SerializeField]
    GameObject kunai;

    [SerializeField]
    Transform kunaiPos;
    [SerializeField]
    float speed;
    bool isJumping = false;
    bool isGlide = false;
    bool isAttacking = false;
    bool isSliding = false;
    bool isThrowing = false;

    [SerializeField]
    bool isCanClimb = false;
    [SerializeField]
    bool isClimbing = false;
    bool isEndClimbing = false;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<CircleCollider2D>();
       
    }

    void Update()
    {
        switch (currentState)
        {
            case State.RUN:
                Run();
                break;
        }

        if (currentState == State.RUN)
        {
            if (isSliding && transform.eulerAngles.y == 0)
            {
                isSliding = false;
                animator.SetBool("isSlide", false);
                StopAllCoroutines();
            }
            if (isSliding)
            {
                return;
            }

            if (transform.eulerAngles.y == 180)
            {
                if (!isAttacking && !isThrowing)
                {
                    rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
                    animator.SetBool("isRun", true);

                }
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
            //spriteRenderer.flipX = true;
        }
        else if (currentState == State.JUMP)
        {
            if (!isJumping && rigidbody2D.velocity.y == 0)
            {
                Jump(10);
            }
        }
        else if (currentState == State.SLIDE)
        {
            isSliding = true;
            animator.SetBool("isSlide", true);
            StartCoroutine(IeStopSliding());
        }

        if (currentState == State.CLIMB)
        {
            isClimbing = true;
            rigidbody2D.gravityScale = 0;
            animator.Play("Climb");
            boxCollider2D.isTrigger = true;
            return;
        }
        if (currentState == State.GLIDE)
        {
            if (!isGlide && rigidbody2D.velocity.y < 5)
            {
                isGlide = true;
                animator.SetBool("isGlide", true);
                rigidbody2D.gravityScale = 0.5f;
                rigidbody2D.velocity = new Vector2(-rigidbody2D.velocity.x, 3);
                return;
            }
        }

        if (currentState == State.ATTACK)
        {
            if (isJumping && rigidbody2D.velocity.y == 0)
            {
                return;
            }
            isAttacking = true;
            if (isSliding)
            {
                isSliding = false;
                animator.SetBool("isSlide", false);
                StopAllCoroutines();
            }
            animator.SetBool("isAttack", true);
            if (isGlide)
            {
                rigidbody2D.gravityScale = 1.5f;
                animator.SetBool("isGlide", false);
            }
        }
        if (currentState == State.THROW)
        {

        }

        if (!isCanClimb && isClimbing)
        {
            isEndClimbing = true;
            isClimbing = false;
            rigidbody2D.gravityScale = 1.5f;
            Jump(5);
            boxCollider2D.isTrigger = false;
            animator.Play("Idle");
        }

        if (isClimbing)
        {
            Climb();
            return;
        }

        if (isCanClimb && Input.GetKey(KeyCode.UpArrow))
        {
            isClimbing = true;
            rigidbody2D.gravityScale = 0;
            animator.Play("Climb");
            boxCollider2D.isTrigger = true;
            return;
        }


        if (isJumping || rigidbody2D.velocity.y <= 0)
            animator.SetFloat("height", rigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {

            //if (!isJumping && rigidbody2D.velocity.y == 0)
            //{
            //    Jump(10);
            //}
            //else if (!isGlide && rigidbody2D.velocity.y < 5)
            //{
            //    isGlide = true;
            //    animator.SetBool("isGlide", true);
            //    rigidbody2D.gravityScale = 0.5f;
            //    rigidbody2D.velocity = new Vector2(-rigidbody2D.velocity.x, 3);
            //    return;
            //}
        }
        if (rigidbody2D.velocity.y == 0)
        {
            if (isEndClimbing)
            {
                isEndClimbing = false;
                return;
            }

            if (isJumping)
            {
                isJumping = false;
            }
            if (isAttacking)
            {
                EndAttack();
                return;
            }
            if (isThrowing)
            {
                EndThrow();
                return;
            }
            if (isGlide)
            {
                rigidbody2D.gravityScale = 1.5f;
                animator.SetBool("isGlide", false);
                isGlide = false;
            }

        }

        if (rigidbody2D.velocity.x == 0 && isSliding)
        {
            isSliding = false;
            animator.SetBool("isSlide", false);
            StopAllCoroutines();
        }

        if (Input.GetKey(KeyCode.V) && !isThrowing)
        {
            isThrowing = true;
            if (isSliding)
            {
                isSliding = false;
                animator.SetBool("isSlide", false);
                StopAllCoroutines();
            }
            animator.SetBool("isThrow", true);
            if (isGlide)
            {
                rigidbody2D.gravityScale = 1.5f;
                animator.SetBool("isGlide", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isSliding && rigidbody2D.velocity.y == 0 && !isJumping && !isAttacking)
        {
            //isSliding = true;
            //animator.SetBool("isSlide", true);
            //StartCoroutine(IeStopSliding());
        }

        if (Input.GetKey(KeyCode.C) && !isAttacking)
        {
            //if (isJumping && rigidbody2D.velocity.y == 0)
            //{
            //    return;
            //}
            //isAttacking = true;
            //if (isSliding)
            //{
            //    isSliding = false;
            //    animator.SetBool("isSlide", false);
            //    StopAllCoroutines();
            //}
            //animator.SetBool("isAttack", true);
            //if (isGlide)
            //{
            //    rigidbody2D.gravityScale = 1.5f;
            //    animator.SetBool("isGlide", false);
            //}

        }

        if (Input.GetKey(KeyCode.LeftArrow) && !isEndClimbing)
        {
            ////if (isAttacking)
            ////{
            ////    EndAttack();
            ////    return;
            ////}
            //if (isSliding && transform.eulerAngles.y == 0)
            //{
            //    isSliding = false;
            //    animator.SetBool("isSlide", false);
            //    StopAllCoroutines();
            //}
            //if (isSliding)
            //{
            //    return;
            //}

            //if (transform.eulerAngles.y == 180)
            //{
            //    if (!isAttacking && !isThrowing)
            //    {
            //        rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
            //        animator.SetBool("isRun", true);

            //    }
            //}
            //transform.eulerAngles = new Vector3(0, 180, 0);
            ////spriteRenderer.flipX = true;
            //return;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !isEndClimbing)
        {
            //if (isAttacking)
            //{
            //    EndAttack();
            //    return;
            //}
            if (isSliding && transform.eulerAngles.y == 180)
            {
                isSliding = false;
                if (!isAttacking)
                    animator.SetBool("isSlide", false);
            }
            if (isSliding)
            {
                return;
            }


            if (transform.eulerAngles.y == 0)
            {

                if (!isAttacking && !isThrowing)
                {
                    rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
                    animator.SetBool("isRun", true);
                }

            }
            transform.eulerAngles = new Vector3(0, 0, 0);
            //spriteRenderer.flipX = false;
            return;
        }

        if (!isSliding)
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            animator.SetBool("isRun", false);
        }

    }

    void Climb()
    {
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

    void Jump(float height)
    {
        isJumping = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, height);
    }
    
    // Update is called once per frame
   

    IEnumerator IeStopSliding()
    {
        yield return new WaitForSeconds(1f);
        isSliding = false;
        animator.SetBool("isSlide", false);
    }

    public void StartSlide()
    {
        if (transform.eulerAngles.y == 180)
            rigidbody2D.velocity = new Vector2(-2 * speed, rigidbody2D.velocity.y);
        else
            rigidbody2D.velocity = new Vector2(2 * speed, rigidbody2D.velocity.y);
    }

    public void EndAttack()
    {
        animator.SetBool("isAttack", false);
        isAttacking = false;
    }

    public void EndThrow()
    {
        animator.SetBool("isThrow", false);
        isThrowing = false;
    }

    public void ThrowKunai()
    {
        Instantiate(kunai, kunaiPos.position, kunaiPos.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            isCanClimb = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climb")
            isCanClimb = false;
    }

    void Run()
    {

    }
}
