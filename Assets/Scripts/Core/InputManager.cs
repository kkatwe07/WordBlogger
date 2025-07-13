using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private List<LetterTile> selectedTiles = new List<LetterTile>();
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedTiles.Clear();
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            LetterTile tile = GetTileUnderPointer();
            if (tile != null && !selectedTiles.Contains(tile))
            {
                selectedTiles.Add(tile);
                tile.background.color = Color.yellow;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            string word = string.Join("", selectedTiles.ConvertAll(t => t.Letter.ToString()));

            if (!GameManager.Instance.WordValidator.IsValidWord(word))
            {
                // Invalid word — reset tile colors
                foreach (var tile in selectedTiles)
                {
                    tile.ResetColor();
                }
                selectedTiles.Clear();
                return;
            }

            GameManager.Instance.SubmitWord(word, selectedTiles);

            // Reset will be handled by GameMode (destroy or not)
            selectedTiles.Clear();
        }
    }

    private LetterTile GetTileUnderPointer()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out LetterTile tile))
                return tile;
        }

        return null;
    }
}