using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BGDoors))]
public class BGDoorEditor : Editor
{

    public List<BoxCollider2D> bx;

    void Start()
    {
        ((BGDoors)target).GetComponents<BoxCollider2D>(bx);

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();
        BGDoors doorsScript = (BGDoors)target;
        if (GUILayout.Button("Reset Wall Colliders"))
        {
            resetWalls(doorsScript);
            Debug.Log("reset Walls");
        }
        if (GUILayout.Button("SetUp Wall Colliders"))
        {

            setupDoors(doorsScript);
            Debug.Log("set up Walls");
        }

    }

    void resetWalls(BGDoors doorsScript)
    {
        BoxCollider2D[] walls = doorsScript.GetComponents<BoxCollider2D>();

        foreach(BoxCollider2D wall in walls)
        {
            if(doorsScript.walls.IndexOf(wall) >= 0)
            {
                // if wall real wall
                wall.enabled = true;
            }
            else
            {
                DestroyImmediate(wall);
            }
        }
    }

    void setupDoors(BGDoors doorsScript)
    {
        foreach(Door door in doorsScript.doors)
        {
            //Debug.Log("Door");
            door.Start();
            foreach (BoxCollider2D doorCol in door.bx)
            {
                //Debug.Log("doorCol");
                doorCol.enabled = true;
                //Debug.Log((Vector2)door.transform.localPosition);

                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(~LayerMask.NameToLayer("Room"));
                List<Collider2D> walls = new List<Collider2D>();
                doorCol.OverlapCollider(filter,walls);

                //Debug.Log(walls.Count);


                foreach(BoxCollider2D wall in walls)
                {

                    
                    //Debug.Log("wall" +wall.offset+" "+wall.size);
                    
                    //Debug.Log("1  "+firstWall.size.x);
                    //Debug.Log(door.sr.sprite.name+" "+BGDoors.spriteName.Replace("&n", BGDoors.sideToNum["top"])+" ");
                    if (door.sr.sprite.name.Equals(BGDoors.spriteName.Replace("&n",BGDoors.sideToNum["top"])) ||
                        door.sr.sprite.name.Equals(BGDoors.spriteName.Replace("&n",BGDoors.sideToNum["bottom"])))
                    {
                        if(doorCol.size.x < doorCol.size.y)
                        {
                            //not horizontal
                            doorCol.enabled = door.sr.enabled;
                            Debug.Log("skipped");
                            continue;
                        }

                        //horizontal wall

                        BoxCollider2D firstWall = doorsScript.gameObject.AddComponent<BoxCollider2D>();
                        BoxCollider2D secondWall = doorsScript.gameObject.AddComponent<BoxCollider2D>();


                        //Debug.Log(wall.offset +"-"+ (doorCol.offset + (Vector2)door.transform.localPosition) + " "+(wall.offset - (doorCol.offset + (Vector2)door.transform.localPosition)));
                        //Debug.Log((doorCol.offset.x + door.transform.localPosition.x) + doorCol.size.x + " " + wall.offset.y);


                        firstWall.size = new Vector2((wall.size.x / 2) - ((doorCol.size.x / 2) + (wall.offset.x - doorCol.offset.x - door.transform.localPosition.x)),     //x
                            wall.size.y);                                                                                                                                   //y
                        //Debug.Log(firstWall.size);

                        firstWall.offset = new Vector2(-(wall.size.x/2)+firstWall.size.x/2, //x
                            wall.offset.y);                                                 //y
                        //Debug.Log(firstWall.offset+" ");

                        secondWall.size = new Vector2(wall.size.x - (firstWall.size.x + doorCol.size.x),              //x
                            wall.size.y);                                                                             //y
                        //Debug.Log(secondWall.size);

                        secondWall.offset = new Vector2(-(-doorCol.offset.x - door.transform.localPosition.x) + (doorCol.size.x/2) + (secondWall.size.x /2) ,  //x
                            wall.offset.y);                                                                                                                     //y
                        //Debug.Log(secondWall.offset);   
                        
                        

                    }
                    else
                    {

                        if (doorCol.size.x > doorCol.size.y )
                        {
                            //not vertical
                            doorCol.enabled = door.sr.enabled;
                            Debug.Log("skipped");
                            continue;
                        }

                        //vertical wall

                        BoxCollider2D firstWall = doorsScript.gameObject.AddComponent<BoxCollider2D>();
                        BoxCollider2D secondWall = doorsScript.gameObject.AddComponent<BoxCollider2D>();


                        firstWall.size = new Vector2(wall.size.x,     //x
                            (wall.size.y / 2) - ((doorCol.size.y / 2) + (wall.offset.y - doorCol.offset.y - door.transform.localPosition.y)));                                    //y
                       // Debug.Log(firstWall.size);

                        firstWall.offset = new Vector2(wall.offset.x, //x
                            -(wall.size.y / 2) + firstWall.size.y / 2);                                                 //y
                        //Debug.Log(firstWall.offset+" ");

                        secondWall.size = new Vector2(wall.size.x,              //x
                            wall.size.y - (firstWall.size.y + doorCol.size.y));                                                                             //y
                        //Debug.Log(secondWall.size);

                        secondWall.offset = new Vector2(wall.offset.x,  //x
                            -(-doorCol.offset.y - door.transform.localPosition.y) + (doorCol.size.y / 2) + (secondWall.size.y / 2));                                            //y
                        //Debug.Log((doorCol.offset.y) +" "+ door.transform.localPosition.y + " " + (doorCol.size.y / 2) + " " + (secondWall.size.y / 2));
                        //Debug.Log(secondWall.offset);  

                    }

                    doorCol.enabled = !door.sr.enabled;

                    ////Debug.Log(wall.offset +"-"+ (doorCol.offset + (Vector2)door.transform.localPosition) + " "+(wall.offset - (doorCol.offset + (Vector2)door.transform.localPosition)));
                    //firstWall.offset = wall.offset;
                    //firstWall.size = wall.offset - (doorCol.offset+(Vector2)door.transform.localPosition); //+door.trans.pos

                    //secondWall.offset = (doorCol.offset + (Vector2)door.transform.localPosition) + doorCol.size;
                    //secondWall.size = wall.size - (firstWall.size + doorCol.size);

                    //if (doorsScript.walls.IndexOf(wall) >= 0)
                    //{
                    //    //if wall is main wall
                    //    wall.enabled = false;

                    //}
                    //else
                    //{
                    //    //else not real wall;
                    //    DestroyImmediate(wall);
                    //}





                }
                for(int x = walls.Count-1; x >= 0; x--)
                {
                    if (doorsScript.walls.IndexOf((BoxCollider2D)walls[x]) >= 0)
                    {
                        //if wall is main wall
                        walls[x].enabled = false;

                    }
                    else if (door.bx.Contains((BoxCollider2D)walls[x]))
                    {

                    }
                    else
                    {
                        //else not real wall;
                        DestroyImmediate((BoxCollider2D)walls[x]);
                    }
                }


                
            }
            
             

        }
    }

}
