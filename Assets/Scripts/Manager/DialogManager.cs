using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogManager : Singleton<DialogManager>
{
    public DialogData curDialog;

    public UnityEvent OnDialogDone;

    [Range(0.1f, 10)]
    public float DialogSpeed = 5;

    private Coroutine typingCoroutine;

    private bool next;

    [Header("UI")]
    [SerializeField]
    private Canvas dialogPanel;

    [SerializeField]
    private Image dialogBg;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Image speakerIcon;

    [SerializeField]
    private TextMeshProUGUI speakerText;

    [SerializeField]
    private Button nextBtn;

    public override void Initialization()
    {
        nextBtn.onClick.AddListener(NextDialog);

        GameManager.Instance.OnBackToMenu.AddListener(Clear);
    }

    public void StartDialog(DialogData cur)
    {
        curDialog = cur;
        dialogPanel.gameObject.SetActive(true);
    }

    public IEnumerator Dialoging()
    {
        dialogBg.sprite = curDialog.Background;
        for (int i = 0; i < curDialog.Dialog.Length; i++)
        {
            next = false;
            DialogText dial = curDialog.Dialog[i];

            speakerText.text = dial.Speaker.name;
            speakerIcon.sprite = dial.Speaker.icon;

            typingCoroutine = StartCoroutine(typingText(dial.Text));

            if (curDialog.LeftSpeaker == dial.Speaker)
            {
                // left pane
            }

            yield return new WaitUntil(() => next);
            StopCoroutine(typingCoroutine);
        }

        yield return DialogDone();
    }

    private IEnumerator typingText(string text)
    {
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(0.1f / DialogSpeed);
        }
    }

    public void NextDialog()
    {
        next = true;
    }

    private IEnumerator DialogDone()
    {
        OnDialogDone?.Invoke();
        dialogPanel.gameObject.SetActive(false);

        yield return null;
    }

    private void Clear()
    {
        //StopCoroutine(dialogCoroutine);
    }
}