using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectNumberScript : MonoBehaviour
{
    int addedNumber;
    bool isMouseOver = false;
    bool isSelected = false;
    RectTransform rectTransform;
    [SerializeField]
    float index = 0;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = new Vector3((Camera.main.pixelWidth / 2f) + index * 150f, (Camera.main.pixelHeight / 2f) - 450f, 0f);
        float test = Random.value;
        if (test > 0.99f)
        {
            addedNumber = 3;
        }
        else if (test > 0.66f)
        {
            addedNumber = 2;
        }
        else
        {
            addedNumber = 1;
        }
        GetComponent<Text>().text = "+" + addedNumber.ToString();
        StartCoroutine(EnterAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0) && !GameManager.splittingInProgress)
        {
            isSelected = true;
            
        }
        if (isSelected)
        {
            if (Input.GetMouseButton(0))
            {
                rectTransform.position = Input.mousePosition;
            }
            else
            {
                isSelected = false;
                RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    hit.collider.gameObject.GetComponent<AddNumberScript>().AddNumber(addedNumber);
                    hit.collider.gameObject.GetComponent<AddNumberScript>().justGotHit = true;
                    Instantiate(gameObject, transform.parent);
                    Destroy(gameObject);
                }
                else
                {
                    rectTransform.position = initialPosition;
                }
            }
        }
    }

    public void SetIsMouseOver(bool value)
    {
        isMouseOver = value;
    }

    private IEnumerator EnterAnimation()
    {
        rectTransform.position = initialPosition - 200f * Vector3.up;
        float timeElapsed = 0f;
        while (timeElapsed < 0.5f)
        {
            rectTransform.position = SmoothAnim(initialPosition - 200f * Vector3.up, initialPosition + 50f * Vector3.up, timeElapsed * 2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.position = initialPosition + 50f * Vector3.up;
        timeElapsed = 0f;
        while (timeElapsed < 0.25f)
        {
            rectTransform.position = SmoothAnim(initialPosition + 50f * Vector3.up, initialPosition, timeElapsed * 4f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.position = initialPosition;
    }

    private Vector3 SmoothAnim(Vector3 a, Vector3 b, float t)
    {
        return ((a + b) / 2f) - ((b - a) * Mathf.Cos(t * Mathf.PI) / 2f);
    }
}
