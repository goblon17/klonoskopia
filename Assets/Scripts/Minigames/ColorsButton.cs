using UnityEngine;
using UnityEngine.UI;

public class ColorsButton : MonoBehaviour
{
    [System.Serializable]
    public struct Config
    {
        public string letter;
        public ColorBlock colors;
    }

    [SerializeField]
    private Button button;
    [SerializeField]
    private Image image;

    public event System.Action<ColorsButton, char> OnPress;

    private char letter;

    public void SetButton(Config config)
    {
        button.colors = config.colors;
        letter = config.letter[0];
        image.color = config.colors.normalColor;
    }

    private void Awake()
    {
        button.onClick.AddListener(OnButtonCLick);
    }

    private void OnButtonCLick()
    {
        button.interactable = false;
        OnPress?.Invoke(this, letter);
    }

    public void UnlockButton()
    {
        button.interactable = true;
    }
}
