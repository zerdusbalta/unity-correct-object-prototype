using UnityEngine;
using TMPro;

public class DragObject : MonoBehaviour
{
    private bool isDragging;
    private bool isWrongFeedback;

    private Vector3 offset;
    private Vector3 startPosition;

    private float wrongTimer = 0f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Color highlightColor;

    private TextMeshProUGUI feedbackText;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        originalColor = sr.color;
        highlightColor = Color.Lerp(originalColor, Color.white, 0.35f);
        highlightColor.a = originalColor.a;

        GameObject feedbackObject = GameObject.Find("Text (TMP)(FeedBackText)");
        if (feedbackObject != null)
        {
            feedbackText = feedbackObject.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();

        if (isWrongFeedback)
        {
            wrongTimer -= Time.deltaTime;
            sr.color = Color.red;

            if (wrongTimer <= 0f)
            {
                sr.color = originalColor;
                isWrongFeedback = false;
                transform.position = startPosition;

                if (feedbackText != null)
                {
                    feedbackText.text = "Try again!";
                }
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorld;
                sr.color = highlightColor;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mouseWorld + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            sr.color = originalColor;

            DraggableItem item = GetComponent<DraggableItem>();
            Collider2D itemCollider = GetComponent<Collider2D>();

            DropZone matchedZone = FindMatchingZone(itemCollider);

            if (matchedZone != null && item != null)
            {
                if (matchedZone.IsCorrect(item))
                {
                    transform.position = matchedZone.GetSnapPosition();

                    if (feedbackText != null)
                    {
                        if (gameObject.name == "Fish" && matchedZone.acceptedType == "Food")
                        {
                            feedbackText.text = "Correct! Fish is food too.";
                        }
                        else if (gameObject.name == "Fish" && matchedZone.acceptedType == "Animal")
                        {
                            feedbackText.text = "Correct! Fish is also an animal.";
                        }
                        else if (gameObject.name == "Apple")
                        {
                            feedbackText.text = "Good job! Apple is food.";
                        }
                        else if (gameObject.name == "Butterfly")
                        {
                            feedbackText.text = "Good job! Butterfly is an animal.";
                        }
                        else
                        {
                            feedbackText.text = "Correct!";
                        }
                    }

                    Debug.Log("Correct item!");
                }
                else
                {
                    isWrongFeedback = true;
                    wrongTimer = 0.2f;
                    Debug.Log("Wrong item!");
                }
            }
            else
            {
                transform.position = startPosition;
            }
        }
    }

    private DropZone FindMatchingZone(Collider2D itemCollider)
    {
        DropZone[] allZones = FindObjectsOfType<DropZone>();

        foreach (DropZone zone in allZones)
        {
            if (zone.IsMoreThanHalfInside(itemCollider))
            {
                return zone;
            }
        }

        return null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}