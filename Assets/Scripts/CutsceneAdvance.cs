using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CutsceneAdvance : MonoBehaviour
{
    [SerializeField] LineView dialogueView;
    [SerializeField] StoryManager storyManager;
    [SerializeField] Sprite[] dialogueIcons;
    [SerializeField] Image dialogueIcon;

    void Start()
    {

    }

    public void Advance()
    {
        dialogueView.UserRequestedViewAdvancement();
    }

    [YarnCommand("setcspeaker")]
    public void SetSpeaker(int speaker)
    {
        dialogueIcon.sprite = dialogueIcons[speaker];
    }
}
