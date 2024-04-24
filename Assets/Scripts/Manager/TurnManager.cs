using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    private int totalChars = 0;
    private bool nextState = true;

    private Coroutine turnCoroutine = null;
    private Coroutine postTurnCoroutine = null;

    [SerializeField]
    private List<BaseChar> charas = new List<BaseChar>();

    [Header("UI")]
    [SerializeField]
    private List<TurnCharUI> turnCharUIs = new List<TurnCharUI>();

    private float sliderHeight;

    [SerializeField]
    private GameObject charTurnPrefab;

    [Header("Components")]
    [SerializeField]
    private Transform parentCharTurn;

    [SerializeField]
    private RectTransform slider;

    private TurnCharUI curReady;

    public override void Initialization()
    {
        GameManager.Instance.Level.OnGameOver.AddListener(ResetTurn);
        sliderHeight = slider.rect.height;
    }

    public IEnumerator RegisterTurn(List<BaseChar> _charas)
    {
        charas.AddRange(_charas);

        totalChars = _charas.Count;

        for (int i = 0; i < _charas.Count; i++)
        {
            TurnCharUI _spawn = Instantiate(charTurnPrefab, parentCharTurn).GetComponent<TurnCharUI>();
            _spawn.Data = _charas[i];

            turnCharUIs.Add(_spawn);
            movePos(_spawn, _spawn.Data.s_Speed.CurValue);
        }
        yield return null;
        // TODO: set them lerping from its speed along the max slider length
    }

    public void StartTurn()
    {
        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    private IEnumerator StartTurnEnum()
    {
        nextState = false;

        // looping all to find the first attaker
        BaseChar baseChar = findAttacker();

        yield return hisTurn(baseChar);

        yield return new WaitUntil(() => nextState);

        endTurn();
    }

    private BaseChar findAttacker()
    {
        int index = 0;
        for (int i = 0; i < totalChars; i++)
        {
            if (turnCharUIs[index].PosPercent < turnCharUIs[i].PosPercent
                && !turnCharUIs[i].Data.IsDie)
            {
                index = i;
            }
        }

        return turnCharUIs[index].Data;
    }

    private IEnumerator hisTurn(BaseChar baseChar)
    {
        yield return new WaitForSeconds(0.1f);

        yield return MoveToReady(baseChar);
        yield return baseChar.PreTurnBuff();

        if (baseChar.IsDie || baseChar.IsStunned)
        {
            //Debug.Log("is Stunned");
            NextTurn();
            yield break;
        }

        baseChar.TurnPhase();
    }

    public IEnumerator SpeedEffectBuff(BaseChar baseChar, float amount)
    {
        MovePosEffect(baseChar, amount);

        yield return null;
    }

    private void endTurn()
    {
        turnCoroutine = StartCoroutine(StartTurnEnum());
    }

    [ContextMenu("Next")]
    public void NextTurn()
    {
        if (GameManager.Instance.Level.IsGameOver) { return; }
        postTurnCoroutine = StartCoroutine(PostTurn());
    }

    private IEnumerator PostTurn()
    {
        resetPos(curReady, 0);

        yield return curReady.Data.PostTurnBuff();
        curReady = null;

        yield return new WaitForSeconds(1);
        nextState = true;
    }

    public bool isCurrentPlaying(BaseChar _char)
    {
        return curReady.Data == _char;
    }

    public void ResetTurn(bool isWin)
    {
        if (turnCoroutine != null)
            StopCoroutine(turnCoroutine);

        if (postTurnCoroutine != null)
            StopCoroutine(postTurnCoroutine);

        foreach (TurnCharUI item in turnCharUIs)
        {
            Destroy(item.gameObject);
        }
        turnCharUIs.Clear();
    }

    #region UI

    private void resetPos(TurnCharUI ui, float movePercentage)
    {
        ui.PosPercent = Mathf.Round(movePercentage * 100f) / 100f;

        float targetY = movePercentage * -sliderHeight / 100;

        ui.rect.anchoredPosition = Vector3.zero;

        LeanTween.moveY(ui.rect, targetY, 1f).setEase(LeanTweenType.easeOutQuint);
    }

    public void MovePosEffect(BaseChar ui, float amount)
    {
        movePos(FindCharUi(ui), amount);
    }

    private void movePos(TurnCharUI ui, float amount)
    {
        ui.PosPercent += amount;

        float targetY = ui.PosPercent * -sliderHeight / 100;
        targetY = Mathf.Clamp(targetY, -sliderHeight, 0);

        LeanTween.moveY(ui.rect, targetY, 1f).setEase(LeanTweenType.easeOutQuint);
    }

    public IEnumerator MoveToReady(BaseChar charReady)
    {
        curReady = FindCharUi(charReady);
        curReady.transform.SetAsLastSibling();

        float delta = 100 - curReady.PosPercent;

        foreach (TurnCharUI item in turnCharUIs)
        {
            movePos(item, delta);
        }
        yield return new WaitForSeconds(1f);
    }

    public TurnCharUI FindCharUi(BaseChar character)
    {
        return turnCharUIs.Find(a => a.Data == character);
    }

    #endregion UI
}