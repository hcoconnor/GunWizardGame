using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialRoom : Room
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void playerEnter()
    {

        //transform.Find("Fog").GetComponent<Animator>().SetTrigger("PlayerEnter");
        playerInRoom = true;
        StartCoroutine(lg.expandRoom(this));

    }
}
