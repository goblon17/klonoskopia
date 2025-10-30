using Radishmouse;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CablesMinigame : Minigame
{
    [SerializeField]
    private List<UILineRenderer> lineRenderers;
    [SerializeField]
    private List<CablesPoint> points;

    [SerializeField]
    private GameObject error;
    [SerializeField]
    private GameObject repeat;

    private int drawIndex = -1;

    private List<(int start, int end)> ans = new List<(int, int)>();

    private void Start()
    {
        HideAll();
        ResetMinigame();

        foreach (CablesPoint point in points)
        {
            point.OnPointerDown += OnPointerDown;
            point.OnPointerEnter += OnPointerEnter;
        }
    }

    private void Update()
    {
        foreach (UILineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.points.Clear();
        }

        int i = 0;
        foreach ((int start, int end) in ans)
        {
            lineRenderers[i].points.Add(points[start].transform.localPosition);
            lineRenderers[i].points.Add(points[end].transform.localPosition);
            i++;
        }

        if (drawIndex > -1)
        {
            lineRenderers[i].points.Add(points[drawIndex].transform.localPosition);

            Vector2 pointerPos = Pointer.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineRenderers[i].transform as RectTransform,
                pointerPos,
                null,
                out Vector2 point
            );
            lineRenderers[i].points.Add(point);
        }

        foreach (UILineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.SetVerticesDirty();
        }
    }

    private void OnPointerDown(CablesPoint point, PointerEventData eventData)
    {
        int i = GetPointIndex(point);
        if (ans.Any(e => e.start == i || e.end == i))
        {
            return;
        }

        HideAll();
        drawIndex = i;
    }

    private void OnPointerEnter(CablesPoint point, PointerEventData eventData)
    {
        if (drawIndex == -1)
        {
            return;
        }

        int i = GetPointIndex(point);
        if (ans.Any(e => e.start == i || e.end == i))
        {
            return;
        }

        ans.Add((Mathf.Min(drawIndex, i), Mathf.Max(drawIndex, i)));
        drawIndex = -1;

        if (ans.Count == lineRenderers.Count)
        {
            string a = "";
            foreach ((int start, int end) in ans.OrderBy(e => e.start))
            {
                a += $"{start}{end}";
            }
            OnMinigameEnd(a);
        }
    }

    private int GetPointIndex(CablesPoint point)
    {
        return points.IndexOf(point);
    }

    public override void ResetMinigame()
    {
        ans.Clear();
        drawIndex = -1;
        foreach (UILineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.points = new List<Vector2>();
            lineRenderer.SetVerticesDirty();
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
