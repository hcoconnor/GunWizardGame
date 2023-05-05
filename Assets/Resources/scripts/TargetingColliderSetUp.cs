using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingColliderSetUp : MonoBehaviour
{

    public float originOffset = 0;
    public static float SortingOrderScale = 100;
    //public static Animator slowTimeAnim;
    SpriteRenderer sr;

    string SL;
    // Start is called before the first frame update
    void Start()
    {
        sr = transform.parent.GetComponent<SpriteRenderer>();
        //sr.sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        //gameObject.layer = LayerMask.NameToLayer("Targetable");
        SL = sr.sortingLayerName;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (slowTime.isSlow)
        {
            //sr.sortingOrder = Mathf.Max(10000,sr.sortingOrder);
            sr.sortingLayerName = "SlowTime";
        }
        else
        {
            //sr.sortingOrder = Mathf.Min(0, sr.sortingOrder);
            sr.sortingLayerName = SL;
        }
    }
}
