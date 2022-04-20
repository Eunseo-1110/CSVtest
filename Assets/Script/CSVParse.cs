using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVParse
{
    public static List<List<string>> Parse(TextAsset text)
    {
        StringReader stringReader = new StringReader(text.text);
        TextReader reader = stringReader;

        List<List<string>> result = new List<List<string>>();       // 결과 리스트
        bool isQuote = false;          // "확인

        string line;        // 읽은 라인
        List<string> tempLine = new List<string>(); // 결과 리스트에 담길 리스트
        string tempStr = "";        // 읽은 텍스트
        while ((line = reader.ReadLine()) != null)
        {
            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                switch (ch)
                {
                    case ',':
                        tempLine.Add(tempStr);
                        tempStr = "";
                        break;
                    case '"':
                        break;
                    default:
                        tempStr += ch;
                        break;
                }
            }

            // 다음 라인
            result.Add(tempLine);
            tempLine = new List<string>();
        }

        return result;
    }
}
