using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BGDoors))]
public class Room : MonoBehaviour
{
    public int maxConnections = 3;
    public int minConnections = 3;

    public static LevelGen lg;

    [HideInInspector]
    public BGDoors bgDoors;

    [HideInInspector]
    public List<Room> adjRooms;

    [HideInInspector]
    public bool expanded;
    [HideInInspector]
    public bool playerInRoom;

    [HideInInspector]
    public float height;
    [HideInInspector]
    public float width;

    [HideInInspector]
    public BoxCollider2D roomTrigger;

    

    void Awake()
    {
        if (lg == null)
        {
            lg = transform.root.gameObject.GetComponentInChildren<LevelGen>();
        }
        bgDoors = GetComponent<BGDoors>();
        adjRooms = new List<Room>();
        expanded = false;
        playerInRoom = false;
        roomTrigger = transform.transform.Find("RoomTrigger").GetComponent<BoxCollider2D>();

        height = GetComponent<SpriteRenderer>().size.x * transform.localScale.x;
        width = GetComponent<SpriteRenderer>().size.y * transform.localScale.y;

    }

    

    public Door[] getConnectedDoor(Room room)
    {
        List<Door[]> doorPairs = new List<Door[]>();
        foreach(Door door in this.bgDoors.doors)
        {
            foreach(Door otherDoor in room.bgDoors.doors)
            {
                if (Door.canConnect(door, otherDoor))
                {
                    if (Door.isTopBottomDoor(door) && Door.isTopBottomDoor(otherDoor))
                    {
                        if (door.doorWall.size.y + otherDoor.doorWall.size.y >=
                            Vector3.Distance(door.transform.position, otherDoor.transform.position))
                        {
                            Door[] newDoors = { door, otherDoor };
                            doorPairs.Add(newDoors);
                        }
                    }
                    else
                    {
                        if (door.doorWall.size.x + otherDoor.doorWall.size.x >=
                            Vector3.Distance(door.transform.position, otherDoor.transform.position))
                        {
                            Door[] newDoors = { door, otherDoor };
                            doorPairs.Add(newDoors);
                        }
                    }
                }
                
            }
        }
        if(doorPairs.Count > 0)
        {

            return doorPairs[Random.Range(0, doorPairs.Count - 1)]; 
        }
        return null;
    }

    bool isConnected(Room other)
    {
        return this.adjRooms.Find(x => x == other) != null;
    }

    static void connectDoors(Door door1, Door door2)
    {
        door1.setConnectingDoor(door2);
        door1.setDoor(true);

        door2.setConnectingDoor(door1);
        door2.setDoor(true);
    }

    public bool connectRooms(Room other)
    {
        Door[] doors = getConnectedDoor(other);
        if(this.isConnected(other) && doors != null)
        {
            return false;
        }
        connectDoors(doors[0], doors[1]);
        this.adjRooms.Add(other);
        other.adjRooms.Add(this);
        return true;
    }
    

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.transform.parent.name+" "+collision.transform.position+" "+this.transform.position);
    //    if (collision.CompareTag("Player")){
    //        playerInRoom = true;
    //        lg.expandRoom(this);
    //    }
    //}

    
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        playerInRoom = false;
    //    }


    //}


    public void playerEnter()
        {
            if(this.adjRooms.Count <= 0)
            {
                playerInRoom = true;
                lg.expandRoom(this);
            }
           
        }
    public void playerExit()
    {
        if (this.adjRooms.Count <= 0)
        {
            playerInRoom = false;
        }
    }

}
