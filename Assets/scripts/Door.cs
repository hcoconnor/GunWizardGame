using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sr;
    
    public BoxCollider2D doorWall; 
    public List<BoxCollider2D> bx;

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //GetComponents(bx);
        

    }

    

    public void toggleDoor()
    {
        sr.enabled = !sr.enabled;
        doorWall.enabled = !doorWall.enabled;
        //for(int x = 0; x < bx.Count;x++)
        //{
        //    bx[x].enabled = !bx[x].enabled;
        //    Debug.Log(x + "  toggled");
        //}
        foreach(BoxCollider2D box in bx)
        {
            bx.Find(x => x == box).enabled = !box.enabled;
        }
        foreach(Transform trans in GetComponentsInChildren<Transform>(true))
        {
            if(trans.gameObject == this.gameObject)
            {
                continue;
            }
            trans.gameObject.SetActive(!trans.gameObject.activeSelf);
        }
        
        
    }

   
}
