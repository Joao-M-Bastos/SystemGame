using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    Grid grid;
    [SerializeField] HeatmapVisual heatMap;
    [SerializeField] int width, height ,cellSize;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(width, height, cellSize, new Vector3(-(width*cellSize) / 2, -(height * cellSize) / 2, 0));

        heatMap.SetGrid(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;

            int gridValue = grid.GetValue(position);

            grid.SetValue(position, gridValue + 4);

        }
    }


}
