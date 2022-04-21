using System.Collections.Generic;
using System.IO;


public static class CSVParse
{
    public static List<List<string>> Parse(string text)
    {
        StringReader stringReader = new StringReader(text);
        TextReader reader = stringReader;

        List<List<string>> result = new List<List<string>>();       // ��� ����Ʈ

        bool isQuote = false;          // "Ȯ��
        bool isOneQuote = false;    // "�� 1���̶� ������ Ȯ��

        string line;

        List<string> tempLine = new List<string>(); // ���� ����
        string tempStr = "";        // ���� �ؽ�Ʈ

        while ((line = reader.ReadLine()) != null)
        {
            foreach(char ch in line)
            {
                switch (ch)
                {
                    case ',':
                        // " Ȯ��
                        if (isQuote)
                        {
                            tempStr += ch;
                        }
                        else
                        {
                            //���� �ؽ�Ʈ��
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
                        // " Ȯ��
                        if (isQuote)
                        {
                            tempStr += ch;      // �ؽ�Ʈ�� " �߰�
                            isQuote = false;
                        }
                        else
                        {
                            isOneQuote = true;
                            isQuote = true;
                        }
                        break;
                    default:
                        tempStr += ch;      // �ؽ�Ʈ�� ���� �߰�
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
                tempLine.Add(tempStr);  // ���ο� �ؽ�Ʈ �߰�
                result.Add(tempLine);       // ��ü ����Ʈ�� ���� �߰�
                tempLine = new List<string>();
                tempStr = "";
            }
            else
            {
                // �ٹٲ�
                tempStr += "\n";
            }
        }

        return result;
    }
}
