using LTAUnityBase.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WariorController : NormalController
{
    [SerializeField]
    GameObject kunai;
    [SerializeField]
    Transform kunaiPos;
    //public ButtonController BtnAttack, BtnThrow;
    protected override void Start()
    {
        base.Start();
        //BtnAttack.OnClick((ButtonController btn) =>
        //{
        //    JoystickInput.Attack = true;
        //});
        //BtnThrow.OnClick((ButtonController btn) =>
        //{
        //    JoystickInput.Throw = true;
        //});
    }

  

    protected override void Idle()
    {
        base.Idle();
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
    protected override void Run()
    {
        if (JoystickInput.Attack == true)
        {
            currentState = State.ATTACK;
            animator.Play("Attack");
            rigidbody2D.velocity = Vector2.zero;
            return;
        }
        base.Run();
    }
    
    protected override void Attack()
    {
        if (JoystickInput.Attack == false)
        {
            IdleAction();
        }
    }
    
    protected override void Throw()
    {
        if (JoystickInput.Throw == false)
            IdleAction();

    }
    public void ThrowKunai()
    {
        Instantiate(kunai, kunaiPos.position, kunaiPos.rotation);
    }
}
