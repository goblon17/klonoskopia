using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : Singleton<HumanController>
{
    [SerializeField]
    private RectTransform minigameParent;
    [SerializeField]
    private MinigameDoors doors;
    [SerializeField]
    private float waitBetweenMinigames;
    [SerializeField]
    private float waitTime;

    private IEnumerable<Minigame> minigames;
    private List<string> answers = new List<string>();

    private Coroutine minigameCoroutine;
    private bool nextMinigame = false;

    private void OnEnable()
    {
        if (!GameManager.IsInitialized)
        {
            return;
        }

        if (GameManager.Instance.CurrentScene != GameManager.Scene.Human)
        {
            return;
        }

        minigameCoroutine = StartCoroutine(MinigameCoroutine());
    }

    private void OnDisable()
    {
        foreach (Transform child in minigameParent)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator MinigameCoroutine()
    {
        GameManager.Instance.SetupLevel();
        minigames = GameManager.Instance.GetMinigames();
        answers.Clear();
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

        GameManager.Instance.SetAnswers(answers);
        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.ChangeScene(GameManager.Scene.Clone);
        minigameCoroutine = null;
    }

    private void OnMinigameEnd(Minigame minigame, string answer)
    {
        if (VerifyAnswer(minigame, answer))
        {
            answers.Add(answer);
            nextMinigame = true;
        }
        else
        {
            minigame.ResetMinigame();
            minigame.ShowError();
        }
    }

    private bool VerifyAnswer(Minigame minigame, string answer)
    {
        switch (minigame)
        {
            case PinMinigame pin:
                return answer.Length == 4;
        }
        return false;
    }
}
