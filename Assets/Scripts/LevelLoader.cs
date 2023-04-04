using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI progressText;
    DataPersistenceManager dataPersistenceManager;

    void Awake()
    {
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
    }

    public void LoadLevel(string name)
    {
        if (name == "Main Menu")
        {
            StartCoroutine(SaveGame());
        }
        StartCoroutine(LoadAsynchronously(name));
    }

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = "LOADING: " + (progress * 100) + "%";
            fill.color = gradient.Evaluate(slider.normalizedValue);
            yield return null;
        }
    }

    IEnumerator SaveGame()
    {
        dataPersistenceManager.SaveGame();
        yield return null;
    }
}