using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuObj;

    public bool canPause = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && canPause)
        {
            pauseMenuObj.SetActive(!pauseMenuObj.activeSelf);
            Time.timeScale = System.Convert.ToInt32( !pauseMenuObj.activeSelf);
        }
    }

    public void returnToMenu()
    {
        //Debug.Log(SceneManager.GetSceneAt(1).name);
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
        
    }

    public void reloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
