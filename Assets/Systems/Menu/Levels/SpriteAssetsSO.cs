using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSprites", menuName = "Game/Level Sprites")]
public class LevelSpritesSO : ScriptableObject
{
    public List<Sprite> sprites;
}
