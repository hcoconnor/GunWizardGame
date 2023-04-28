using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetLayer : MonoBehaviour
{

    public bool isStatic = false;
    public float originOffset = .08f;

    SpriteRenderer sr;
    public static float SortingOrderMulti = 100;
    

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)((-transform.position.y + originOffset) * (SortingOrderMulti));
        //Debug.Log("Box: " + sr.sortingOrder);
    }

    private void Update()
    {
        if (!isStatic)
        {
            sr.sortingOrder = (int)((-transform.position.y + originOffset) * (SortingOrderMulti));
        }
    }


}
