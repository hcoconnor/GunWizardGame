using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_2D : MonoBehaviour
{

    public float speedX = 5;
    public float speedY = 5;

    Rigidbody2D rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb= this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();

        
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

        //rb.AddForce(new Vector2(deltaX, deltaY));
        Vector2 delta = new Vector2(deltaX, deltaY);
        RaycastHit2D[] hits = new RaycastHit2D[1];
        ContactFilter2D contactFilter = new ContactFilter2D();

        contactFilter.layerMask = LayerMask.NameToLayer("RoomTrigger");
        if (rb.Cast(delta,contactFilter, hits, delta.magnitude) > 0)
        {
            delta = delta.normalized*hits[0].distance;
        }
        //Debug.Log(deltaX);
        transform.Translate(delta.x, delta.y, 0);
        sr.sortingOrder = (int)((-transform.position.y+.08f)*(SetLayer.SortingOrderMulti));
        //Debug.Log("Player: " + sr.sortingOrder+" "+ (-transform.position.y + .08f) * (100));

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("wizard collision: " + other.name);
        if (other.CompareTag("RoomTrigger"))
        {
            other.transform.parent.GetComponent<Room>().playerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RoomTrigger"))
        {
            other.transform.parent.GetComponent<Room>().playerExit();
        }
    }
}
