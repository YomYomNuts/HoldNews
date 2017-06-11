using UnityEngine;
using System.Collections;
using Midi;
using System;

public class LaunchpadManager : MonoBehaviour
{
    private OutputDevice launchpadOutput;
    private InputDevice launchpadInput;

    private bool ready = false;

    public bool Ready
    {
        get { return ready; }
    }

    void Awake()
    {
        InitLaunchpad();
    }

    private void InitLaunchpad()
    {
        if (ready)
            return;

        if (launchpadOutput != null && launchpadOutput.IsOpen)
            launchpadOutput.Close();

        if (launchpadInput != null && launchpadInput.IsOpen)
            launchpadInput.Close();

        launchpadOutput = null;
        launchpadInput = null;

        Debug.Log("Looking for output device...");

        foreach (OutputDevice device in OutputDevice.InstalledDevices)
        {
            if (device.Name.Contains("Launchpad"))
            {
                launchpadOutput = device;
                Debug.Log("Launchpad output found!");
            }
        }

        if (launchpadOutput == null)
        {
            Debug.Log("Unable to find launchpad output :(");
            ready = false;
            return;
        }

        Debug.Log("Looking for input device...");

        foreach (InputDevice device in InputDevice.InstalledDevices)
        {
            if (device.Name.Contains("Launchpad"))
            {
                launchpadInput = device;
                Debug.Log("Launchpad input found!");
            }
        }

        if (launchpadInput == null)
        {
            Debug.Log("Unable to find launchpad input :(");
            ready = false;
            return;
        }

        launchpadOutput.Open();

        launchpadInput.Open();
        ready = true;
        launchpadInput.NoteOn += new InputDevice.NoteOnHandler(MyNoteOn);
        launchpadInput.StartReceiving(null);
    }

    public delegate void OnPressHandler(int x, int y);
    public event OnPressHandler OnPress;
    public delegate void OnReleaseHandler(int x, int y);
    public event OnReleaseHandler OnRelease;
    private void MyNoteOn(NoteOnMessage msg)
    {
        if (msg.Velocity > 0)
        {
            OnPress((int)msg.Pitch % 16, 7 - (int)msg.Pitch / 16);
        }
        else
        {
            OnRelease((int)msg.Pitch % 16, 7 - (int)msg.Pitch / 16);
        }
    }

    public int valueColor = 58;
    public void ledOnYellow(int x, int y)
    {
        if (x < 0 || x > 8 || y < 0 || y > 7)
        {
            //Debug.Log(new System.ArgumentException("Button out of bounds"));
            return;
        }
        try
        {
            //launchpadOutput.SendNoteOn(Channel.Channel1, (Pitch)(x + 16 * (7 - y)), 58);
            if (launchpadOutput != null)
                launchpadOutput.SendNoteOn(Channel.Channel1, (Pitch)(x + 16 * (7 - y)), valueColor);
        }
        catch (DeviceException)
        {
            ready = false;
            launchpadOutput = null;
            launchpadInput = null;
        }
    }
    public void ledOnRed(int x, int y)
    {
        if (x < 0 || x > 8 || y < 0 || y > 7)
        {
            //Debug.Log(new System.ArgumentException("Button out of bounds"));
            return;
        }

        try
        {
            if (launchpadOutput != null)
                launchpadOutput.SendNoteOn(Channel.Channel1, (Pitch)(x + 16 * (7 - y)), 15);
        }
        catch (DeviceException)
        {
            ready = false;
            launchpadOutput = null;
            launchpadInput = null;
        }
    }

    public void LedOff(int x, int y)
    {
        if (x < 0 || x > 8 || y < 0 || y > 7)
            return;

        try
        {
            if (launchpadOutput != null)
                launchpadOutput.SendNoteOn(Channel.Channel1, (Pitch)(x + 16 * (7 - y)), 0);
        }
        catch (DeviceException)
        {
            ready = false;
            launchpadOutput = null;
            launchpadInput = null;
        }
    }

    public void LedColor(int x, int y, int parColor)
    {
        if (x < 0 || x >= GameScript.WidthPad || y < 0 || y >= GameScript.HeightPad)
            return;

        try
        {
            if (launchpadOutput != null)
                launchpadOutput.SendNoteOn(Channel.Channel1, (Pitch)(x + 16 * (7 - y)), parColor);
        }
        catch (DeviceException)
        {
            ready = false;
            launchpadOutput = null;
            launchpadInput = null;
        }
    }

    void OnApplicationQuit()
    {
        if (ready)
        {
            launchpadOutput.SendControlChange(Channel.Channel1, 0, 0);
            launchpadOutput.Close();
            launchpadInput.StopReceiving();
            launchpadInput.Close();
        }
    }
}
