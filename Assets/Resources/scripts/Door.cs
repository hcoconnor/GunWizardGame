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

    public Door connectingDoor;
    public Transform connectingNavPoint;

    [HideInInspector]
    public static string spriteName = "DungeonBG_Doors&n_0";
    [HideInInspector]
    public static Dictionary<string, string> sideToNum = new Dictionary<string, string>()
    {
        {"top","0" },
        {"right","1" },
        {"bottom","2" },
        {"left","3" },
    };
    [HideInInspector]
    public static Dictionary<string, string> sideToConnectingNum = new Dictionary<string, string>()
    {
        {"top",sideToNum["bottom"] },
        {"right",sideToNum["left"] },
        {"bottom",sideToNum["top"] },
        {"left",sideToNum["right"] }
    };

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //GetComponents(bx);
        

    }

    public static bool isTopBottomDoor(Door door)
    {
        return door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToNum["top"])) ||
                    door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToNum["bottom"]));
    }

    public static string getSide(Door door)
    {
        foreach(KeyValuePair<string,string> entry in Door.sideToNum)
        {
            if(door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", entry.Value)))
            {
                return entry.Key;
            }
                
        }
        return null;
    }

    public static bool canConnect(Door door1, Door door2)
    {
        string side = getSide(door1);
        return door2.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToConnectingNum[side]));


        
    }

    public static float spaceBetweenDoors(Door door1, Door door2)
    {
        if (!canConnect(door1, door2))
        {
            return -1;
        }

        if (isTopBottomDoor(door1))
        {
            //return Mathf.Abs((Mathf.Sign(door1.transform.position.y - door2.transform.position.y) * door2.doorWall.offset.y) + door2.doorWall.size.y) +
            //    Mathf.Abs((Mathf.Sign(door2.transform.position.y - door1.transform.position.y) * door1.doorWall.offset.y) + 
            //    );

            return .09f;

                //(Mathf.Sign(door2.doorWall.offset.y) * (door2.doorWall.offset.y + door2.doorWall.size.y)) +
                //(Mathf.Sign(door1.doorWall.offset.y) * (door1.doorWall.offset.y + door2.doorWall.size.y));
        }
        else
        {
            //return Mathf.Abs((Mathf.Sign(door1.transform.position.x - door2.transform.position.x) * door2.doorWall.offset.x) + door2.doorWall.size.x) +
            //    Mathf.Abs((Mathf.Sign(door2.transform.position.x - door1.transform.position.x) * door1.doorWall.offset.x) + door1.doorWall.size.x);

            return .04f;
        }
        //Debug.Log(door1.transform.position + "," + door2.transform.position);
        //return Vector2.Distance(door1.transform.position, door2.transform.position);



    }
    public void setConnectingDoor(Door otherDoor)
    {
        this.connectingDoor = otherDoor;
        this.connectingNavPoint = otherDoor.transform.Find("PathFindPoint");
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

    public void setDoor(bool active)
    {
        sr.enabled = active;
        doorWall.enabled = !active;
        //for(int x = 0; x < bx.Count;x++)
        //{
        //    bx[x].enabled = !bx[x].enabled;
        //    Debug.Log(x + "  toggled");
        //}
        //walls on door
        foreach(BoxCollider2D box in bx)
        {
            bx.Find(x => x == box).enabled = active;
        }
        //forgrounds
        foreach(Transform trans in GetComponentsInChildren<Transform>(true))
        {
            if(trans.gameObject == this.gameObject)
            {
                continue;
            }
            if (Door.getSide(this).Equals("bottom"))
            {
                trans.gameObject.SetActive(!active);
            }
            else
            {
                trans.gameObject.SetActive(active);
            }
            
        }
    } 
      

   
}
