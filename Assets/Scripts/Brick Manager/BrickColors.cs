using UnityEngine;

public static class BrickColors
{
    public static (Color, int) GetBrickColor(int colorIndex)
    {
        Color color;
        switch (colorIndex)
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.blue;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 4:
                color = Color.magenta;
                break;
            case 5:
                color = Color.cyan;
                break;
            case 6:
                color = Color.gray;
                break;
            case 7:
                color = Color.white;
                break;
            default:
                color = Color.black;
                break;
        }

        return (color, colorIndex);
    }
}
