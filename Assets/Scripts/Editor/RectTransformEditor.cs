using StorkStudios.CoreNest;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectTransform))]
[CanEditMultipleObjects]
public class RectTransformEditor : BuiltInEditorExtensionBase
{
    protected override string BuiltInEditorTypeName => "UnityEditor.RectTransformEditor, UnityEditor";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
        EditorGUILayout.LabelField("Extras", GUIStyles.Bold);

        if (GUILayout.Button("Set anchors to corners"))
        {
            foreach (RectTransform target in targets)
            {
                Undo.RecordObject(target, "SetTopLeftAnchor");

                RectTransform parentRectTransform = target.transform.parent.GetComponent<RectTransform>();
                Vector2 size = CalculateAnchorRectSize(target);
                Vector2 position = CalculateAnchorRectPosition(target, parentRectTransform, size.x, size.y);
                Rect anchorRect = new Rect(position.x, position.y, size.x, size.y);
                MoveAnchorsToCorners(target, parentRectTransform, anchorRect);
            }
        }
    }

    private Vector2 CalculateAnchorRectSize(RectTransform rectTransform)
    {
        return new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }

    private Vector2 CalculateAnchorRectPosition(RectTransform ownRectTransform, RectTransform parentRectTransform, float width, float height)
    {
        Vector2 anchorVector = Vector2.zero;

        float pivotX = width * ownRectTransform.pivot.x;
        float pivotY = height * (1 - ownRectTransform.pivot.y);
        float newX = ownRectTransform.anchorMin.x * parentRectTransform.rect.width + ownRectTransform.offsetMin.x + pivotX - parentRectTransform.rect.width * anchorVector.x;
        float newY = -(1 - ownRectTransform.anchorMax.y) * parentRectTransform.rect.height + ownRectTransform.offsetMax.y - pivotY + parentRectTransform.rect.height * (1 - anchorVector.y);
        return new Vector2(newX, newY);
    }

    private void MoveAnchorsToCorners(RectTransform rectTransform, RectTransform parentRectTransform, Rect anchorRect)
    {
        Vector2 anchorVector = Vector2.zero;

        float pivotX = anchorRect.width * rectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - rectTransform.pivot.y);
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);

        float offsetMinX = anchorRect.x / rectTransform.localScale.x;
        float offsetMinY = anchorRect.y / rectTransform.localScale.y - anchorRect.height;
        rectTransform.offsetMin = new Vector2(offsetMinX, offsetMinY);
        float offsetMaxX = anchorRect.x / rectTransform.localScale.x + anchorRect.width;
        float offsetMaxY = anchorRect.y / rectTransform.localScale.y;
        rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);

        float anchorMinX = rectTransform.anchorMin.x + anchorVector.x + (rectTransform.offsetMin.x - pivotX) / parentRectTransform.rect.width * rectTransform.localScale.x;
        float anchorMinY = rectTransform.anchorMin.y - (1 - anchorVector.y) + (rectTransform.offsetMin.y + pivotY) / parentRectTransform.rect.height * rectTransform.localScale.y;
        rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
        float anchorMaxX = rectTransform.anchorMax.x + anchorVector.x + (rectTransform.offsetMax.x - pivotX) / parentRectTransform.rect.width * rectTransform.localScale.x;
        float anchorMaxY = rectTransform.anchorMax.y - (1 - anchorVector.y) + (rectTransform.offsetMax.y + pivotY) / parentRectTransform.rect.height * rectTransform.localScale.y;
        rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);

        offsetMinX = (0 - rectTransform.pivot.x) * anchorRect.width * (1 - rectTransform.localScale.x);
        offsetMinY = (0 - rectTransform.pivot.y) * anchorRect.height * (1 - rectTransform.localScale.y);
        rectTransform.offsetMin = new Vector2(offsetMinX, offsetMinY);
        offsetMaxX = (1 - rectTransform.pivot.x) * anchorRect.width * (1 - rectTransform.localScale.x);
        offsetMaxY = (1 - rectTransform.pivot.y) * anchorRect.height * (1 - rectTransform.localScale.y);
        rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
    }
}
