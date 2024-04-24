using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class LoadingHandler : Singleton<LoadingHandler>
{
    [SerializeField]
    private Canvas loadingCanvas;

    [SerializeField]
    private TextMeshProUGUI loadingText;

    [SerializeField]
    private Image blockerTypeImage;

    [SerializeField]
    private Image loadingBar;

    [SerializeField]
    private UnityEvent finishedEvent;

    public void ShowLoading(UnityAction finishAction = null)
    {
        loadingCanvas.gameObject.SetActive(true);
        if (finishAction != null)
        {
            finishedEvent.AddListener(finishAction);
        }
    }

    public void Progress(float prog)
    {
        loadingBar.fillAmount = prog;
        loadingText.text = $"{(prog * 100).ToString("0.00")}%";
    }

    public void FinishedLoading()
    {
        loadingCanvas.gameObject.SetActive(false);
        finishedEvent?.Invoke();
    }

    public void FailLoading(string err)
    {
        loadingText.text = $"{err}";
    }

    public IEnumerator ShowLoadingEnum()
    {
        loadingCanvas.gameObject.SetActive(true);
        blockerTypeImage.gameObject.SetActive(true);
        loadingText.transform.parent.gameObject.SetActive(false);
        blockerTypeImage.color = new Color(0, 0, 0, 0);

        LeanTween.alpha(blockerTypeImage.rectTransform, 1, 0.5f);
        yield return new WaitForSecondsRealtime(0.5f);
    }

    public IEnumerator FinishedLoadingEnum()
    {
        LeanTween.alpha(blockerTypeImage.rectTransform, 0, 0.5f);

        yield return new WaitForSecondsRealtime(0.5f);

        loadingText.transform.parent.gameObject.SetActive(true);

        loadingCanvas.gameObject.SetActive(false);
        blockerTypeImage.gameObject.SetActive(false);
    }
}