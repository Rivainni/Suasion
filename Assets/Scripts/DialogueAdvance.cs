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
    int speakerIndex;
    int activeSpeaker;

    void Start()
    {
        speakerIndex = 0;
        activeSpeaker = 0;
    }

    // the int value corresponds to the speaker's level + 2 (e.g. Sate = 2, Sate's father = 3)
    public void SetTargetSpeaker(int speaker)
    {
        speakerImage.sprite = speakerSprites[speaker];
        speakerIndex = speaker;
    }

    // either mc or target
    public void SetActiveSpeaker(string speaker)
    {
        if (speaker == "mc")
        {
            // mcIcon.sprite = glowIcons[1];
            targetIcon.sprite = normalIcons[speakerIndex];
            targetGlow.enabled = false;
            mcGlow.enabled = true;
            activeSpeaker = 0;
        }
        else
        {
            mcIcon.sprite = normalIcons[1];
            // targetIcon.sprite = glowIcons[speakerIndex];
            targetGlow.enabled = true;
            mcGlow.enabled = false;
            activeSpeaker = 1;
        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            dialogueView[activeSpeaker].UserRequestedViewAdvancement();
        }
    }
}
