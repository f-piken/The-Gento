using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : Singleton<MainMenuUI>
{
    public List<Canvas> panel = new List<Canvas>();
    public List<Image> ShopPanel = new List<Image>();

    [Header("Name")]
    [SerializeField]
    private GameObject NamePanel;

    [SerializeField]
    private TMP_InputField nameInput;

    [Header("Map")]
    [SerializeField]
    public SpriteRenderer background;
    public Sprite[] backgroundSprites;
    
    [SerializeField]
    public LevelData[] levelDatas;

    [SerializeField]
    public Button[] levelButtons;
    public int status = 0;

    public override void Initialization()
    {
        nameInput.onEndEdit.AddListener(SetName);
        NamePanel.SetActive(SaveManager.Instance.IsNewPlayer);
        setupButtons();
    }
    private void leves(){
            

    }

    private void setupButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int copy = i;
            levelButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelDatas[i].name;
            levelButtons[i].onClick.AddListener(() =>
            {
                GameManager.Instance.Level.SetLevelData(levelDatas[copy]);
                if (copy < backgroundSprites.Length)
                {
                    background.sprite = backgroundSprites[copy];
                    float desiredWidth = 2.3f; // Ganti dengan lebar yang diinginkan
                    float desiredHeight = 2.8f; // Ganti dengan tinggi yang diinginkan
                    background.transform.localScale = new Vector3(desiredWidth, desiredHeight, 1.0f); 
                    float desiredXPosition = 0.0f; // Ganti dengan posisi X yang diinginkan
                    float desiredYPosition = -3.2f; // Ganti dengan posisi Y yang diinginkan
                    float zPosition = background.transform.position.z; // Tetapkan posisi Z yang sama
                    background.transform.position = new Vector3(desiredXPosition, desiredYPosition, zPosition);
                }
                GameManager.Instance.StarGame();
            });
            // if (status == 0){
            //     levelButtons[0].interactable = true;
            //     levelButtons[1].interactable = false;
            //     levelButtons[2].interactable = false;
            // }else if (status == 1){
            //     levelButtons[0].interactable = true;
            //     levelButtons[1].interactable = true;
            //     levelButtons[2].interactable = false;
            // }else if (status == 2){
            //     levelButtons[0].interactable = true;
            //     levelButtons[1].interactable = true;
            //     levelButtons[2].interactable = true;
            // }
        }
    }
    // private void ChangeBackground(LevelData selectedLevelData)
    // {
    //     // Gantilah background dengan gambar yang sesuai dengan level yang dipilih
    //     background.sprite = selectedLevelData.backgroundSprite;
    // }

    public void setVideoPanel(bool state)
    {
        panel[2].gameObject.SetActive(state);
    }

    public void PlayGame()
    {
        //GameManager.Instance.StarGame();
    }

    public void BackToMain()
    {
        panel[0].gameObject.SetActive(true);
        panel[1].gameObject.SetActive(false);
        GameManager.Instance.BackToMain();
    }

    public void HideAllShopPanel()
    {
        for (int i = 0; i < ShopPanel.Count; i++)
        {
            ShopPanel[i].gameObject.SetActive(false);
        }
    }

    #region name

    public void SetName(string name)
    {
        SaveManager.Instance.playerData.PlayerName = name;
    }

    public void DoneEditingName()
    {
        SaveManager.Instance.playerData.PlayerName = nameInput.text;
        SaveManager.Instance.Save();

        NamePanel.SetActive(false);
    }

    #endregion name
}