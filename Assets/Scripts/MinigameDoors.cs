using DG.Tweening;
using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameDoors : MonoBehaviour
{
    public enum Message { None, Correct, Wrong, Dots}

    [SerializeField]
    private RectTransform topSlide;
    [SerializeField]
    private RectTransform bottomSlide;

    [SerializeField]
    private SerializedDictionary<Message, RectTransform> messages;
    [SerializeField]
    private float animationDuration;

    public void ShowMessage(Message message)
    {
        foreach (RectTransform obj in messages.Values)
        {
            obj.gameObject.SetActive(false);
        }
        if (messages.TryGetValue(message, out RectTransform trans))
        {
            trans.gameObject.SetActive(true);
        }
    }

    public YieldInstruction Open()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        float speed = 0.5f / animationDuration;

        bottomSlide.DOAnchorMin(new Vector2(0, -0.5f), speed).SetSpeedBased();
        bottomSlide.DOAnchorMax(new Vector2(1, 0.5f), speed).SetSpeedBased();

        topSlide.DOAnchorMin(new Vector2(0, 0.5f), speed).SetSpeedBased();
        return topSlide.DOAnchorMax(new Vector2(1, 1.5f), speed).SetSpeedBased().WaitForCompletion();
    }

    public void OpenInstantly()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        topSlide.anchorMin = new Vector2(0, 0.5f);
        topSlide.anchorMax = new Vector2(1, 1.5f);

        bottomSlide.anchorMin = new Vector2(0, -0.5f);
        bottomSlide.anchorMax = new Vector2(1, 0.5f);
    }

    public YieldInstruction Close()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        float speed = 0.5f / animationDuration;

        bottomSlide.DOAnchorMin(new Vector2(0, 0), speed).SetSpeedBased();
        bottomSlide.DOAnchorMax(new Vector2(1, 1), speed).SetSpeedBased();
        
        topSlide.DOAnchorMin(new Vector2(0, 0), speed).SetSpeedBased();
        return topSlide.DOAnchorMax(new Vector2(1, 1), speed).SetSpeedBased().WaitForCompletion();
    }

    public void CloseInstantly()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        topSlide.anchorMin = new Vector2(0, 0);
        topSlide.anchorMax = new Vector2(1, 1);

        bottomSlide.anchorMin = new Vector2(0, 0);
        bottomSlide.anchorMax = new Vector2(1, 1);
    }
}
