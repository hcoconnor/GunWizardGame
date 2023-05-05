
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


    [System.Serializable]
    public struct ObjectQuantityPair
    {
        public GameObject name;
        public int quantity;
    }
    public ObjectQuantityPair[] populateValues;

    protected Dictionary<GameObject, int> populations;



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

        maxConnections = Mathf.Clamp(maxConnections, 1, bgDoors.doors.Count);
        minConnections = Mathf.Clamp(minConnections, 0, maxConnections);


        populations = new Dictionary<GameObject, int>();

        foreach(ObjectQuantityPair pair in populateValues)
        {
            populations.Add(pair.name, pair.quantity);
        }

    }

    

    public Door[] getConnectedDoor(Room room)
    {
        List<Door[]> doorPairs = new List<Door[]>();
        foreach(Door door in this.bgDoors.doors)
        {
            foreach(Door otherDoor in room.bgDoors.doors)
            {
                if (Door.canConnect(door, otherDoor)&& !door.sr.enabled && !otherDoor.sr.enabled)
                {

                    if (Door.isTopBottomDoor(door) && Door.isTopBottomDoor(otherDoor))
                    {
                        if (door.doorWall.size.y + otherDoor.doorWall.size.y +.01f>=
                            Vector3.Distance(door.transform.position, otherDoor.transform.position))
                        {
                            Door[] newDoors = { door, otherDoor };
                            doorPairs.Add(newDoors);
                        }
                    }
                    else
                    {
                        if (door.doorWall.size.x + otherDoor.doorWall.size.x +.01f>=
                            Vector3.Distance(door.transform.position, otherDoor.transform.position))
                        {
                            Door[] newDoors = { door, otherDoor };
                            doorPairs.Add(newDoors);
                        }
                    }
                }
                else
                {
                    //if(door.sr.enabled || otherDoor.sr.enabled)
                    //Debug.Log("Ignoring disabled Door");
                }
                
            }
        }
        if(doorPairs.Count > 0)
        {

            return doorPairs[Random.Range(0, doorPairs.Count)]; 
        }
        return null;
    }

    protected bool isConnected(Room other)
    {
        return this.adjRooms.Find(x => x == other) != null;
    }

    protected void connectDoors(Door door1, Door door2,Room room2)
    {
        door1.setConnectingDoor(door2);
        door1.setDoor(true);

        door2.setConnectingDoor(door1);
        door2.setDoor(true);

        //this.bgDoors.doors.Remove(door1);
        //room2.bgDoors.doors.Remove(door2);

    }

    public bool connectRooms(Room other)
    {
        Door[] doors = getConnectedDoor(other);
        if(this.isConnected(other) && doors != null)
        {
            return false;
        }
        //Debug.Log(this.bgDoors.doors.Count);
        //Debug.Log("," + other.bgDoors.doors.Count);
        //Debug.Log("," + doors);
        //Debug.Log("," + doors[1]);
        //Debug.Log("," + doors[0]);
            
            
        //Debug.Log((this == null) + "," + (doors[0] == null) + "," + (doors[1] == null) + "," + (other == null));
        //Debug.Log(this+","+doors[0] + "," + doors[1] + "," + other);
        //Debug.Log((this==null) + "," +( doors[0] == null) + "," +( doors[1] == null) +"," +( other == null) );

        connectDoors(doors[0], doors[1], other);
        this.adjRooms.Add(other);
        other.adjRooms.Add(this);
        return true;
    }


    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.transform.parent.name + " " + collision.transform.position + " " + this.transform.position);
    //    if (collision.CompareTag("Player"))
    //    {
    //        playerInRoom = true;
    //        lg.expandRoom(this);
    //    }
    //}
    private void Update()
    {
        //RaycastHit2D result = Physics2D.BoxCast(this.transform.position, this.roomTrigger.size, 0, Vector2.zero, 0f, LayerMask.GetMask("RoomTrigger"));
        Debug.DrawLine((this.transform.position) + (Vector3)this.roomTrigger.offset + new Vector3(this.roomTrigger.size.x, this.roomTrigger.size.y, 0),
                        (this.transform.position) + (Vector3)this.roomTrigger.offset - new Vector3(this.roomTrigger.size.x, this.roomTrigger.size.y, 0),
                        Color.green, 0);
        //if (result.collider != this.roomTrigger)
        //{
        //    //Debug.Log(this.name +" "+result.collider.transform.parent.name);
        //}
    }


    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        playerInRoom = false;
    //    }


    //}

    protected IEnumerator populate(GameObject obj, int num)
    {
        float timeSinceStart = 0;

        while(num > 0 || timeSinceStart == 5)
        {
            GameObject newObj = Instantiate(obj, this.transform.Find("Populations"));
            CapsuleCollider2D newObjCap = newObj.GetComponent<CapsuleCollider2D>();

            do
            {
                //Debug.Log(-(roomTrigger.size.y - newObjCap.size.y - newObjCap.offset.y)+","+ (roomTrigger.size.y - newObjCap.size.y - newObjCap.offset.y));
                newObj.transform.localPosition = new Vector3(
                    Random.Range(-(roomTrigger.size.x - newObjCap.size.x - newObjCap.offset.x), (roomTrigger.size.x - newObjCap.size.x - newObjCap.offset.x)),
                    Random.Range(-(roomTrigger.size.y - newObjCap.size.y - newObjCap.offset.y), (roomTrigger.size.y - newObjCap.size.y - newObjCap.offset.y)),
                    0);
                yield return null;
                timeSinceStart -= Time.deltaTime;
            } while (Physics2D.CapsuleCast((Vector2)newObj.transform.position + newObjCap.offset, newObjCap.size, newObjCap.direction, 0, Vector2.zero, 0f, LayerMask.GetMask("Room")));

            num--;

        }
        yield return null;
    }

    public void populate()
    {
        foreach(KeyValuePair<GameObject,int> entry in populations)
        {
            StartCoroutine(populate(entry.Key, entry.Value));
        }
    }


    virtual public void playerEnter()
    {
            
        transform.Find("Fog").GetComponent<Animator>().SetTrigger("PlayerEnter");
        playerInRoom = true;
        StartCoroutine( lg.expandRoom(this));
           
    }
    public void playerExit()
    {
        if (this.adjRooms.Count <= 0)
        {
            
        }
        playerInRoom = false;
    }

}
