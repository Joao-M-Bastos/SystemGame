using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public const int HEAT_MAX_VALUE = 100;
    public const int HEAT_MIN_VALUE = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }


    int width;
    int hight;
    int cellSize;
    Vector3 originPos;

    public int Width => width;
    public int Hight => hight;
    public int CellSize => cellSize;

    private int[,] gridArray;
    private TextMesh[,] textMeshes;

    public Grid(int _width, int _higth, int _cellSize, Vector3 _originPos)
    {
        width = _width;
        hight = _higth;
        cellSize = _cellSize;
        originPos = _originPos;

        gridArray = new int[width, hight];
        textMeshes = new TextMesh[width, hight];

        DebugGrid();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + originPos;
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPos).y / cellSize);
    }

    public void SetValue(Vector3 worldPostion, int setValue)
    {
        int x, y;
        GetXZ(worldPostion, out x, out y);
        SetValue(x, y, setValue);
    }

    public void SetValue(int x, int y, int setValue)
    {
        if (x >= 0 && y >= 0 && x < width && y < hight)
        {
            gridArray[x, y] = Mathf.Clamp( setValue, HEAT_MIN_VALUE, HEAT_MAX_VALUE);

            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x ,y=y });
        }
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < hight)
            return gridArray[x, y];
        return 0;
    }

    public int GetValue(Vector3 worldPostion)
    {
        int x, y;
        GetXZ(worldPostion, out x, out y);
        return GetValue(x, y);
    }

    private void DebugGrid()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < gridArray.GetLength(1); j++)
            {
                textMeshes[i,j] = CreateWorldText(gridArray[i,j].ToString(), GetWorldPosition(i,j) + new Vector3(cellSize, cellSize,0) * .5f ,20,Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i,j+1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, hight), GetWorldPosition(width, hight), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, hight), Color.white, 100f);

        OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
        {
            textMeshes[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        };
    }

    public TextMesh CreateWorldText(string text, Vector3 localPosition, int fontSize, Color color , TextAnchor textAncor)
    {
        if(color == null) color = Color.white;

        GameObject go = new GameObject("WorldText", typeof(TextMesh));

        Transform transform = go.transform;
        transform.SetParent(transform.parent, false);
        transform.localPosition = localPosition;
        transform.Rotate(0, 0, 0);

        TextMesh tm = go.GetComponent<TextMesh>();
        tm.anchor = textAncor;
        tm.fontSize = fontSize;
        tm.color = color;

        return tm;
    }
}
