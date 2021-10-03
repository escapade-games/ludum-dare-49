using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<AddNumberScript> addNumberScripts = new List<AddNumberScript>();
    public static bool triggerSplitting = false;
    public static bool splittingInProgress = false;
    int splittingIterations = 0;
    [SerializeField]
    private Text iterationsText;
    int score = 0;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameOverStuff;
    [SerializeField]
    private GameObject nonGameOverStuff;
    private AudioManager audioManager;
    TutorialScript tutorialScript;
    [SerializeField]
    private Text restartText;
    [SerializeField]
    private BoxCollider2D[] bColliders = new BoxCollider2D[8];
    int lastSlotIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject slotText in GameObject.FindGameObjectsWithTag("Slot Texts"))
        {
            addNumberScripts.Add(slotText.GetComponent<AddNumberScript>());
        }
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        tutorialScript = GetComponent<TutorialScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerSplitting)
        {
            triggerSplitting = false;
            splittingInProgress = false;
            foreach (AddNumberScript addNumberScript in addNumberScripts)
            {
                if (addNumberScript.number % 2 == 0 && addNumberScript.number != 0)
                {
                    addNumberScript.DoSplitNumber();
                    splittingInProgress = true;
                }
            }
            if (splittingInProgress)
            {
                splittingIterations++;
                iterationsText.text = splittingIterations.ToString();
                audioManager.Play("Split");
                if (splittingIterations >= 49)
                {
                    StartCoroutine(GameOver());
                }
            }
            else
            {
                splittingIterations = 0;
                iterationsText.text = splittingIterations.ToString();
                tutorialScript.tutorialIndex++;
                foreach (AddNumberScript addNumberScript in addNumberScripts)
                {
                    if (addNumberScript.number > score)
                    {
                        score = addNumberScript.number;
                        scoreText.text = score.ToString();
                        if (score == 0)
                        {
                            scoreText.fontSize = 192; // By right this shouldn't happen
                        }
                        else
                        {
                            switch (Mathf.FloorToInt(Mathf.Log10(score)) / 2)
                            {
                                case 0:
                                    scoreText.fontSize = 192;
                                    break;
                                case 1:
                                    scoreText.fontSize = 128;
                                    break;
                                case 2:
                                    scoreText.fontSize = 96;
                                    break;
                                default:
                                    scoreText.fontSize = 64;
                                    break;
                            }
                        }
                    }
                    StartCoroutine(addNumberScript.FadeColour());
                }
            }
        }
    }

    public void DeactivateCollider(int index)
    {
        bColliders[lastSlotIndex].enabled = true;
        bColliders[index].enabled = false;
        lastSlotIndex = index;
    }

    private IEnumerator GameOver()
    {
        gameOverStuff.SetActive(true);
        nonGameOverStuff.SetActive(false);
        yield return new WaitForSeconds(1f);
        float timeElapsed = 0f;
        while (timeElapsed < 2f)
        {
            scoreText.rectTransform.position = SmoothAnim(scoreText.rectTransform.position, new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0f), timeElapsed / 2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        scoreText.rectTransform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0f);
        timeElapsed = 0f;
        while (timeElapsed < 2f)
        {
            restartText.GetComponent<Text>().color = Color.Lerp(Color.black, Color.white, timeElapsed * 2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        restartText.GetComponent<Text>().color = Color.white;
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        splittingInProgress = false;
    }

    private Vector3 SmoothAnim(Vector3 a, Vector3 b, float t)
    {
        return ((a + b) / 2f) - ((b - a) * Mathf.Cos(t * Mathf.PI) / 2f);
    }
}
