using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private TextMeshProUGUI floatingText;
    public Transform otherObject;

    private float curNum;
    private float targetNum;

    private int LeanIdMove = 0;
    private int LeanIdSize = 0;
    private int LeanIdValue = 0;
    private Vector3 defaultPos;

    public void Initialize()
    {
        defaultPos = floatingText.rectTransform.anchoredPosition;
    }

    public void SetHealthBar(float amount)
    {
        healthBar.fillAmount = amount;
    }

    public void SetFloatingText(float num)
    {
        // TODO: SPLIT EFFECT AND DAMAGE
        targetNum += num;

        if (LeanTween.isTweening(LeanIdMove))
        {
            LeanTween.cancel(LeanIdMove);
            LeanTween.cancel(LeanIdSize);
            LeanTween.cancel(LeanIdValue);
        }

        animateValue();
        animateSize();
        animatePosition();
        // otherObject.position = new Vector3(desiredXPosition, desiredYPosition, otherObject.position.z);
    }

    private void reset()
    {
        curNum = 0;
        targetNum = 0;
        floatingText.text = $"";
    }

    private void animatePosition()
    {
        floatingText.rectTransform.anchoredPosition = new Vector3(defaultPos.x, defaultPos.y - 0.5f, defaultPos.z);
        LeanIdMove = LeanTween.moveY
            (floatingText.rectTransform, defaultPos.y, 2f).setEaseOutQuint()
            .setOnComplete(() =>
            {
                reset();
            }).id;
    }

    private void animateSize()
    {
        floatingText.rectTransform.localScale = Vector3.one;
        LeanIdSize = LeanTween.scale(floatingText.rectTransform, Vector3.one * 1.5f, 0.2f).setEaseOutQuint().setLoopPingPong(1).id;
    }

    private void animateValue()
    {
        LeanIdValue = LeanTween.value(gameObject, (float a) =>
        {
            floatingText.text = a > 0 ? $"+{Mathf.Round(a)}" : $"{Mathf.Round(a)}";
        },
        curNum, targetNum, 0.5f).setEaseOutQuint().id;
    }

    public void SetFloatingText(string text)
    {
        //animatefloatingText($"{text}");
    }
}