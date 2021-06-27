using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.UI;

public class ButtonAttack : ButtonController
{
   
    void Start()
    {
        this.OnClick(OnClickBtn);
    }

    
    void OnClickBtn(ButtonController btn)
    {
        JoystickInput.Attack = true;
    }
}
