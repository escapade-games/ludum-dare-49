using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [HideInInspector]
    public int tutorialIndex = 0;
    int tutorialIndex2 = 0;
    [SerializeField]
    private Text tutorialText;
    string[] tutorialStrings = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        tutorialStrings[0] = "Drag and drop any of the bottom numbers into one of the 8 circles";
        tutorialStrings[1] = "However, you can't place a number into the same circle two times in a row";
        tutorialStrings[2] = "Even numbers split in half, giving one half to each of their neighbours";
        tutorialStrings[3] = "It is possible for the numbers to split for many iterations. After 49 iterations, the whole thing becomes unstable and it is game over";
        tutorialStrings[4] = "Try to maximise your score. Good luck!";
        tutorialText.text = tutorialStrings[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialIndex != tutorialIndex2 && tutorialIndex <= 5)
        {
            tutorialIndex2 = tutorialIndex;
            if (tutorialIndex > 4)
            {
                tutorialText.text = "";
                tutorialText.enabled = false;
            }
            else
            {
                tutorialText.text = tutorialStrings[tutorialIndex];
            }
        }
    }
}
