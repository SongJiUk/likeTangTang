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
    /* TODO : Grid를 활용하여 Gem 최적화하기.

    Gem을 획득하는건 Player에서 관리함
    연산을 사용할때 루트를 씌우면 부하가 심하다.기존의 값을 제곱해서 sqrMagnitude를 사용하면 됨.

    Grid는 사용법이 되게 많다.

     */

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
        cell.obj.Remove(_go);
    }



    public List<GameObject> GetObjects(Vector3 _pos, float _range)
    {
        List<GameObject> objs = new List<GameObject>();

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
