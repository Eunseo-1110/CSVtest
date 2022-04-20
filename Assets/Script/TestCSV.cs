using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    public TextAsset text;
    public List<List<string>> tempList;

    void Start()
    {
        tempList = CSVParse.Parse(text);

    }

    void Update()
    {
        
    }
}
