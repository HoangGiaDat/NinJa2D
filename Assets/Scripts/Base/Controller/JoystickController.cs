using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LTAUnityBase.Base.DesignPattern;

public class JoystickController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector3 direction;
    public Vector3 posJoyStick;
    [SerializeField]
    Transform JoyStick;
    [SerializeField]
    Transform BgJoyStick;
    Vector3 Originalpos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        BgJoyStick.position = eventData.position;
        direction = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveJoyStick(eventData.position);
    }

    void MoveJoyStick(Vector3 touchPos)
    {
        Vector2 offset = touchPos - BgJoyStick.position;
        Vector3 realdirection = Vector2.ClampMagnitude(offset, 70.0f);
        direction = realdirection.normalized;
        JoyStick.position = new Vector3(BgJoyStick.position.x + realdirection.x, BgJoyStick.position.y + realdirection.y  );
        posJoyStick = JoyStick.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        JoyStick.transform.localPosition = Vector3.zero;
        BgJoyStick.position = Originalpos;
        posJoyStick = Originalpos;
        direction = Vector3.zero;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Originalpos = BgJoyStick.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class JoyStick  : SingletonMonoBehaviour<JoystickController>
{

}