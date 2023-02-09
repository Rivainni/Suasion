using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DialogueAdvance : MonoBehaviour
{
    [SerializeField] Sprite[] speakerSprites;
    [SerializeField] Sprite[] normalIcons;
    [SerializeField] Sprite[] glowIcons; // future use
    [SerializeField] Image speakerImage;
    [SerializeField] Image targetIcon;
    [SerializeField] Image mcIcon;
    [SerializeField] Image targetGlow;
    [SerializeField] Image mcGlow;
    [SerializeField] DialogueViewBase[] dialogueView;
    [SerializeField] StoryManager storyManager;
    int speakerIndex;
    int activeSpeaker;

    void Start()
    {
        speakerIndex = 0;
        activeSpeaker = 1;
    }

    // the int value corresponds to the speaker's level + 2 (e.g. Sate = 2, Sate's father = 3)
    [YarnCommand("settarget")]
    public void SetTargetSpeaker(int speaker)
    {
        speakerImage.sprite = speakerSprites[speaker];
        mcIcon.sprite = normalIcons[1];
    }

    // the int value corresponds to the speaker's level + 2 (e.g. Sate = 2, Sate's father = 3)
    [YarnCommand("setactive")]
    public void SetActiveSpeaker(int speaker)
    {
        speakerIndex = speaker;
        targetIcon.sprite = normalIcons[speakerIndex];
    }

    [YarnCommand("setmainspeaker")]
    public void SetMainSpeaker(int speaker)
    {
        mcIcon.sprite = normalIcons[speaker];
    }

    // changes the active dialogue box and moves the glow
    [YarnCommand("focustarget")]
    public void FocusTarget(bool focus)
    {
        if (focus)
        {
            targetGlow.enabled = true;
            mcGlow.enabled = false;
            activeSpeaker = 1;
            storyManager.FocusTarget(true);
        }
        else
        {
            targetGlow.enabled = false;
            mcGlow.enabled = true;
            activeSpeaker = 0;
            storyManager.FocusTarget(false);
        }
    }

    public void Advance()
    {
        dialogueView[activeSpeaker].UserRequestedViewAdvancement();
    }
}
