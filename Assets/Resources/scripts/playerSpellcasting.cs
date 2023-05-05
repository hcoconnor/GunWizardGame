using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(playerStats))]
public class playerSpellcasting : MonoBehaviour

{
    static public int PIXEL_SCALE = 3;


    public Spell equippedSpell;
    public Image TargetingCircle;
    
    [HideInInspector]
    public playerStats pStats;
    

    // Start is called before the first frame update
    void Start()
    {
        equippedSpell = new FireSpell(this);
        Texture2D newTexture = new Texture2D(Camera.main.pixelWidth / PIXEL_SCALE, Camera.main.pixelHeight / PIXEL_SCALE);
        newTexture.filterMode = FilterMode.Point;

        clearTexture2D(newTexture);
        

        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width,newTexture.height), new Vector2(0, 0));
        TargetingCircle.sprite = newSprite;

        pStats = GetComponent<playerStats>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("CastSpell"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,.1f,LayerMask.GetMask("Targetable","bullet"));

            if (hit.collider != null )
            {
                //Debug.Log("CLICKED " + hit.collider.name);

                equippedSpell.start(hit.collider.gameObject);
            }
        }
        if (Input.GetButton("CastSpell") && equippedSpell.isTargeting())
        {
            
            equippedSpell.update(TargetingCircle.sprite.texture);
        }
        if (Input.GetButtonUp("CastSpell") && equippedSpell.isTargeting())
        {
            equippedSpell.cast(TargetingCircle.sprite.texture);
        }


    }


    public static void clearTexture2D(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        for (int x = 0; x < pixels.Length; x++)
        {
            pixels[x] = Color.clear;
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }

}



public abstract class Spell
{
    public GameObject targetObj;
    bool targeting;
    protected playerSpellcasting monoBehaviour;
    protected float maxCost;
    protected float maxDist;

    public Spell(playerSpellcasting mb)
    {
        targetObj = null;
        targeting = false;
        monoBehaviour = mb;
        maxCost = 0;
        maxDist = 0;

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

    abstract protected IEnumerator effect(Vector3 mouseCoords);


    protected float getCost(float dist)
    {
        return (dist / maxDist) * maxCost;
    }
    protected float getDistGivenMana(float dist)
    {
        dist = Mathf.Min(maxDist, dist);
        if (getCost(dist) > monoBehaviour.pStats.mana)
        {
            //math stuff

            dist = (monoBehaviour.pStats.mana * maxDist) / maxCost;
            return dist;
        }
        else
        {
            //can afford what we doin
            return dist;
        }




    }

    public void cast(Texture2D targetUI)
    {
        targeting = false;
        playerSpellcasting.clearTexture2D(targetUI);
        this.monoBehaviour.StartCoroutine(effect(Input.mousePosition));
    }
   
   

}


public class FireSpell : Spell
{

    static float initialDamage = 5;
    static float dps = 2;   //5 init + 6 dot = 11 tot damage;
    static float fireTime = 3;
    static GameObject explosionPrefab;

    public FireSpell(playerSpellcasting mb) : base(mb)
    {
        if(explosionPrefab == null)
        {
            explosionPrefab = (GameObject)Resources.Load("Prefab/FireExplosion",typeof(GameObject));
        }
        maxCost = 5;
        maxDist = .3f;
        //Debug.Log(explosionPrefab);
    }

    

