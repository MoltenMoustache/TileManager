using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDebugPanel_TileManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] PanelButton randomTiles;
    [SerializeField] PanelButton getTile;
    [SerializeField] PanelButton getRow;
    [SerializeField] PanelButton getColumn;
    [SerializeField] PanelButton getLine;
    [SerializeField] PanelButton getCross;

    [SerializeField] TileManager tileManager;
    public void GetRow()
    {
        int y = int.Parse(getRow.inputFields[0].text);
        GameObject[] row = tileManager.GetRow(y);
        for (int i = 0; i < row.Length; i++)
        {
            row[i].SetActive(false);
        }

        StartCoroutine(EnableAfterSeconds(row, 2.5f));
    }
    public void GetColumn()
    {
        int x = int.Parse(getColumn.inputFields[0].text);
        GameObject[] column = tileManager.GetColumn(x);
        for (int i = 0; i < column.Length; i++)
        {
            column[i].SetActive(false);
        }

        StartCoroutine(EnableAfterSeconds(column, 2.5f));
    }

    public void GetTile()
    {
        int x = int.Parse(getTile.inputFields[0].text);
        int y = int.Parse(getTile.inputFields[1].text);

        GameObject tile = tileManager.GetTile(new IntVector2(x, y));
        tile.SetActive(false);

        StartCoroutine(EnableAfterSeconds(tile, 2.5f));
    }

    public void GetCross()
    {
        int x = int.Parse(getCross.inputFields[0].text);
        int y = int.Parse(getCross.inputFields[1].text);

        int length = int.Parse(getCross.inputFields[2].text);

        GameObject[] cross = tileManager.GetCrossFromPoint(new IntVector2(x, y), length);
        for (int i = 0; i < cross.Length; i++)
        {
            Debug.Log(i);
            cross[i].SetActive(false);
        }

        StartCoroutine(EnableAfterSeconds(cross, 2.5f));
    }

    public void GetRandomTile()
    {
        GameObject tile = tileManager.GetRandomTile();
        tile.SetActive(false);

        StartCoroutine(EnableAfterSeconds(tile, 2.5f));
    }

    public void GetRandomTiles()
    {
        int count = int.Parse(randomTiles.inputFields[0].text);

        GameObject[] tiles = tileManager.GetRandomTiles(count);
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetActive(false);
        }

        StartCoroutine(EnableAfterSeconds(tiles, 2.5f));
    }

    public void GetLine()
    {
        int tileCount = int.Parse(getLine.inputFields[3].text);
        int direction = int.Parse(getLine.inputFields[2].text);

        IntVector2 origin = new IntVector2(int.Parse(getLine.inputFields[0].text), int.Parse(getLine.inputFields[1].text));

        GameObject[] line = tileManager.GetLineFromPoint(origin, direction, tileCount);
        for (int i = 0; i < line.Length; i++)
        {
            line[i].SetActive(false);
        }

        StartCoroutine(EnableAfterSeconds(line, 2.5f));
    }

    IEnumerator EnableAfterSeconds(GameObject a_object, float a_seconds)
    {
        yield return new WaitForSeconds(a_seconds);
        a_object.SetActive(true);
    }

    IEnumerator EnableAfterSeconds(GameObject[] a_objects, float a_seconds)
    {
        yield return new WaitForSeconds(a_seconds);
        for (int i = 0; i < a_objects.Length; i++)
        {
            a_objects[i].SetActive(true);
        }
    }
}

[System.Serializable]
public class PanelButton
{
    public TMP_InputField[] inputFields;
}
