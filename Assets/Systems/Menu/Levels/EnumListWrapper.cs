using System.Collections.Generic;

[System.Serializable]
public class EnumListWrapper
{
    public List<ELevelStates> Levels;

    public EnumListWrapper(List<ELevelStates> levels)
    {
        Levels = levels;
    }
}