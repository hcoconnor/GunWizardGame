using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingColliderSetUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        gameObject.layer = LayerMask.NameToLayer("Targetable");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
