using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

public class ColorsMinigame : Minigame
{
    [SerializeField]
    private List<ColorsButton> buttons;
    [SerializeField]
    private List<ColorsButton.Config> configs;
    [SerializeField]
    private GameObject error;
    [SerializeField]
    private GameObject repeat;

    private string ans = "";

    private void Start()
    {
        HideAll();
        configs.ShuffleSelf();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].UnlockButton();
            buttons[i].SetButton(configs[i]);
            buttons[i].OnPress += OnPress;
        }
    }

    private void OnPress(ColorsButton button, char letter)
    {
        HideAll();

        ans += letter;
        if (ans.Length >= 4)
        {
            OnMinigameEnd(ans);
        }
    }

    public override void ResetMinigame()
    {
        ans = "";
        foreach (ColorsButton button in buttons)
        {
            button.UnlockButton();
        }
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
