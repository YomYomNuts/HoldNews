using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour
{
    #region Static Attributs
    private static StoryScript _Instance;
    public static StoryScript Instance
    {
        get
        {
            if (StoryScript._Instance == null)
                StoryScript._Instance = new StoryScript();
            return StoryScript._Instance;
        }
    }
    #endregion

    public List<Turn> _Turns = new List<Turn>();

    void Awake()
    {
        if (StoryScript._Instance == null)
            StoryScript._Instance = this;
        else if (StoryScript._Instance != this)
            Destroy(this.gameObject);
    }

    void Start ()
    {
	}
	
	void Update ()
    {
	}
}
