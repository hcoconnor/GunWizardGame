using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunControl : MonoBehaviour
{

    public GameObject Bullet;
    public Transform aimRotate;
    public float coolDownMax = 1;

    private float coolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Gun") && coolDown <= 0)
        {
            Instantiate(Bullet,transform.position, aimRotate.rotation);
            coolDown = coolDownMax;
        }

        coolDown = Mathf.Max(0, coolDown - Time.deltaTime);
    }
}
