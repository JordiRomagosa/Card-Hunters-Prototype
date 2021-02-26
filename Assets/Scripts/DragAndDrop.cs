using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
   
    public CardUpdater cardValues;

    private bool isDragging = false;
    private Vector2 startPosition;

    void Update()
    {
        if (isDragging)
        {
            GameObject gameController = GameObject.Find("GameController");
            if (gameController)
            {
                Vector2 mousePos = gameController.GetComponent<GameController>().GetCanvasMousePosition();
                transform.position = mousePos;
            }
        }
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        GameObject gameController = GameObject.Find("GameController");
        if (gameController)
        {
            if (!gameController.GetComponent<GameController>().DropCard(cardValues.card.cardID))
            {
                transform.position = startPosition;
            }
        }
    }
}
