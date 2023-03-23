using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Button settingsButton;
    [SerializeField]
    Slider masterVolume;
    [SerializeField]
    Slider musicVolume;
    [SerializeField]
    Slider effectsVolume;
    [SerializeField]
    TMP_Dropdown resolutionDropdown;
    [SerializeField]
    Toggle fullscreenToggle;

    int[] currentSettings = new int[6];
    Resolution currentResolution;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        settingsButton.interactable = false;
        resolutions = Screen.resolutions;
        ReadSettings();
        DisplaySettings();
    }

    void DisplaySettings()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Convert.ToBoolean(currentSettings[2]);
        masterVolume.value = currentSettings[3];
        musicVolume.value = currentSettings[4];
        effectsVolume.value = currentSettings[5];
    }

    void UpdateSettings()
    {
        currentSettings[0] = resolutions[resolutionDropdown.value].width;
        currentSettings[1] = resolutions[resolutionDropdown.value].height;
        currentSettings[2] = Convert.ToInt32(fullscreenToggle.isOn);
        currentSettings[3] = (int)masterVolume.value;
        currentSettings[4] = (int)musicVolume.value;
        currentSettings[5] = (int)effectsVolume.value;
    }

    void RevertSettings()
    {
        resolutionDropdown.value = currentSettings[0];
        fullscreenToggle.isOn = Convert.ToBoolean(currentSettings[2]);
        masterVolume.value = currentSettings[3];
        musicVolume.value = currentSettings[4];
        effectsVolume.value = currentSettings[5];
    }

    void SetResolution()
    {
        Screen.SetResolution(currentSettings[0], currentSettings[1], Convert.ToBoolean(currentSettings[2]));
    }

    // IO
    void ReadSettings()
    {
        string path = Path.Combine(Application.dataPath, "settings.txt");
        string txt = "";
        try
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(path)))
            {
                txt = reader.ReadToEnd();
            }
        }
        catch (FileNotFoundException e)
        {
            WriteSettings(Screen.currentResolution.width, Screen.currentResolution.height, 1, 100, 100, 100);
            Debug.Log(e);
        }

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());
        if (lines.Length > 1)
        {
            ReadSettings(lines);
        }
    }

    void ReadSettings(string[] lines)
    {
        string currentSetting = "";
        foreach (string line in lines)
        {
            if (line.StartsWith("["))
            {
                currentSetting = line.Substring(1, line.IndexOf(']') - 1);
            }
            else
            {
                int parsed = 0;
                if (Int32.TryParse(line, out parsed))
                {
                    if (currentSetting == "ResolutionW")
                    {
                        currentSettings[0] = parsed;
                    }
                    else if (currentSetting == "ResolutionH")
                    {
                        currentSettings[1] = parsed;
                    }
                    else if (currentSetting == "Fullscreen")
                    {
                        currentSettings[2] = parsed;
                    }
                    else if (currentSetting == "MasterVolume")
                    {
                        currentSettings[3] = parsed;
                    }
                    else if (currentSetting == "MusicVolume")
                    {
                        currentSettings[4] = parsed;
                    }
                    else if (currentSetting == "EffectsVolume")
                    {
                        currentSettings[5] = parsed;
                    }
                }
            }
        }
    }

    void WriteSettings(int resolutionW, int resolutionH, int fullscreen, int masterVolume, int musicVolume, int effectsVolume)
    {
        currentSettings[0] = resolutionW;
        currentSettings[1] = resolutionH;
        currentSettings[2] = fullscreen;
        currentSettings[3] = masterVolume;
        currentSettings[4] = musicVolume;
        currentSettings[5] = effectsVolume;

        string path = Path.Combine(Application.dataPath, "settings.txt");
        using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create)))
        {
            writer.WriteLine("[ResolutionW]");
            writer.WriteLine(currentSettings[0]);
            writer.WriteLine("[ResolutionH]");
            writer.WriteLine(currentSettings[1]);
            writer.WriteLine("[Fullscreen]");
            writer.WriteLine(currentSettings[2]);
            writer.WriteLine("[MasterVolume]");
            writer.WriteLine(currentSettings[3]);
            writer.WriteLine("[MusicVolume]");
            writer.WriteLine(currentSettings[4]);
            writer.WriteLine("[EffectsVolume]");
            writer.WriteLine(currentSettings[5]);
        }
    }

    public void Cancel()
    {
        ReadSettings();
        RevertSettings();
        settingsButton.interactable = true;
        Destroy(gameObject);
    }

    public void Save()
    {
        UpdateSettings();
        SetResolution();
        WriteSettings(currentSettings[0], currentSettings[1], currentSettings[2], currentSettings[3], currentSettings[4], currentSettings[5]);
        settingsButton.interactable = true;
        Destroy(gameObject);
    }
}