using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public event System.Action<Minigame, string> MinigameEnded;

    public abstract void ResetMinigame();

    public abstract void ShowError();

    public abstract void ShowRepeat();

    protected void OnMinigameEnd(string val)
    {
        MinigameEnded?.Invoke(this, val);
    }
}
