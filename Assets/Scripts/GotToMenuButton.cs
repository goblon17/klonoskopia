using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GotToMenuButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ChangeScene(GameManager.Scene.Menu));
    }
}
