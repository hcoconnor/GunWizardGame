using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_2D : MonoBehaviour
{

    public float speedX = 5;
    public float speedY = 5;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float deltaX = 0f;
        float deltaY = 0f;

        deltaX += Input.GetAxis("Horizontal");
        deltaY += Input.GetAxis("Vertical");

        deltaX = deltaX * speedX * Time.deltaTime;
        deltaY = deltaY * speedY * Time.deltaTime;

        transform.Translate(deltaX, deltaY, 0);

    }
}
