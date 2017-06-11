using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateGame
{
    Starting = 0,
    Gaming,
    Ending,
    Length
}

public enum StateButton
{
    None = 0,
    News,
    NewsValidate,
    FakeNews,
    FakeNewsValidate,
    Length
}

public enum StateWin
{
    None,
    FakeNews,
    TrueNews,
    Together,
    Lenght
}


public class GameScript : MonoBehaviour
{
    #region Static Attributs
    public static int WidthPad = 8;
    public static int HeightPad = 8;
    private static GameScript _Instance;
    public static GameScript Instance
    {
        get
        {
            if (GameScript._Instance == null)
                GameScript._Instance = new GameScript();
            return GameScript._Instance;
        }
    }
    #endregion

    #region Private Attributes
    private LaunchpadManager _Launchpad;
    private StateButton[,] _ButtonsState;
    private Dictionary<StateGame, State> _States;
    public StateGame _CurrentState;
    public StateWin _StateWin;
    #endregion

    void Awake()
    {
        if (GameScript._Instance == null)
            GameScript._Instance = this;
        else if (GameScript._Instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        _Launchpad = FindObjectOfType<LaunchpadManager>();
        Instantiate(Resources.Load("Prefabs/StoryGame"));
        _ButtonsState = new StateButton[WidthPad, HeightPad];
        _States = new Dictionary<StateGame, State>();
        _States.Add(StateGame.Starting, new StateStarting(_Launchpad));
        _States.Add(StateGame.Gaming, new StateGaming(_Launchpad));
        _States.Add(StateGame.Ending, new StateEnding(_Launchpad));
        _CurrentState = StateGame.Length;

        Reset();
    }

	void Update()
    {
        if (_States.ContainsKey(_CurrentState))
        {
            _States[_CurrentState].UpdateState();
            StateWin stateWin = _States[_CurrentState].ForceEnding();
            if (stateWin != StateWin.None)
                SetStateWin(stateWin);
        }
    }

    public void Reset()
    {
        _StateWin = StateWin.None;
        for (int x = 0; x < WidthPad; ++x)
        {
            for (int y = 0; y < HeightPad; ++y)
                SetDisplayButton(x, y, StateButton.None);
        }
        ChangeState(StateGame.Starting);
    }

    public void ChangeState(StateGame parNewState)
    {
        if (_States.ContainsKey(_CurrentState))
            _States[_CurrentState].EndState();
        _CurrentState = parNewState;
        if (_States.ContainsKey(_CurrentState))
            _States[_CurrentState].StartState();
    }

    public StateButton GetStateButton(int x, int y)
    {
        return _ButtonsState[x, y];
    }
    public bool SetStateButton(int x, int y, StateButton parNewState)
    {
        StateButton previousState = _ButtonsState[x, y];
        switch (previousState)
        {
            case StateButton.None:
                SetDisplayButton(x, y, parNewState);
                return true;
            case StateButton.News:
                if (parNewState == StateButton.NewsValidate)
                {
                    SetDisplayButton(x, y, parNewState);
                    return true;
                }
                break;
        }
        return false;
    }

    void SetDisplayButton(int x, int y, StateButton parNewState)
    {
        _ButtonsState[x, y] = parNewState;
        SwitchColor(x, y, parNewState);
    }

    public void SwitchColor(int x, int y, StateButton parNewState)
    {
        switch (parNewState)
        {
            case StateButton.None:
                _Launchpad.LedColor(x, y, 0);
                break;
            case StateButton.FakeNews:
                _Launchpad.LedColor(x, y, 15);
                break;
            case StateButton.FakeNewsValidate:
                _Launchpad.LedColor(x, y, 15);
                break;
            case StateButton.News:
                _Launchpad.LedColor(x, y, 16);
                break;
            case StateButton.NewsValidate:
                _Launchpad.LedColor(x, y, 16);
                break;
        }
    }

    public Vector2 GetNextNewsButton()
    {
        int currentIndex = Random.Range(0, GameScript.WidthPad * GameScript.HeightPad);
        while (true)
        {
            for (int x = 0; x < WidthPad; ++x)
            {
                for (int y = 0; y < HeightPad; ++y)
                {
                    if (_ButtonsState[x, y] == StateButton.None)
                    {
                        --currentIndex;
                        if (currentIndex < 0)
                            return new Vector2(x, y);
                    }
                }
            }
        }
    }
    
    public int NbButtons(StateButton parStateButton = StateButton.None)
    {
        int nbButtons = 0;
        for (int x = 0; x < WidthPad; ++x)
        {
            for (int y = 0; y < HeightPad; ++y)
            {
                if (_ButtonsState[x, y] == parStateButton)
                    ++nbButtons;
            }
        }
        return nbButtons;
    }

    public void SetStateWin(StateWin parStateWin)
    {
        _StateWin = parStateWin;
        Debug.Log(_StateWin);
        ChangeState(StateGame.Ending);
    }
}