    protected override void drawTargetUI(Texture2D targetUI, Vector3 mouseCoord, Transform targetTrans)
    {
        //Debug.Log("drawing at " + mouseCoord);
        //temp.position = mouseCoord;

        //targetUI.SetPixels(0, 0, targetUI.width, targetUI.height,emptyColor);
        Color[] pixels = targetUI.GetPixels();

        //Debug.Log(Camera.main.WorldToScreenPoint(targetTrans.position).x/playerSpellcasting.PIXEL_SCALE + " "+ Camera.main.WorldToScreenPoint(targetTrans.position).y / playerSpellcasting.PIXEL_SCALE);
        //Debug.Log(mouseCoord.x / playerSpellcasting.PIXEL_SCALE + " " + mouseCoord.y / playerSpellcasting.PIXEL_SCALE);
        float radius = (int)(Vector3.Distance(mouseCoord / playerSpellcasting.PIXEL_SCALE, Camera.main.WorldToScreenPoint(targetTrans.position)/playerSpellcasting.PIXEL_SCALE));
        //float radius = (int)(Vector3.Distance(mouseCoord , Camera.main.WorldToScreenPoint(targetTrans.position)));
        //radius = getDistGivenMana(radius*playerSpellcasting.PIXEL_SCALE) /playerSpellcasting.PIXEL_SCALE;
        //Debug.Log(radius + " "+radius/playerSpellcasting.PIXEL_SCALE+" "+ getDistGivenMana(radius/playerSpellcasting.PIXEL_SCALE));



        float WSradius = (Vector3.Distance(Camera.main.ScreenToWorldPoint(mouseCoord), targetObj.transform.position));
        float modifiedWSradius = getDistGivenMana(WSradius);
        float ratio =  modifiedWSradius/WSradius;

        radius *= ratio;

        //Debug.Log(radius);
        for (int x=0; x < targetUI.width; x++)
        {
            for(int y=0; y < targetUI.height; y++)
            {
                //Debug.Log((Mathf.Pow(x - (Camera.main.WorldToScreenPoint(targetTrans.position).x / playerSpellcasting.PIXEL_SCALE), 2)) +" "+
                //    Mathf.Pow(y - (Camera.main.WorldToScreenPoint(targetTrans.position).y / playerSpellcasting.PIXEL_SCALE), 2) +" "+
                //    Mathf.Pow(radius, 2)+" "+

                //    (Mathf.Abs((Mathf.Pow(x - (Camera.main.WorldToScreenPoint(targetTrans.position).x / playerSpellcasting.PIXEL_SCALE), 2) + 
                //    Mathf.Pow(y - (Camera.main.WorldToScreenPoint(targetTrans.position).y / playerSpellcasting.PIXEL_SCALE), 2))
                //    - Mathf.Pow(radius, 2))));
                if(Mathf.Abs((Mathf.Pow(x-(Camera.main.WorldToScreenPoint(targetTrans.position).x / playerSpellcasting.PIXEL_SCALE), 2)+Mathf.Pow(y-( Camera.main.WorldToScreenPoint(targetTrans.position).y / playerSpellcasting.PIXEL_SCALE), 2)) - Mathf.Pow(radius,2) ) <= (radius+.1f))
                {
                    //Debug.Log("true");
                    pixels[y*targetUI.width+x] = Color.white;
                }
                else
                {
                    pixels[y * targetUI.width + x] = Color.clear;
                }
            }
        }

        targetUI.SetPixels(pixels);
        //targetUI.SetPixel((int)mouseCoord.x/playerSpellcasting.PIXEL_SCALE,(int) mouseCoord.y/ playerSpellcasting.PIXEL_SCALE, Color.blue);
        targetUI.Apply();
       
    }

    protected override IEnumerator effect(Vector3 mouseCoords)
    {

        float radius = (Vector3.Distance(Camera.main.ScreenToWorldPoint(mouseCoords), targetObj.transform.position));


        radius = getDistGivenMana(radius);
        monoBehaviour.pStats.castSpell(getCost(radius));
        Debug.Log("final: " + radius + " " + getDistGivenMana(radius));
        Debug.Log(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);
        




        RaycastHit2D[] hits = Physics2D.CircleCastAll(this.targetObj.transform.position, radius, Vector2.zero, 0f);
        //Debug.Log(mouseCoords+" "+Camera.main.ScreenToWorldPoint(mouseCoords)+" "+ targetObj.transform.position + " "+radius);
        


        Transform parent = targetObj.transform;
        //Debug.Log(parent+" "+targetObj.name);
        GameObject explosion = GameObject.Instantiate<GameObject>(explosionPrefab,parent);
        
        explosion.GetComponent<explosion>().setExplosionSize(radius);


        List<Burnable> burned = new List<Burnable>();
        for(int x=0; x < hits.Length; x++)
        {
            Burnable burnableObj = hits[x].collider.gameObject.GetComponent<Burnable>();
            if(burnableObj != null)
            {
                burnableObj.onFire = true;
                burnableObj.stats.hurt(FireSpell.initialDamage,DamageType.fire);
                burned.Add(burnableObj);
            }
        }
        float timer = FireSpell.fireTime;
        while(timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer -= 1;
            foreach (Burnable obj in burned)
            {
                if (obj != null)
                {
                    obj.stats.hurt(FireSpell.dps, DamageType.fire);
                }
                

            }
        }

        foreach (Burnable obj in burned)
        {
            if (obj != null)
            {
                obj.onFire = false;
            }
            

        }

        yield return null;

    }
}

public enum DamageType
{
    fire,
    force,
    physical
}