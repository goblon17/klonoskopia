using Radishmouse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShapeMinigame : Minigame
{
    [SerializeField]
    private List<ShapePoint> points;
    [SerializeField]
    private UILineRenderer lineRenderer;
    [SerializeField]
    private GameObject error;
    [SerializeField]
    private GameObject repeat;

    private bool isDown = false;

    private string ans = "";

    private void Start()
    {
        ResetMinigame();
        HideAll();
        foreach (ShapePoint point in points)
        {
            point.OnPointerDown += OnPointerDown;
            point.OnPointerEnter += OnPointerEnter;
        }
    }

    private void Update()
    {
        lineRenderer.points.Clear();

        foreach (char ci in ans)
        {
            int i = ci - '0';
            ShapePoint point = points[i];
            lineRenderer.points.Add(point.transform.localPosition);
        }

        Pointer current = Pointer.current;
        if (current == null)
        {
            return;
        }

        if (isDown)
        {
            if (current.press.isPressed)
            {
                Vector2 pointerPos = current.position.ReadValue();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    lineRenderer.transform as RectTransform,
                    pointerPos,
                    null,
                    out Vector2 point
                );
                lineRenderer.points.Add(point);
            }
            else
            {
                isDown = false;
                OnMinigameEnd(ans);
            }
        }

        lineRenderer.SetVerticesDirty();
    }

    private void OnPointerEnter(ShapePoint point, PointerEventData eventData)
    {
        int i = GetPointNumber(point);
        if (isDown && !ans.Contains(i.ToString()))
        {
            int last = ans.Last() - '0';
            int? between = GetBetweenPoint(last, i);
            if (between.HasValue && !ans.Contains(between.Value.ToString()))
            {
                ans += between.Value.ToString();
            }
            ans += i.ToString();
        }
    }

    private int? GetBetweenPoint(int a, int b)
    {
        switch ((a, b))
        {
            case (0, 2):
            case (2, 0):
                return 1;

            case (0, 6):
            case (6, 0):
                return 3;

            case (0, 8):
            case (8, 0):
            case (1, 7):
            case (7, 1):
            case (2, 6):
            case (6, 2):
            case (3, 5):
            case (5, 3):
                return 4;

            case (2, 8):
            case (8, 2):
                return 5;

            case (6, 8):
            case (8, 6):
                return 7;
        }
        return null;
    }

    private void OnPointerDown(ShapePoint point, PointerEventData eventData)
    {
        if (isDown)
        {
            return;
        }

        isDown = true;
        HideAll();
        ans += GetPointNumber(point).ToString();
    }

    private int GetPointNumber(ShapePoint point)
    {
        return points.IndexOf(point);
    }

    public override void ResetMinigame()
    {
        ans = "";
        isDown = false;
        lineRenderer.points = new List<Vector2>();
        lineRenderer.SetVerticesDirty();
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
