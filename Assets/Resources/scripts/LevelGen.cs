using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    static string roomPrefabsPath = "Prefab/Rooms";
    static GameObject[] roomPrefabs;

    List<Room> roomsToExpand;
    public Room initialRoom;


    float maxDistToOldRoom = 5f;
    float percentChanceOfOldRoom = 50f;

    IEnumerator GenerateRooms(Room thisRoom)
    {
        float timeSinceStart = 0;
        //Debug.Log("starting");
        if(thisRoom.adjRooms.Count > thisRoom.maxConnections || thisRoom.expanded)
        {
            //Debug.Log("oops");
            yield break;
        }

        //connect with previous rooms if space for connection and still space for new room
        if (thisRoom.adjRooms.Count < thisRoom.maxConnections - 1)
        {
            List<Room> potentialPrevious = new List<Room>();

            foreach (Room room in roomsToExpand)
            {
                if (Vector3.Distance(room.transform.position, thisRoom.transform.position) < maxDistToOldRoom && //if close enough
                    thisRoom.adjRooms.Find(x => x == room && x.adjRooms.Find(y => y == room)) == null &&   //and if room not adjacent and close to adjacent

                    thisRoom.getConnectedDoor(room) != null)
                {  //and rooms have connected door
                    potentialPrevious.Add(room);

                }

            }
            if (potentialPrevious.Count > 0)
            {
                Room oldRoom = potentialPrevious[Random.Range(0, potentialPrevious.Count)];
                float rnd = Random.Range(0, 100);

                //percentChanceOfOldRoom of connecting to old room
                if (rnd <= percentChanceOfOldRoom)
                {
                    thisRoom.connectRooms(oldRoom);
                    yield return null;
                    Debug.DrawRay(thisRoom.transform.position, -thisRoom.transform.position + oldRoom.transform.position, Color.red, 20);
                }

            }
        }



        while (thisRoom.adjRooms.Count < thisRoom.maxConnections && timeSinceStart < 1)
        {
            //Debug.Log("max");
            List<GameObject> roomsToTest = new List<GameObject>(roomPrefabs);
            while(roomsToTest.Count >= 0)
            {
                //Debug.Log("rooms");
                int rndInt = Random.Range(0, roomsToTest.Count);
                //Debug.Log("rand: " + rndInt+" "+ (roomsToTest.Count));
                Room newRoom = Instantiate(roomsToTest[rndInt], transform).GetComponent<Room>();
                //newRoom.name += Random.Range(0, 10000);
                //Debug.Log(newRoom);

                List<Vector3> possibleLocations = getPossibleLocations(thisRoom, newRoom);
                //Debug.Log(possibleLocations.Count);
                while(possibleLocations.Count > 0)
                {
                    //Debug.Log("posLoc");
                    Vector3 currentPossibleLocation = possibleLocations[Random.Range(0, possibleLocations.Count)];
                    //Debug.Log(currentPossibleLocation);
                    newRoom.transform.position = thisRoom.transform.position + currentPossibleLocation;


                    //check if overlapping another room

                    ContactFilter2D filter = new ContactFilter2D();
                    filter.layerMask = LayerMask.GetMask("RoomTrigger");
                    filter.useLayerMask = true;
                    filter.useTriggers = true;

                    //Debug.Log("FILTER: " + filter.IsFilteringLayerMask(newRoom.roomTrigger.gameObject));
                    //List<RaycastHit2D> results = new List<RaycastHit2D>();
                    //newRoom.roomTrigger.OverlapCollider(filter, results);
                    //foreach(Collider2D col in results)
                    //{
                    //    Debug.Log(col.name);
                    //}
                    //Debug.Log(results.Count);
                    //Debug.Log(results.Find(x => x.transform.parent.GetComponent<Room>().adjRooms.Count > 0));


                    //RaycastHit2D result = new RaycastHit2D();
                    List<RaycastHit2D> results = new List<RaycastHit2D>();
                    Physics2D.BoxCast((Vector2)(newRoom.transform.position)+newRoom.roomTrigger.offset, newRoom.roomTrigger.size*2, 0, Vector2.zero,filter, results,1f);
                    //Debug.DrawLine(newRoom.transform.position + new Vector3(newRoom.roomTrigger.size.x, newRoom.roomTrigger.size.y, 0),
                    //    newRoom.transform.position - new Vector3(newRoom.roomTrigger.size.x, newRoom.roomTrigger.size.y, 0),
                    //    Color.green, 20 );
                    RaycastHit2D result = results.Find(x => x.collider != newRoom.roomTrigger);
                    //Debug.Log(result+" "+results.Count);
                    

                    //!newRoom.roomTrigger.IsTouchingLayers(~LayerMask.NameToLayer("RoomTrigger"))
                    //( thisRoom.name == "Init" && thisRoom.adjRooms.Count == 0 ) || results.Find(x => x.GetComponent<Room>().adjRooms.Count > 0) == null
                    //&& result.collider.transform.parent != null && result.collider.transform.parent.GetComponent<Room>()
                    if (result.collider == null ){
                        //Debug.Log("not TOUCHING");
                        bool connectSuccess = thisRoom.connectRooms(newRoom);
                        if (connectSuccess)
                        {
                            //Debug.Log("yay: "+newRoom.transform.position);
                            //Debug.DrawRay(thisRoom.transform.position, -thisRoom.transform.position + newRoom.transform.position, Color.blue,20);
                            //successfull connected rooms
                            //Debug.Log("room added");
                            roomsToExpand.Add(newRoom);
                            yield return null;
                            break;
                        }
                    }
                    else
                    {
                        //result.collider.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
                        //Debug.Log("boo: "+newRoom.transform.position);
                        //Debug.Log("Space Not Available: "+result.collider.transform.position);
                    }
                    
                    possibleLocations.Remove(currentPossibleLocation);
                    //Debug.Log("posLoc end");
                } //cycle through all posible locations
                //Debug.Log("out posLoc"+possibleLocations.Count);
                yield return null;
                if (possibleLocations.Count > 0)
                {
                    //room connected
                    break;
                }
                else
                {
                    //room not conncted

                    roomsToTest.Remove(roomsToTest.Find(x => (x.name + "(Clone)").Equals(newRoom.name)));
                    Destroy(newRoom.gameObject);
                    //Debug.Log();
                    if(roomsToTest.Count <= 0)
                    {
                        break;
                    }
                }

            }//cycle through all possible rooms

            yield return null;
            timeSinceStart += Time.deltaTime;
            if(roomsToTest.Count <= 0)
            {
                //no room possible
                yield break;
            }

            //break;
            

        }

        yield break;
    }

    static List<Vector3> getPossibleLocations(Room room1, Room room2)
    {
        List<Vector3> possibleLocations = new List<Vector3>();
        //Debug.Log("|"+room1.bgDoors + " || " + room2.bgDoors+"|");
        foreach(Door door1 in room1.bgDoors.doors)
        {
            foreach (Door door2 in room2.bgDoors.doors)
            {


                if (!(door1.sr.enabled || door2.sr.enabled) && Door.canConnect(door1,door2))
                {
                    // if not enabled and door capable of connecting, then doors is available for connecting


                    float x = door1.transform.localPosition.x * room1.transform.localScale.x -
                            door2.transform.localPosition.x * room2.transform.localScale.x;

                    float y = door1.transform.localPosition.y * room1.transform.localScale.y -
                        door2.transform.localPosition.y * room2.transform.localScale.y;
                    if (Door.isTopBottomDoor(door1))
                    {
                        y += Mathf.Sign(y)*Door.spaceBetweenDoors(door1,door2);

                        //possibleLocations.Add(new Vector3(
                        //    room2.width / 2f,
                        //    (room1.height + room2.height) / 2f,
                        //    0));
                    }
                    else
                    {
                        x += Mathf.Sign(x) *  Door.spaceBetweenDoors(door1, door2);

                        //possibleLocations.Add(new Vector3(
                        //    (room1.width + room2.width) / 2f,
                        //    room1.height / 2f,
                        //    0));
                    }
                    //Debug.Log("Side: "+door1.gameObject.name+"+"+door2.gameObject.name
                    //    +"|x: " + x + "|y: " + y+"|sp: "+ Door.spaceBetweenDoors(door1, door2));

                    Vector3 newPosLoc = new Vector3(x, y, 0);
                    if (!possibleLocations.Contains(newPosLoc))
                    {
                        possibleLocations.Add(newPosLoc);
                    }
                    else
                    {
                        //Debug.Log("containts");
                    }
                    
                }
            }
        }

        return possibleLocations;
    }

    public IEnumerator expandRoom(Room room, int maxDist = 2)
    {
        Debug.Log("expanding: " + maxDist);
        if (!room.expanded)
        {
            yield return GenerateRooms(room);
            
        }
        if (maxDist > 0)
        {
            foreach (Room adjRoom in room.adjRooms)
            {
                yield return expandRoom(adjRoom, maxDist - 1);
            }
            //expandRoom(room.adjRooms[0], maxDist - 1);
            //yield return null;
            //expandRoom(room.adjRooms[1], maxDist - 1);
            room.expanded = true;
            roomsToExpand.Remove(room);
            yield return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        roomPrefabs = Resources.LoadAll<GameObject>(roomPrefabsPath);
        roomsToExpand = new List<Room>();

        int seed = Random.Range(0, 10000);
        Random.InitState(seed);
        Debug.Log("SEED: " + seed);
        //GenerateRooms(initialRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
