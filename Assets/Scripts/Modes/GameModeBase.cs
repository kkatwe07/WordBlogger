using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeBase : MonoBehaviour
{
    public abstract void Init();
    public abstract void OnWordSubmitted(string word, List<LetterTile> tilesUsed);
}