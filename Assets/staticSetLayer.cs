using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class staticSetLayer : MonoBehaviour
{

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)((-transform.position.y + .08f) * (100));
        //Debug.Log("Box: " + sr.sortingOrder);
    }

    
}
