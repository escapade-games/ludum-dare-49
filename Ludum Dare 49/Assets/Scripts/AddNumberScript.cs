using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNumberScript : MonoBehaviour
{
    [HideInInspector]
    public int number = 0;
    Text numberText;
    [SerializeField]
    GameObject adjacentSlot;
    [SerializeField]
    GameObject adjacentSlot2;
    [SerializeField]
    GameObject plusTextPrefab;
    [SerializeField]
    GameObject slot;
    SpriteRenderer slotSprite;
    GameManager gameManager;
    [HideInInspector]
    public bool justGotHit = false;
    [SerializeField]
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        numberText = GetComponent<Text>();
        slotSprite = slot.GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (justGotHit)
        {
            justGotHit = false;
            gameManager.DeactivateCollider(index);
        }
    }

    public void AddNumber(int addedNumber)
    {
        number += addedNumber;
        numberText.text = number.ToString();
        GameManager.triggerSplitting = true;
    }

    public void DoSplitNumber()
    {
        StartCoroutine(SplitNumber(adjacentSlot.transform.position, adjacentSlot2.transform.position));
    }

    private IEnumerator SplitNumber(Vector3 adjacentSlotPos, Vector3 adjacentSlot2Pos)
    {
        int splitNumber = number / 2;
        number = 0;
        numberText.text = "0";
        GameObject split = Instantiate(plusTextPrefab, transform.parent);
        split.GetComponent<Text>().text = splitNumber.ToString();
        GameObject split2 = Instantiate(plusTextPrefab, transform.parent);
        split2.GetComponent<Text>().text = splitNumber.ToString();
        float timeElapsed = 0f;
        while (timeElapsed < 0.25f)
        {
            split.transform.position = Vector3.Lerp(transform.position, adjacentSlotPos, timeElapsed * 4f);
            split2.transform.position = Vector3.Lerp(transform.position, adjacentSlot2Pos, timeElapsed * 4f);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        Destroy(split);
        Destroy(split2);
        adjacentSlot.GetComponent<AddNumberScript>().AddNumber(splitNumber);
        adjacentSlot2.GetComponent<AddNumberScript>().AddNumber(splitNumber);
    }

    public IEnumerator FadeColour()
    {
        Color newColor = Color.white;
        if (number == 0)
        {
            newColor.a = 0.08f;
        }
        else
        {
            switch (number % 3)
            {
                case 1:
                    newColor = Color.red;
                    break;
                case 2:
                    newColor = Color.blue;
                    break;
                case 0:
                    newColor = Color.green;
                    break;
            }
        }
        float timeElapsed = 0f;
        while (timeElapsed < 1f)
        {
            slotSprite.color = Color.Lerp(slotSprite.color, newColor, timeElapsed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        slotSprite.color = newColor;
    }
}
