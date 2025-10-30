using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : Singleton<CloneController>
{
    [SerializeField]
    private RectTransform minigameParent;
    [SerializeField]
    private MinigameDoors doors;
    [SerializeField]
    private float waitBetweenMinigames;
    [SerializeField]
    private float waitTime;

    private List<Minigame> minigames;

    private Coroutine minigameCoroutine;
    private bool nextMinigame = false;

    private void OnEnable()
    {
        if (!GameManager.IsInitialized)
        {
            return;
        }

        if (GameManager.Instance.CurrentScene != GameManager.Scene.Clone)
        {
            return;
        }

        minigameCoroutine = StartCoroutine(MinigameCoroutine());
    }

    private void OnDisable()
    {
        if (minigameCoroutine != null)
        {
            StopCoroutine(minigameCoroutine);
            minigameCoroutine = null;
        }
        foreach (Transform child in minigameParent)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator MinigameCoroutine()
    {
        minigames = GameManager.Instance.GetMinigames();
        minigames.ShuffleSelf();

        doors.ShowMessage(MinigameDoors.Message.Dots);
        doors.CloseInstantly();

        yield return new WaitForSeconds(waitTime);

        foreach (Minigame minigame in minigames)
        {
            foreach (Transform child in minigameParent)
            {
                Destroy(child.gameObject);
            }

            Minigame currentMinigame = Instantiate(minigame.gameObject, minigameParent).GetComponent<Minigame>();
            currentMinigame.MinigameEnded += OnMinigameEnd;
            doors.Open();

            yield return new WaitUntil(() => nextMinigame);
            nextMinigame = false;

            yield return doors.Close();

            yield return new WaitForSeconds(waitBetweenMinigames);
        }

        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.NextLevel();

        minigameCoroutine = null;
    }

    private void OnMinigameEnd(Minigame minigame, string answer)
    {
        if (GameManager.Instance.RegisterAnswer(minigame, answer))
        {
            doors.ShowMessage(MinigameDoors.Message.Correct);
            nextMinigame = true;
        }
        else
        {
            StartCoroutine(LoseCoroutine());
        }
    }

    private IEnumerator LoseCoroutine()
    {
        doors.ShowMessage(MinigameDoors.Message.Wrong);

        yield return doors.Close();

        yield return new WaitForSeconds(waitTime + waitBetweenMinigames);

        GameManager.Instance.ChangeScene(GameManager.Scene.Lose);
    }
}
