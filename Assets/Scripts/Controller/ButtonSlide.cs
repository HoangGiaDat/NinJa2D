using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.UI;
public class ButtonSlide : ButtonController
{
    // Start is called before the first frame update
    void Start()
    {
        this.OnClick(OnClickBtn);
    }

    // Update is called once per frame
    void OnClickBtn(ButtonController btn)
    {
        JoystickInput.Slide = true;
    }
}
