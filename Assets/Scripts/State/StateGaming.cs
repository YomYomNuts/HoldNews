using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGaming : State
{
    public static int _NbPressTouchPossible = 2;
    public static int _NbPressTouch = 2;
    public static float _TimeMaxMissRelease = 1.0f;

    int _IndexTurn;
    bool _TurnNews;
    bool _ReleaseButton;
    bool _WaitingNewTurn;
    List<Vector2> _CoordinateWaitingNews = new List<Vector2>();
    float _PreviousTimeDisplay;
    bool _IsBlinking;
    List<WaintingReleaseButton> _WaintingReleaseButton = new List<WaintingReleaseButton>();

    public StateGaming(LaunchpadManager parLaunchpad) : base(parLaunchpad) { }

    public override void StartState()
    {
        base.StartState();
        _IndexTurn = -1;
        _TurnNews = false;
        _WaintingReleaseButton.Clear();
        GoNextTurn();
    }

    public override void EndState()
    {
        base.EndState();
    }

    public override void UpdateState()
    {
        StateButton state = _TurnNews ? StateButton.News : StateButton.FakeNews;
        if (!_ReleaseButton)
        {
            if (_WaitingNewTurn)
            {
                _CoordinateWaitingNews.Clear();
                int nbButtonsToDisplay = GameScript.Instance.NbButtons();
                nbButtonsToDisplay = nbButtonsToDisplay > _NbPressTouchPossible ? _NbPressTouchPossible : nbButtonsToDisplay;
                for (int i = 0; i < _NbPressTouchPossible; ++i)
                {
                    _CoordinateWaitingNews.Add(GameScript.Instance.GetNextNewsButton());
                    GameScript.Instance.SwitchColor((int)_CoordinateWaitingNews[i].x, (int)_CoordinateWaitingNews[i].y, state);
                }
                _PreviousTimeDisplay = Time.time;
                _IsBlinking = true;
                _WaitingNewTurn = false;
            }
            if (Time.time > _PreviousTimeDisplay + 0.5f)
            {
                StateButton currentStateBlink = (_IsBlinking ? StateButton.None : state);
                for (int i = 0; i < _CoordinateWaitingNews.Count; ++i)
                    GameScript.Instance.SwitchColor((int)_CoordinateWaitingNews[i].x, (int)_CoordinateWaitingNews[i].y, currentStateBlink);
                _PreviousTimeDisplay = Time.time;
                _IsBlinking = !_IsBlinking;
            }
        }
    }

    public override StateWin ForceEnding()
    {
        //Miss Release
        for (int i = 0; i < _WaintingReleaseButton.Count; ++i)
        {
            if (_WaintingReleaseButton[i].Update())
            {
                StateButton stateButton = GameScript.Instance.GetStateButton(_WaintingReleaseButton[i].x, _WaintingReleaseButton[i].y);
                return (stateButton == StateButton.News ? StateWin.FakeNews : StateWin.TrueNews);
            }
        }
        return StateWin.None;
    }

    public override void OnPressState(int x, int y)
    {
        //Miss Release
        for (int i = 0; i < _WaintingReleaseButton.Count; ++i)
        {
            WaintingReleaseButton wrb = _WaintingReleaseButton[i];
            if (wrb.x == x && wrb.y == y)
            {
                _WaintingReleaseButton.RemoveAt(i);
                --i;
            }
        }

        bool isCorrect = false;
        for (int i = 0; i < _CoordinateWaitingNews.Count; ++i)
        {
            if (x == _CoordinateWaitingNews[i].x && y == _CoordinateWaitingNews[i].y)
            {
                isCorrect = true;
                break;
            }
        }
        if (!isCorrect)
            return;
        StateButton state = _TurnNews ? StateButton.News : StateButton.FakeNews;
        for (int i = 0; i < _CoordinateWaitingNews.Count; ++i)
            GameScript.Instance.SwitchColor((int)_CoordinateWaitingNews[i].x, (int)_CoordinateWaitingNews[i].y, StateButton.None);
        _CoordinateWaitingNews.Clear();
        bool work = GameScript.Instance.SetStateButton(x, y, state);
        if (work)
            GoNextTurn();
    }
    public override void OnReleaseState(int x, int y)
    {
        StateButton state = GameScript.Instance.GetStateButton(x, y);
        if (state != StateButton.None)
        {
            bool isCorrectTurn = (_TurnNews && state == StateButton.News) || (!_TurnNews && state == StateButton.FakeNews);
            if (_ReleaseButton && isCorrectTurn)
            {
                StateButton newState = state == StateButton.News ? StateButton.NewsValidate : StateButton.FakeNewsValidate;
                GameScript.Instance.SetStateButton(x, y, newState);
                GoNextTurn();
            }
            else
                _WaintingReleaseButton.Add(new WaintingReleaseButton(x, y));
        }
    }

    void GoNextTurn()
    {
        ++_IndexTurn;
        _TurnNews = !_TurnNews;
        _ReleaseButton = (_IndexTurn >= _NbPressTouch * 2 && (_IndexTurn % 4) >= 2);
        _WaitingNewTurn = true;

        // Win together
        if (GameScript.Instance.NbButtons() == 0)
            GameScript.Instance.SetStateWin(StateWin.Together);
    }
}

public class WaintingReleaseButton
{
    public int x;
    public int y;
    bool _IsInitialize;
    float _PreviousTimer;

    public WaintingReleaseButton(int parX, int parY)
    {
        x = parX;
        y = parY;
        _IsInitialize = false;
    }

    public bool Update()
    {
        if (!_IsInitialize)
        {
            _PreviousTimer = Time.time;
            _IsInitialize = true;
        }
        return Time.time > _PreviousTimer + StateGaming._TimeMaxMissRelease;
    }
}
