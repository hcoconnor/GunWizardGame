using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerSpellcasting : MonoBehaviour

{

    public Spell equippedSpell;
    public Image TargetingCircle;
    

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
            
            equippedSpell.update(null);
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

    abstract protected void drawTargetUI(Texture2D targetUI, Vector3 mouseCoord, Transform targetTrans);
    
    public void update(Texture2D targetUI)
    {
        Vector3 mouse = Input.mousePosition;
        Transform target = this.targetObj.transform;

        drawTargetUI(targetUI, mouse, target);
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
        //Debug.Log("new Spell");
    }

    protected override void drawTargetUI(Texture2D targetUI, Vector3 mouseCoord, Transform targetTrans)
    {
        if(targetUI == null)
        {
            Debug.Log("null");
        }
       // targetUI.SetPixel((int)mouseCoord.x,(int) mouseCoord.y,new Color(1f, 1f, 1f, 0f));
       
    }

    protected override IEnumerator effect()
    {
        throw new System.NotImplementedException();
    }
}