using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Cell
{
    public HashSet<GameObject> obj = new HashSet<GameObject>();
}

public class GridController : BaseController
{
    Grid grid;
    Dictionary<Vector3Int, Cell> cells = new Dictionary<Vector3Int, Cell>();

    public override bool Init()
    {
        base.Init();
        grid = gameObject.GetOrAddComponent<Grid>();

        return true;
    }

    public Cell GetCell(Vector3Int _pos)
    {
        Cell cell = null;
        if(cells.TryGetValue(_pos, out cell) == false)
        {
            cell = new Cell();
            cells.Add(_pos, cell);
        }

        return cell;
    }

    public void AddCell(GameObject _go)
    {
        Vector3Int pos = grid.WorldToCell(_go.transform.position);
        Cell cell = GetCell(pos);

        if(cell == null) return;

        cell.obj.Add(_go);
    }

    public void RemoveCell(GameObject _go)
    {
        Vector3Int pos = grid.WorldToCell(_go.transform.position);
        Cell cell = GetCell(pos);

        if(cell == null) return;
        if (cell.obj.Count == 0)
            cells.Remove(pos);

            cell.obj.Remove(_go);
    }



    public List<DropItemController> GetObjects(Vector3 _pos, float _range)
    {
        List<DropItemController> objs = new List<DropItemController>();

        Vector3Int left = grid.WorldToCell(_pos + new Vector3(-_range,0));
        Vector3Int right = grid.WorldToCell(_pos + new Vector3(_range,0));
        Vector3Int top = grid.WorldToCell(_pos + new Vector3(0,_range));
        Vector3Int bottom = grid.WorldToCell(_pos + new Vector3(0,-_range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;


        for(int i = minX; i<=maxX; i++)
        {
            for(int j = minY; j <= maxY; j++)
            {
                if(cells.ContainsKey(new Vector3Int(i,j,0)) == false) continue;

                objs.AddRange(cells[new Vector3Int(i,j,0)].obj);
            }
        }

        return objs;
    }

}
