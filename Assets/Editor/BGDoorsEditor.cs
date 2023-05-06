using System;
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

        SpriteRenderer[] oldSRs = doorsScript.transform.Find("ForgroundParent").GetComponentsInChildren<SpriteRenderer>();

        //rest first forground wall SR based on room SR
        oldSRs[0].size = new Vector2(oldSRs[0].transform.parent.GetComponentInParent<SpriteRenderer>().size.x,
            oldSRs[0].size.y);
        oldSRs[0].transform.localPosition = new Vector3(0, oldSRs[0].transform.localPosition.y,0);

        //skip first wall
        for(int i = 1;i < oldSRs.Length; i++)
        {
            DestroyImmediate(oldSRs[i].gameObject);
        }
    }

    void setupDoors(BGDoors doorsScript)
    {
        foreach(Door door in doorsScript.doors)
        {
            //Debug.Log("Door");
            door.Start();
            BoxCollider2D doorCol = door.doorWall;
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
                if (!Array.Exists(doorsScript.GetComponents<BoxCollider2D>(), x => x == wall))
                {
                    continue;
                }
               
                    
                //Debug.Log("wall" +wall.offset+" "+wall.size);
                    
                //Debug.Log("1  "+firstWall.size.x);
                //Debug.Log(door.sr.sprite.name+" "+BGDoors.spriteName.Replace("&n", BGDoors.sideToNum["top"])+" ");
                if (door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToNum["top"])) ||
                    door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToNum["bottom"])))
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

                    firstWall.offset = new Vector2(-(wall.size.x/2)+(firstWall.size.x/2)+wall.offset.x, //x
                        wall.offset.y);                                                 //y
                    //Debug.Log(firstWall.offset+" ");

                    secondWall.size = new Vector2(wall.size.x - (firstWall.size.x + doorCol.size.x),              //x
                        wall.size.y);                                                                             //y
                    //Debug.Log(secondWall.size);

                    secondWall.offset = new Vector2(-(-doorCol.offset.x - door.transform.localPosition.x) + (doorCol.size.x/2) + (secondWall.size.x /2) ,  //x
                        wall.offset.y);                                                                                                                     //y
                                                                                                                                                            //Debug.Log(secondWall.offset);   



                    if (door.sr.sprite.name.Equals(Door.spriteName.Replace("&n", Door.sideToNum["bottom"])))
                    {
                        //if bottom door, update forground

                        SpriteRenderer oldSr = doorsScript.forgroundsSR.Find(x =>
                            x.size.x == wall.size.x &&
                            x.transform.localPosition.x == wall.offset.x);

                        oldSr.size= new Vector2(firstWall.size.x,oldSr.size.y);
                        
                        oldSr.transform.localPosition = new Vector3(firstWall.offset.x,oldSr.transform.localPosition.y,0);

                        GameObject newSrObj = GameObject.Instantiate<GameObject>(oldSr.gameObject,oldSr.transform.parent);
                        
                        //Debug.Log(newSrObj);
                        
                        
                        SpriteRenderer newSr = newSrObj.GetComponent<SpriteRenderer>();

                        //copy oldSr fields to newSr
                        //System.Reflection.FieldInfo[] fields = oldSr.GetType().GetFields();
                        //Debug.Log("|"+fields[0]+"|");
                        //foreach(System.Reflection.FieldInfo field in fields)
                        //{
                        //    field.SetValue(newSr, field.GetValue(oldSr));
                        //}

                        newSr.size = new Vector2(secondWall.size.x,oldSr.size.y);
                        newSr.transform.localPosition = new Vector3(secondWall.offset.x, oldSr.transform.localPosition.y, 0);
                        doorsScript.forgroundsSR.Add(newSr);

                    }

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
                        -(wall.size.y / 2) + (firstWall.size.y / 2) + wall.offset.y);                                                 //y
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
