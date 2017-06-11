using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effects : MonoBehaviour
{
	public LaunchpadManager launchpad;
	private bool active = false;
	private List<Effect> effects = new List<Effect>();
	private float newEffectCounter, updateCounter;
	private bool redNext = false;

	public void Activate()
    {
		active = true;
	}

	public void Deactivate()
    {
		active = false;

		foreach (Effect e in effects)
        {
			e.clear();
		}
		effects.Clear();
	}

	void Update ()
    {
		if (!active)
			return;

		newEffectCounter += Time.deltaTime;
		updateCounter += Time.deltaTime;

		if (newEffectCounter > 0.7)
        {
			effects.Add(new Effect(this, (int)(Random.value * 4) + 2, (int)(Random.value * 4) + 2, redNext));
			newEffectCounter = 0;
			redNext = !redNext;
		}

		if (updateCounter > 0.15)
        {
			//List<Effect> toRemove = new List<Effect>();

			effects.ForEach(x => x.update());
			effects.RemoveAll(x => x.getDistance() > 8);

			updateCounter = 0;
		}
	}

	public class Effect
    {
		Effects parent;
		private int x, y, distance = -1;
		private bool red;

		public Effect(Effects parent, int x, int y, bool red)
        {
			this.parent = parent;
			this.x = x;
			this.y = y;
			this.red = red;
		}

		public void update()
        {
			if (distance > -1)
				clear ();

			distance += 1;
			if (red)
            {
				parent.launchpad.ledOnRed(x + distance, y + distance);
				parent.launchpad.ledOnRed(x - distance, y + distance);
				parent.launchpad.ledOnRed(x + distance, y - distance);
				parent.launchpad.ledOnRed(x - distance, y - distance);
			} else {
				parent.launchpad.ledOnYellow(x + distance, y + distance);
				parent.launchpad.ledOnYellow(x - distance, y + distance);
				parent.launchpad.ledOnYellow(x + distance, y - distance);
				parent.launchpad.ledOnYellow(x - distance, y - distance);
			}
		}

		public void clear()
        {
			parent.launchpad.LedOff(x + distance, y + distance);
			parent.launchpad.LedOff(x - distance, y + distance);
			parent.launchpad.LedOff(x + distance, y - distance);
			parent.launchpad.LedOff(x - distance, y - distance);
		}

		public int getDistance()
        {
			return distance;
		}
	}
}
