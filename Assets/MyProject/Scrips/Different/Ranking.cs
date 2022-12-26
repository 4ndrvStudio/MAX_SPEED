using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public List<Record> records;
    public TMP_Text Postext;
    public TMP_Text ReasultText;
    public int rank;
    Record car;
    private void Start()
    {
        car = records[0];
    }
    private void Update()
    {
        UpdateRanking();       
        Postext.text = rank.ToString() + "/" + records.Count.ToString();
        ReasultText.text = rank.ToString();
    }

    private void UpdateRanking()
    {
        records.Sort((a, b) => CompareRecords(a, b));
        for (int i = 0; i < records.Count; i++)
        {
            if (car == records[i])
                rank = i + 1;
        }
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
        if (a.dist > b.dist)
        {
            return 1;
        }
        if (a.dist < b.dist)
        {
            return -1;
        }
        return 0;
    }
    
    
}
