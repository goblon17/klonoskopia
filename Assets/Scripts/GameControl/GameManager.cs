using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum Scene { Menu, Win, Lose, Human, Clone, Cutscene }

    [System.Serializable]
    public struct Level
    {
        public int minigameCount;
        public bool repeatMinigames;
    }

    [SerializeField]
    private SerializedDictionary<Scene, GameObject> scenes;

    [SerializeField]
    private List<Level> levels;
    [SerializeField]
    private List<Minigame> minigames;

    public event ObservableVariable<int>.ValueChangedDelegate LevelChanged
    {
        add => currentLevel.ValueChanged += value;
        remove => currentLevel.ValueChanged -= value;
    }

    public Scene CurrentScene => currentScene;

    private Scene currentScene = Scene.Menu;

    private ObservableVariable<int> currentLevel = new ObservableVariable<int>(0);
    private List<(Minigame minigame, string answer)> answers = new List<(Minigame minigame, string answer)>();

    private void Start()
    {
        SetScene(Scene.Menu);
    }

    public void ChangeScene(Scene scene)
    {
        if (currentScene == scene)
        {
            return;
        }

        SetScene(scene);
    }

    private void SetScene(Scene scene)
    {
        currentScene = scene;

        foreach (GameObject item in scenes.Values)
        {
            item.SetActive(false);
        }
        scenes[scene].SetActive(true);
    }

    public void SetupLevel()
    {
        answers.Clear();
        Level level = levels[currentLevel.Value];

        if (level.repeatMinigames)
        {
            while (answers.Count < level.minigameCount)
            {
                answers.Add((minigames.GetRandomElement(), null));
            }
            if (currentLevel.Value == 2 && answers.Count > 2)
            {
                answers[2] = answers[0];
            }
        }
        else
        {
            List<Minigame> minigames = this.minigames;
            minigames.ShuffleSelf();
            foreach (Minigame minigame in minigames.Take(level.minigameCount))
            {
                answers.Add((minigame, null));
            }
        }

        // there are no more unique minigames so we randomize the rest
        while (answers.Count < level.minigameCount)
        {
            answers.Add((minigames.GetRandomElement(), null));
        }
    }

    public List<Minigame> GetMinigames()
    {
        return answers.Select(e => e.minigame).ToList();
    }

    public void SetAnswers(IEnumerable<string> answers)
    {
        if (answers.Count() != this.answers.Count())
        {
            throw new System.Exception("Fuck you");
        }

        int i = 0;
        foreach (string answer in answers)
        {
            var entry = this.answers[i];
            entry.answer = answer;
            this.answers[i] = entry;
            i++;
        }
    }

    public bool RegisterAnswer(Minigame minigame, string answer)
    {
        int count = answers.Count;
        for (int i = 0; i < count; i++)
        {
            if (answers[i].minigame.GetType() == minigame.GetType() && answers[i].answer != null)
            {
                if (answers[i].answer == answer)
                {
                    answers[i] = (answers[i].minigame, null);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public void NextLevel()
    {
        currentLevel.Value++;
        if (currentLevel.Value < levels.Count)
        {
            ChangeScene(Scene.Human);
        }
        else
        {
            ChangeScene(Scene.Win);
        }
    }
}
