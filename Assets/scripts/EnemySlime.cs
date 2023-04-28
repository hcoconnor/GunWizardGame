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
            else if(toPlayer.magnitude <= attackRange || cooldown > 0)
            {
                state = EnemyState.IDLE;
            }

            anim.SetInteger("State", (int)state);
            int frame;
            switch (state)
            {
                
                case (EnemyState.IDLE):

                    break;
                case (EnemyState.WALKING):

                    frame = sr.sprite.name[sr.sprite.name.Length - 1] - '0';




                    float frameSpeed = walkSpeed.Evaluate(frame) * speed * Time.deltaTime;

                    toPlayer = toPlayer.normalized * frameSpeed;
                    transform.Translate(toPlayer);

                    break;
                case (EnemyState.ATTACKING):
                    
                    break;
                case (EnemyState.NULL):
                    break;
            }

            yield return null;
        }
        cooldown -= Time.deltaTime;
        yield return null;
    }
}


enum EnemyState{
    IDLE,
    WALKING,
    ATTACKING,
    NULL
}