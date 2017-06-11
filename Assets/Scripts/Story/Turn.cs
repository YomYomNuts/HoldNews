using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateTurn
{
    FakeNews = 0,
    TrueNews
}

[System.Serializable]
public class Turn
{
    public StateTurn _State;
    public int _NbPressTouchPossible;
    public int _NbReleaseTouch;
    public bool _BonusFake;

    public Turn()
    {
        _State = StateTurn.FakeNews;
        _NbPressTouchPossible = 0;
        _NbReleaseTouch = 0;
        _BonusFake = false;
    }
    public Turn(Turn parTurn)
    {
        _State = parTurn._State;
        _NbPressTouchPossible = parTurn._NbPressTouchPossible;
        _NbReleaseTouch = parTurn._NbReleaseTouch;
        _BonusFake = parTurn._BonusFake;
    }
}
