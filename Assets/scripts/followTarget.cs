using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTarget : MonoBehaviour
{

    public Transform target;
    [Range(0f, 50f)]
    public float XMargin;
    [Range(0f, 50f)]
    public float YMargin;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float diffX = target.position.x - transform.position.x;
        float diffY = target.position.y - transform.position.y;
        Vector3 deltaCam = Vector3.zero;


        //Debug.Log(deltaCam +" "+diffX);
        if(Mathf.Abs(diffX) > ((100 - XMargin)/100) * transform.localScale.x)
        {
            deltaCam.x = diffX - (Mathf.Sign(diffX)*(((100 - XMargin) / 100) * transform.localScale.x));
        }
        if (Mathf.Abs(diffY) > ((50 - YMargin) / 100) * transform.localScale.y)
        {
            deltaCam.y = diffY- (Mathf.Sign(diffY) * (((50 - YMargin) / 100) * transform.localScale.y));
        }

        transform.Translate(deltaCam);

    }

    
}
