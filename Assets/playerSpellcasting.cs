using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerSpellcasting : MonoBehaviour

{

    public Spell equippedSpell;

    

    // Start is called before the first frame update
    void Start()
    {
        equippedSpell = new FireSpell(this);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("CastSpell"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,.1f,LayerMask.GetMask("Targetable"));

            if (hit.collider != null )
            {
                //Debug.Log("CLICKED " + hit.collider.name);

                equippedSpell.start(hit.collider.gameObject);
            }
        }
        if (Input.GetButton("CastSpell") && equippedSpell.isTargeting())
        {
            equippedSpell.update();
        }
        if (Input.GetButtonUp("CastSpell") && equippedSpell.isTargeting())
        {
            equippedSpell.cast();
        }


    }

    
}

public abstract class Spell
{
    public GameObject targetObj;
    bool targeting;
    MonoBehaviour monoBehaviour;

    public Spell(MonoBehaviour mb)
    {
        targetObj = null;
        targeting = false;
        monoBehaviour = mb;

    }

    public bool isTargeting()
    {
        return targeting;
    }

    public void start(GameObject target)
    {
        this.targetObj = target;
        targeting = true;
    }

    abstract protected void drawTargetUI(Vector3 mouseCoord, Transform targetTrans);
    
    public void update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform target = this.targetObj.transform;

        drawTargetUI(mouse, target);
    }

    abstract protected IEnumerator effect();
    

    public void cast()
    {
        targeting = false;
        this.monoBehaviour.StartCoroutine(effect());
    }
   

}


public class FireSpell : Spell
{

    public FireSpell(MonoBehaviour mb) : base(mb)
    {

    }
    

    protected override void drawTargetUI(Vector3 mouseCoord, Transform targetTrans)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator effect()
    {
        throw new System.NotImplementedException();
    }
}