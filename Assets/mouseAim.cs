using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseAim : MonoBehaviour
{
    public Transform aimRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        float deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;

        float angle = Mathf.Atan((deltaY / deltaX)) * (180 / Mathf.PI) ;

        //Debug.Log(deltaX+" "+deltaY+" "+angle);

        if (deltaX < 0)
        {
            angle = angle + 90;
        }
        else
        {
            angle += 270;
        }

        aimRotate.eulerAngles = new Vector3(0, 0, angle);

    }
}
