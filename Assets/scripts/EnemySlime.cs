using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : ObjectStats
{

    public float speed;
    public GameObject player;
    public float attackCooldown;
    public float attackRange;

    public AnimationCurve walkSpeed;

    Animator anim;
    SpriteRenderer sr;

    EnemyState state;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    new void Update() 
    {
        base.Update();


        Vector3 toPlayer =   player.transform.position - transform.position; 

        if(toPlayer.magnitude <= attackRange)
        {
            state = EnemyState.IDLE;
        }
        else if(toPlayer.magnitude > attackRange)
        {
            state = EnemyState.WALKING;

        }

        anim.SetInteger("State", (int)state);
        switch (state)
        {
            
            case(EnemyState.IDLE):

                break;
            case (EnemyState.WALKING):



                int frame = sr.sprite.name[sr.sprite.name.Length-1] - '0';


                float frameSpeed = walkSpeed.Evaluate(frame) * speed * Time.deltaTime;

                toPlayer = toPlayer.normalized * frameSpeed ;
                transform.Translate(toPlayer);

                break;
            case (EnemyState.ATTACKING):

                break;
            case (EnemyState.NULL):
                break;
        }

        sr.sortingOrder = (int)((-transform.position.y + .08f) * (100));

    }
}


enum EnemyState{
    IDLE,
    WALKING,
    ATTACKING,
    NULL
}