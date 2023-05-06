using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{

    public List<GameObject> pages;
    public GameObject buttonNext;
    public GameObject buttonBack;

    int pageNum = 0;


    public void instructionsNext()
    {
        if(pageNum < pages.Count-1)
        {
            pages[pageNum].SetActive(false);
            pageNum++;
            pages[pageNum].SetActive(true);
        }
        buttonCheck();
    }



    public void instructionsBack()
    {
        if (pageNum > 0)
        {
            pages[pageNum].SetActive(false);
            pageNum--;
            pages[pageNum].SetActive(true);
        }
        buttonCheck();
    }

    public void buttonCheck()
    {
        if(pageNum >= pages.Count - 1)
        {
            buttonNext.SetActive(false);
        }
        else
        {
            buttonNext.SetActive(true);
        }

        if (pageNum <= 0)
        {
            buttonBack.SetActive(false);
        }
        else
        {
            buttonBack.SetActive(true);
        }
    }

    public void instructionsReset()
    {
        foreach(GameObject page in pages)
        {
            page.SetActive(false);
        }
        pageNum = 0;
        pages[pageNum].SetActive(true);
        buttonCheck();
    }
}
