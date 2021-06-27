using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    ButtonAttack btnAttack;
    ButtonSlide btnSlide;
    ButtonThrow btnThrow;
    void Start()
    {
        btnAttack = FindObjectOfType<ButtonAttack>();
        btnSlide = FindObjectOfType<ButtonSlide>();
        btnThrow = FindObjectOfType<ButtonThrow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
