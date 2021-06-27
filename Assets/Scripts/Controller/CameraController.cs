using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.Controller;
public class CameraController : BehaviourController
{
    [SerializeField]
    Transform player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float posy = Mathf.Clamp(player.position.y, 0f, 6.9f);
        float posx = Mathf.Clamp(player.position.x, -10f, 2.9f);
        Vector3 newpos = new Vector3(posx,posy,-10);
        MoveUpdate(newpos);
    }
}


