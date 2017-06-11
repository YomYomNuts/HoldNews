using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayColor : MonoBehaviour
{
    public LaunchpadManager launchpad;
    public Effects effects;
    private int[,] buttons = new int[8, 8];

    void Start ()
    {
        launchpad.OnPress += new LaunchpadManager.OnPressHandler(onPress);
        launchpad.OnRelease += new LaunchpadManager.OnReleaseHandler(onRelease);
        //effects.Activate();
    }
	
	void Update ()
    {

    }

    public void reset()
    {
        //effects.Deactivate();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                buttons[x, y] = 0;
                launchpad.LedOff(x, y);
            }
        }
    }

    private void onPress(int x, int y)
    {
        /*if (gameOver && ((x == 8) || gameOverTimer > 10))
        {
            reset();
            return;
        }*/

        if (x < 0 || x > 7 || y < 0 || y > 7)
        {
            return;
        }

        /*if (x == targetX && y == targetY && !gameOver)
        {*/
            buttons[x, y] = 1;
            launchpad.ledOnYellow(x, y);
        /*    buttonsHeld++;
            turnTimer = 10;
            startNextTurn();
        }*/
    }

    private void onRelease(int x, int y)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7)
        {
            return;
        }

        /*if (!gameOver && buttons[x, y] != 0)
        {
            gameOver = true;
            winner = (buttons[x, y] % numPlayers) + 1;*/
            //launchpad.ledOnRed(x, y);
            launchpad.LedOff(x, y);

        /*    gameOverTimer = 0;
        }*/
    }
}
