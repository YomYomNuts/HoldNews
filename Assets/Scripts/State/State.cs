using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public LaunchpadManager _Launchpad;

    public State(LaunchpadManager parLaunchpad)
    {
        _Launchpad = parLaunchpad;
    }

    public virtual void StartState()
    {
        _Launchpad.OnPress += new LaunchpadManager.OnPressHandler(OnPress);
        _Launchpad.OnRelease += new LaunchpadManager.OnReleaseHandler(OnRelease);
    }
    public virtual void EndState()
    {
        _Launchpad.OnPress -= new LaunchpadManager.OnPressHandler(OnPress);
        _Launchpad.OnRelease -= new LaunchpadManager.OnReleaseHandler(OnRelease);
    }
    public abstract void UpdateState();
    public virtual StateWin ForceEnding() { return StateWin.None; }
    public void OnPress(int x, int y)
    {
        if (x < 0 || x >= GameScript.WidthPad || y < 0 || y >= GameScript.WidthPad)
            return;
        OnPressState(x, y);
    }
    public abstract void OnPressState(int x, int y);
    public void OnRelease(int x, int y)
    {
        if (x < 0 || x >= GameScript.WidthPad || y < 0 || y >= GameScript.WidthPad)
            return;
        OnReleaseState(x, y);
    }
    public abstract void OnReleaseState(int x, int y);
}
