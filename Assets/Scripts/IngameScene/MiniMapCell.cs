using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MiniMapCell : MonoBehaviour
{
    private Image img;

    [SerializeField] private Color wallColor;
    [SerializeField] private Color groundColor;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void DrawCell(CellType c)
    {
        switch(c)
        {
        case CellType.Goal:
            img.color = groundColor;
            break;
        case CellType.Ground:
            img.color = groundColor;
            break;
        case CellType.Wall:
            img.color = wallColor;
            break;
        }
    }

    public void ClearCell()
    {
        img.color = Color.black;
    }
}
