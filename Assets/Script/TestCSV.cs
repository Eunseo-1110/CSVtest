using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestList
{
    public List<string> Test;
}

public class TestCSV : MonoBehaviour
{
    public TextAsset text;
    public List<TestList> tempShowList;

    void Start()
    {
        List<List<string>> temp= CSVParse.Parse(text.text);
        foreach (List<string> item in temp)
        {
            TestList testList = new TestList();
            testList.Test = item;
            tempShowList.Add(testList);
        }
    }

    void Update()
    {
        
    }
}
