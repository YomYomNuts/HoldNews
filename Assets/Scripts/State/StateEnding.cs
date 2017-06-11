using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnding : State
{
    static float _TimerBeforePressEnd = 2.0f;
    float _CurrentTimer;
    bool _WaitPress;

    public StateEnding(LaunchpadManager parLaunchpad) : base(parLaunchpad) { }

    public override void StartState()
    {
        base.StartState();
        _CurrentTimer = _TimerBeforePressEnd;
        _WaitPress = false;
    }
    public override void EndState()
    {
        base.EndState();
    }
    public override void UpdateState()
    {
        _CurrentTimer -= Time.deltaTime;
        if (_CurrentTimer <= 0.0f)
            _WaitPress = true;
    }
    public override void OnPressState(int x, int y)
    {
        if (_WaitPress)
            GameScript.Instance.Reset();
    }
    public override void OnReleaseState(int x, int y)
    {
    }
}
