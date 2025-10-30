using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PinMinigame : Minigame
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private List<Button> numberButtons;
    [SerializeField]
    private GameObject repeat;
    [SerializeField]
    private GameObject error;

    private void Start()
    {
        ResetMinigame();
        HideAll();
        int i = 0;
        foreach (Button button in numberButtons)
        {
            int j = i;
            button.onClick.AddListener(() => OnButton(j));
            i++;
        }
    }

    private void OnButton(int num)
    {
        text.text += num.ToString();
        if (text.text.Length >= 4)
        {
            foreach (Button button in numberButtons)
            {
                button.interactable = false;
            }
        }
        HideAll();
    }

    public override void ResetMinigame()
    {
        text.text = "";
        foreach (Button button in numberButtons)
        {
            button.interactable = true;
        }
    }

    public void AcceptButton()
    {
        OnMinigameEnd(text.text);
    }

    public override void ShowError()
    {
        HideAll();
        error.SetActive(true);
    }

    public override void ShowRepeat()
    {
        HideAll();
        repeat.SetActive(true);
    }

    private void HideAll()
    {
        repeat.SetActive(false);
        error.SetActive(false);
    }
}
