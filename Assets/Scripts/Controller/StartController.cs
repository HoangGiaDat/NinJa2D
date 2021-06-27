using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.UI;
public class StartController : MonoBehaviour
{
    [SerializeField]
    ButtonController BtnPlay;
    void Start()
    {
        BtnPlay.OnClick((ButtonController btn) =>
        {
            SceneController.OpenScene(AllSceneName.GamePlay);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
