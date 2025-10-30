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
    private RectTransform topOpenPos;
    [SerializeField]
    private RectTransform bottomOpenPos;

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
        bottomOpenPos.DOKill();

        float speed = topOpenPos.localPosition.y / animationDuration;

        bottomSlide.DOAnchorPos(bottomOpenPos.localPosition, speed).SetSpeedBased();
        return topSlide.DOAnchorPos(topOpenPos.localPosition, speed).SetSpeedBased().WaitForCompletion();
    }

    public void OpenInstantly()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        topSlide.anchoredPosition = topOpenPos.localPosition;
        bottomSlide.anchoredPosition = bottomOpenPos.localPosition;
    }

    public YieldInstruction Close()
    {
        topSlide.DOKill();
        bottomOpenPos.DOKill();

        float speed = topOpenPos.localPosition.y / animationDuration;

        bottomSlide.DOAnchorPos(Vector2.zero, speed).SetSpeedBased();
        return topSlide.DOAnchorPos(Vector2.zero, speed).SetSpeedBased().WaitForCompletion();
    }

    public void CloseInstantly()
    {
        topSlide.DOKill();
        bottomSlide.DOKill();

        topSlide.anchoredPosition = Vector2.zero;
        bottomSlide.anchoredPosition = Vector2.zero;
    }
}
