using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStarting : State
{
    public StateStarting(LaunchpadManager parLaunchpad) : base(parLaunchpad) { }

    public override void StartState()
    {
        base.StartState();
    }
    public override void EndState()
    {
        base.EndState();
    }
    public override void UpdateState()
    {
    }
    public override void OnPressState(int x, int y)
    {
        GameScript.Instance.ChangeState(StateGame.Gaming);
    }
    public override void OnReleaseState(int x, int y)
    {
    }
}
