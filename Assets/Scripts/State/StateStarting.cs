using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStarting : State
{
    bool _IsInitialize;

    public StateStarting(LaunchpadManager parLaunchpad) : base(parLaunchpad) { }

    public override void StartState()
    {
        base.StartState();
        _IsInitialize = false;
    }
    public override void EndState()
    {
        base.EndState();
    }
    public override void UpdateState()
    {
        if (!_IsInitialize)
        {
            GameScript.Instance.LaunchTrigger("START");
            if (!GameScript.Instance._FirstLaunch)
                GameScript.Instance._PressValidation.Play();
            else
                GameScript.Instance._FirstLaunch = false;
            _IsInitialize = true;
        }
    }
    public override void OnPressState(int x, int y)
    {
        GameScript.Instance.ChangeState(StateGame.Gaming);
    }
    public override void OnReleaseState(int x, int y)
    {
    }
}
