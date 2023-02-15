using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic("Title");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void PlayMusic(string music)
    {
        AkSoundEngine.PostEvent("Enter_" + music, gameObject);
    }
}