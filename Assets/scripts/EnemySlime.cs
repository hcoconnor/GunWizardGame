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

    Animator anim;
    SpriteRenderer sr;

    EnemyState state;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("AIStateMachine");
    }

    // Update is called once per frame
    new void Update() 
    {
        base.Update();

        sr.sortingOrder = (int)((-transform.position.y + .08f) * (100));

    }

    IEnumerator AIStateMachine()
    {
        float cooldown = 0;
        while (health > 0)
        {
            Vector3 toPlayer = player.transform.position - transform.position;

            if (toPlayer.magnitude <= attackRange && cooldown <= 0)
            {
                state = EnemyState.ATTACKING;
            }
            else if (toPlayer.magnitude <= attackRange || cooldown > 0)
            {
                state = EnemyState.IDLE;
            }
            else if (toPlayer.magnitude > attackRange)
            {
                state = EnemyState.WALKING;

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
        
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player)
        {
            player.GetComponent<playerStats>().hurt(collisionDamage, DamageType.physical);
        }
    }
}


enum EnemyState{
    IDLE,
    WALKING,
    ATTACKING,
    NULL
}