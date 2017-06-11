using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class StoryEditorWindow : EditorWindow
{
    List<TurnEditor> turnsEditor = new List<TurnEditor>();
    Vector2 scrollPos;

    [MenuItem("Turn/StoryEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StoryEditorWindow));
    }

    void Load()
    {
        GameObject storyGame = Instantiate(Resources.Load("Prefabs/StoryGame")) as GameObject;
        storyGame.name = "StoryGame";
        StoryScript storyScript = storyGame.GetComponent<StoryScript>();

        foreach (Turn t in storyScript._Turns)
        {
            TurnEditor newTurnEditor = new TurnEditor(t);
            newTurnEditor._AnimBool.valueChanged.AddListener(Repaint);
            turnsEditor.Add(newTurnEditor);
        }

        // Save the changement
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/" + storyGame.name + ".prefab", storyGame);
#endif
        float timeStart = Time.time;
        while (Time.time - timeStart > 1.0f) { }
        DestroyImmediate(storyGame);
        Debug.Log("Story Load");
    }

    void Save()
    {
        GameObject storyGame = Instantiate(Resources.Load("Prefabs/StoryGame")) as GameObject;
        storyGame.name = "StoryGame";
        StoryScript storyScript = storyGame.GetComponent<StoryScript>();

        storyScript._Turns.Clear();
        foreach (TurnEditor te in turnsEditor)
            storyScript._Turns.Add(te._Turn);

        // Save the changement
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/" + storyGame.name + ".prefab", storyGame);
#endif
        float timeStart = Time.time;
        while (Time.time - timeStart > 1.0f) { }
        DestroyImmediate(storyGame);
        Debug.Log("Story Save");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load"))
            Load();
        if (GUILayout.Button("Save"))
            Save();

        int newCount = Mathf.Max(0, EditorGUILayout.IntField("Number Turns", turnsEditor.Count));
        while (newCount < turnsEditor.Count)
            turnsEditor.RemoveAt(turnsEditor.Count - 1);
        while (newCount > turnsEditor.Count)
        {
            TurnEditor newTurnEditor;
            if (turnsEditor.Count > 0)
                newTurnEditor = new TurnEditor(turnsEditor[turnsEditor.Count - 1]);
            else
                newTurnEditor = new TurnEditor();
            newTurnEditor._AnimBool.valueChanged.AddListener(Repaint);
            turnsEditor.Add(newTurnEditor);
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < turnsEditor.Count; ++i)
        {
            TurnEditor currNiveau = turnsEditor[i];
            currNiveau._AnimBool.target = EditorGUILayout.ToggleLeft("Turn " + i, currNiveau._AnimBool.target);
            if (EditorGUILayout.BeginFadeGroup(currNiveau._AnimBool.faded))
            {
                ++EditorGUI.indentLevel;

                Turn turn = currNiveau._Turn;
                turn._State = (StateTurn)EditorGUILayout.EnumPopup("State", turn._State);
                turn._NbPressTouchPossible = Mathf.Max(0, EditorGUILayout.IntField("Nb Press Touch", turn._NbPressTouchPossible));
                if (turn._State == StateTurn.TrueNews)
                    turn._NbReleaseTouch = Mathf.Max(0, EditorGUILayout.IntField("Nb Release Touch", turn._NbReleaseTouch));
                if (turn._State == StateTurn.FakeNews)
                    turn._BonusFake = EditorGUILayout.Toggle("Bonus Fake", turn._BonusFake);

                --EditorGUI.indentLevel;
            }
            EditorGUILayout.EndFadeGroup();
        }
        EditorGUILayout.EndScrollView();
    }
}

[System.Serializable]
public class TurnEditor : Object
{
    public Turn _Turn;
    public AnimBool _AnimBool;

    public TurnEditor()
    {
        _Turn = new Turn();
        _AnimBool = new AnimBool();
    }
    public TurnEditor(Turn parTurn)
    {
        _Turn = new Turn(parTurn);
        _AnimBool = new AnimBool();
    }
    public TurnEditor(TurnEditor parTurnEditor) : this(parTurnEditor._Turn)
    {
    }
}
