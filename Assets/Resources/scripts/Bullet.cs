using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    
    public float speed = 10f;
    SpriteRenderer sr;
    public int bulletDamage;

    public void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,speed * Time.deltaTime,0);
        sr.sortingOrder = (int)-transform.position.y;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(!collision.collider.gameObject == player)
        //{

        //}
        ObjectStats stats = collision.gameObject.GetComponent<ObjectStats>();
        if(stats != null)
        {
            stats.hurt( bulletDamage,DamageType.physical);
        }
        Destroy(gameObject);
    }
    
}
