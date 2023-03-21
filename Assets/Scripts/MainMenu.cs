using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    Button settingsButton;
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

    public void Settings()
    {
        GameObject settings = Instantiate(settingsMenu, transform.position, Quaternion.identity, transform);
        Settings settingsScript = settings.GetComponent<Settings>();
        settingsScript.settingsButton = settingsButton;
    }

    void PlayMusic(string music)
    {
        AkSoundEngine.PostEvent("Enter_" + music, gameObject);
    }
}