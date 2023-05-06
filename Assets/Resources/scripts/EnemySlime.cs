using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : ObjectStats
{

    public float speed;
    public GameObject player;
    public float attackCooldown;
    public float attackRange;
    public float collisionDamage;

    public AnimationCurve walkSpeed;
    public AnimationCurve attackSpeed;

    public int pointsPerKill = 10;
    public int manaReturn = 2;

    Animator anim;
    SpriteRenderer sr;

    [HideInInspector]
    public bool aggro;

    List<Transform> aStarPath;
    bool pathSafe;
    bool aStarRunning;

    Vector3 toPlayer;

    Room startRoom;
    EnemyState state;

    // Start is called before the first frame update
    new void Start()
    { 
        
        base.Start();
        pathSafe = false;
        aStarRunning = false;
        aStarPath = new List<Transform>();
        player = this.transform.root.Find("Wizard").gameObject;
        Debug.Log(player.name);
        startRoom = GetComponentInParent<Room>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        aggro = false;
        toPlayer = player.transform.position - transform.position;
        StartCoroutine("AIStateMachine");
        
    }

    // Update is called once per frame
    new void Update() 
    {
        base.Update();

        sr.sortingOrder = (int)((-transform.position.y + .08f) * (100));



        if (!aggro && startRoom.playerInRoom)
        {
            Debug.Log("AGGRO!");
            aggro = true;
        }

    }
    override public void onDeath()
    {
        playerStats pStats = player.GetComponent<playerStats>();
        pStats.points += pointsPerKill;
        pStats.restoreMana(manaReturn);
        base.onDeath();
    }
    

    IEnumerator AIStateMachine()
    {
        float cooldown = 0;
        while (health > 0)
        {
            toPlayer = player.transform.position - transform.position;


            if (!aggro)
            {
                state = EnemyState.IDLE;
            }
            else
            {


                //check if need to pathfind
                RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, toPlayer, toPlayer.magnitude, LayerMask.GetMask("Room"));
                if (hit && !aStarRunning)
                {
                    //no straight line path, so path find
                    //StartCoroutine(getPath(player.transform));
                    state = EnemyState.IDLE;
                }

                else if (!hit && toPlayer.magnitude <= attackRange && cooldown <= 0)
                {
                    state = EnemyState.ATTACKING;
                }
                else if ((hit && aStarRunning) ||  (toPlayer.magnitude <= attackRange || cooldown > 0))
                {
                    state = EnemyState.IDLE;
                }
                else if (hit || toPlayer.magnitude > attackRange)
                {
                    state = EnemyState.WALKING;

                }
            }
            

            anim.SetInteger("State", (int)state);
            int frameNum;
            float frameSpeed;
            switch (state)
            {
                
                case (EnemyState.IDLE):

                    break;
                case (EnemyState.WALKING):

                    
                    frameNum = sr.sprite.name[sr.sprite.name.Length - 1] - '0';

                    frameSpeed = walkSpeed.Evaluate(frameNum) * speed * Time.deltaTime;


                    if (aStarPath.Count > 0)
                    {
                        toPlayer = aStarPath[0].transform.position - transform.position;

                        if (toPlayer.magnitude <= frameSpeed)
                        {
                            aStarPath.RemoveAt(0);
                        }
                    }
                    

                    

                    toPlayer = toPlayer.normalized * frameSpeed;
                    transform.Translate(toPlayer);

                    break;
                case (EnemyState.ATTACKING):
                    string attackSpriteName = "Slime_Attack";
                    while (!sr.sprite.name.Contains(attackSpriteName))
                    {
                        yield return null;
                    }
                    
                    //Debug.Log("|"+sr.sprite.name.Substring(attackSpriteName.Length)+"|");
                    frameNum = int.Parse(sr.sprite.name.Substring(attackSpriteName.Length));
                    while (frameNum < 10)
                    {
                        //Debug.Log(sr.sprite.name);
                        toPlayer = player.transform.position - transform.position;
                        frameSpeed = walkSpeed.Evaluate(frameNum) * speed * Time.deltaTime;
                        toPlayer = toPlayer.normalized * frameSpeed;
                        transform.Translate(toPlayer);

                        yield return null;
                        //Debug.Log("|" + sr.sprite.name.Substring(attackSpriteName.Length) + "|");
                        frameNum = int.Parse(sr.sprite.name.Substring(attackSpriteName.Length));
                        //Debug.Log(state + " " + frameNum);
                    }
                    //Debug.Log(state + " " + frameNum);
                    //frame 10
                    toPlayer = player.transform.position - transform.position;

                    frameSpeed = walkSpeed.Evaluate(frameNum) * speed * Time.deltaTime;
                    Vector2 mvmt = toPlayer.normalized * frameSpeed;
                    transform.Translate(mvmt);
                    yield return null;

                    //frame 11 
                    frameNum = int.Parse(sr.sprite.name.Substring(attackSpriteName.Length));

                    frameSpeed = walkSpeed.Evaluate(frameNum) * speed * Time.deltaTime;
                    mvmt = toPlayer.normalized * frameSpeed;
                    //Debug.Log("frameSP:" + frameSpeed);
                    transform.Translate(mvmt);
                    yield return null;
                    cooldown = attackCooldown;

                    break;
                case (EnemyState.NULL):
                    break;
            }

            yield return null;
            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(cooldown, 0);
            //Debug.Log(state +" " + cooldown);
        }
        // dead

       
        
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collider: " + collision.collider.name);
        if (collision.gameObject == player)
        {
            player.GetComponent<playerStats>().hurt(collisionDamage, DamageType.physical);
        }
        else
        {
            
        }
    }


    private Room getRoom(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, toPlayer, 0, LayerMask.GetMask("RoomTrigger"));
        Debug.Log(hit.collider.name);
        return hit.collider.transform.parent.GetComponent<Room>();
    }

    IEnumerator getPath(Transform goal)
    {
        aStarRunning = true;
        List<PathNode> open = new List<PathNode>();
        List<PathNode> closed = new List<PathNode>();
        PathNode start = new PathNode(this.transform,0,goal,null);
        open.Add(start);
        PathNode current = null;

        int loopsSinceFrame = 0;
        while (open.Count > 0)
        {
            loopsSinceFrame++;
            if (loopsSinceFrame > 50)
            {
                yield return null;
                loopsSinceFrame = 0;
            }


            open.Sort();
            current = open[0];
            open.RemoveAt(0);


            Debug.Log("current: " + current.here.position);
            if (getRoom(current.here.position).playerInRoom)
            {
                break;
            }
            List<Door> doors = getNeighbors(getRoom(this.transform.position));


            foreach(Door door in doors)
            {
                Transform firstNavPoint = door.transform.Find("PathFindPoint");
                PathNode node1 = new PathNode(firstNavPoint, Vector3.Distance(current.here.position, firstNavPoint.position) + current.pathCost, goal, current);
                PathNode node2 = new PathNode(door.connectingNavPoint, Vector3.Distance(firstNavPoint.position, door.connectingNavPoint.position) + node1.pathCost, goal, node1);
                closed.Add(node1);
                open.Add(node2);
            }

            closed.Add(current);

        }
        pathSafe = false;
        aStarPath = new List<Transform>();
        while(current.parent != null)
        {
            aStarPath.Add(current.here);
            current = current.parent;
        }
        aStarPath.Reverse();
        pathSafe = true;
        aStarRunning = false;
        yield return null;
    }

    private List<Door> getNeighbors(Room room)
    {
        List<Door> doors = new List<Door>();

        foreach(Door door in room.bgDoors.doors)
        {
            if (door.sr.enabled)
            {
                doors.Add(door);
            }
        }
        return doors;
    }
    
}


enum EnemyState{
    IDLE,
    WALKING,
    ATTACKING,
    NULL
}

class PathNode : System.IComparable
{
    public PathNode parent;
    public Transform here;
    public float pathCost;
    public Transform goal;

    public PathNode(Transform trans, float cost,Transform goal, PathNode last)
    {
        here = trans;
        pathCost = cost;
        this.goal = goal;
        parent = last;
    }

    public int CompareTo(object obj)
    {
        if(obj == null)
        {
            return 1;
        }
        PathNode other = obj as PathNode;
        if (other != null)
        {

            return this.getFCost(this.goal).CompareTo(other.getFCost(this.goal));
        }
        else
        {
            throw new System.ArgumentException("Not Comparable");
        }

    }

    static float getHcost(Transform node, Transform goal)
    {
        return Vector3.Distance(node.position, goal.position);
    }
    public float getFCost(Transform goal)
    {
        return pathCost + getHcost(this.here, goal);
    }
}
    