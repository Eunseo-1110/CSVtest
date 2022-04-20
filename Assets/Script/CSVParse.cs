using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVParse
{
    public static List<List<string>> Parse(TextAsset text)
    {
        StringReader stringReader = new StringReader(text.text);
        TextReader reader = stringReader;

        List<List<string>> result = new List<List<string>>();       // ��� ����Ʈ
        bool isQuote = false;          // "Ȯ��

        string line;        // ���� ����
        List<string> tempLine = new List<string>(); // ��� ����Ʈ�� ��� ����Ʈ
        string tempStr = "";        // ���� �ؽ�Ʈ
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

            // ���� ����
            result.Add(tempLine);
            tempLine = new List<string>();
        }

        return result;
    }
}
