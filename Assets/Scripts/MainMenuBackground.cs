using DG.Tweening;
using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField]
    private RectTransform image1;
    [SerializeField]
    private RectTransform image2;
    [SerializeField]
    private float duration;

    private void Start()
    {
        image1.DOAnchorMin(new Vector2(-1, 0), duration).SetLoops(-1).SetEase(Ease.Linear);
        image1.DOAnchorMax(new Vector2(0, 1), duration).SetLoops(-1).SetEase(Ease.Linear);

        image2.DOAnchorMin(new Vector2(0, 0), duration).SetLoops(-1).SetEase(Ease.Linear);
        image2.DOAnchorMax(new Vector2(1, 1), duration).SetLoops(-1).SetEase(Ease.Linear);
    }
}
