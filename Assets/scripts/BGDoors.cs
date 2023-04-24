using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGDoors : MonoBehaviour
{

    public List<Door> doors;
    //public List<GameObject> topDoors;
    //public List<GameObject> rightDoors;
    //public List<GameObject> bottomDoors;
    //public List<GameObject> leftDoors;


    public List<BoxCollider2D> walls;



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

    //replace &n with:
    //  - 0 = top
    //  - 1 = right
    //  - 2 = bottom
    //  - 3 = left


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
