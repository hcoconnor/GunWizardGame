using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowTime : MonoBehaviour
{

    [SerializeField]
    [Range(0f, 1f)]
    float timeScale;

    [SerializeField]
    
    Animator UIAnimator;

    [HideInInspector]
    public static bool isSlow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SlowTime"))
        {
            UIAnimator.SetTrigger("FadeIn");
            Time.timeScale = timeScale;
            isSlow = true;
            
        }
        else if (Input.GetButtonUp("SlowTime"))
        {
            Time.timeScale = 1;
            UIAnimator.SetTrigger("FadeOut");

            isSlow = false;
        }
    }
}
