using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public List<Record> records;
    public TMP_Text Postext;
    public int rank;

    private void Update()
    {
        UpdateRanking();
        Postext.text = rank.ToString() + "/" + records.Count.ToString();
    }

    private void UpdateRanking()
    {
        records.Sort((a, b) => CompareRecords(a, b));
    }

    private int CompareRecords(Record a, Record b)
    {
        if (a.lap > b.lap)
        {
            return -1;
        }
        if (a.lap < b.lap)
        {
            return 1;
        }
        if (a.checkPointId > b.checkPointId)
        {
            return -1;
        }
        if (a.checkPointId < b.checkPointId)
        {
            return 1;
        }
        return 0;
    }
        
}
