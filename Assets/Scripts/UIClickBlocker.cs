using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickBlocker : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(BlockClicks());
    }

    IEnumerator BlockClicks()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
