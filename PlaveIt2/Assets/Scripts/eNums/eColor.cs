[System.Serializable]
public enum eColor
{
    None,
    Blue,
    Yellow,
    Purple,
    Orange,
    White
}

static class eColorMethods
{
    public static bool IsEqual(this eColor color1, eColor color2)
    {
        return color1 == color2 || color2 == eColor.White || color1 == eColor.White;
    }
}
