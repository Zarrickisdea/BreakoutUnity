using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private int colorIndex;
    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }
        set
        {
            colorIndex = value;
        }
    }

    private void Start()
    {
        SetSelfColor();
    }

    public void SetSelfColor()
    {
        Color color = BrickColors.GetBrickColor(colorIndex).Item1;
        spriteRenderer.color = color;
        textMeshProUGUI.text = (colorIndex + 1).ToString();
    }
}
