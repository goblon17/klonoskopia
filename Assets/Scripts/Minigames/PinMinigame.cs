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

    private void Start()
    {
        ResetMinigame();

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
        
    }

    public override void ShowRepeat()
    {
        
    }
}
