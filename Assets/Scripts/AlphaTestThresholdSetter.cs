using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaTestThresholdSetter : MonoBehaviour
{
    [SerializeField]
    private float threshold;

    private void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
    }
}
