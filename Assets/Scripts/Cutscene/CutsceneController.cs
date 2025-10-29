using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : Singleton<CutsceneController>
{
    [System.Serializable]
    private struct Frame
    {
        public float duration;
        public Sprite sprite;
        [TextArea]
        public string text;
        public AudioClip clip;
    }

    [SerializeField]
    private List<Frame> frames;

    [Header("References")]
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private AudioSource audioSource;

    private Coroutine cutsceneRoutine = null;

    private void OnEnable()
    {
        if (!GameManager.IsInitialized)
        {
            return;
        }

        if (GameManager.Instance.CurrentScene != GameManager.Scene.Cutscene)
        {
            return;
        }

        cutsceneRoutine = StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        foreach (Frame frame in frames)
        {
            if (frame.sprite != null)
            {
                image.sprite = frame.sprite;
            }

            text.text = frame.text;

            if (frame.clip != null)
            {
                audioSource.PlayOneShot(frame.clip);
            }

            yield return new WaitForSeconds(frame.duration);
        }

        EndCutscene();
    }

    private void EndCutscene()
    {
        if (cutsceneRoutine != null)
        {
            StopCoroutine(cutsceneRoutine);
            cutsceneRoutine = null;
        }

        GameManager.Instance.ChangeScene(GameManager.Scene.Human);
    }
}
