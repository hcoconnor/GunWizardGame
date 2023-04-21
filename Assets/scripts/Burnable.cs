using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectStats))]
public class Burnable : MonoBehaviour
{

    public bool onFire = false;
    //[SerializeField]
    public static GameObject  prefab;
    GameObject effect;
    [HideInInspector]
    public ObjectStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<ObjectStats>();
        if (prefab == null)
        {
            prefab = (GameObject)Resources.Load("Prefab/Fire", typeof(GameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire && effect == null)
        {
            effect = Instantiate(prefab, transform);
        }
        else if (!onFire && effect != null)
        {
            Destroy(effect);
            effect = null;
        }
    }
}
