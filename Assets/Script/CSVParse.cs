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
        bool isOneQuote = false;    // "�� 1���̶� ������ Ȯ��

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
                        if (isQuote)
                        {
                            tempStr += ch;
                        }
                        else
                        {
                            // ���� "����
                            if (isOneQuote)
                            {
                                tempStr = tempStr.Remove(tempStr.Length - 1);
                                isOneQuote = false;
                            }

                            tempLine.Add(tempStr);
                            tempStr = "";
                        }
                        break;
                    case '"':
                        if (isQuote)
                        {
                            tempStr += ch;
                            isQuote = false;
                        }
                        else
                        {
                            isOneQuote = true;
                            isQuote = true;
                        }
                        break;
                    default:
                        tempStr += ch;
                        break;
                }
            }

            if (!isQuote)
            {
                // ���� ����
                // ���� "����
                if (isOneQuote)
                {
                    tempStr = tempStr.Remove(tempStr.Length - 1);
                    isOneQuote = false;
                }
                tempLine.Add(tempStr);
                result.Add(tempLine);
                tempLine = new List<string>();
                tempStr = "";
            }
            else
            {
                tempStr += "\n";
            }
        }

        return result;
    }
}
