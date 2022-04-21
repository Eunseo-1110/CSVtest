using System.Collections.Generic;
using System.IO;


public static class CSVParse
{
    public static List<List<string>> Parse(string text)
    {
        StringReader stringReader = new StringReader(text);
        TextReader reader = stringReader;

        List<List<string>> result = new List<List<string>>();       // 결과 리스트

        bool isQuote = false;          // "확인
        bool isOneQuote = false;    // "를 1번이라도 만나면 확인

        string line;

        List<string> tempLine = new List<string>(); // 읽은 라인
        string tempStr = "";        // 읽은 텍스트

        while ((line = reader.ReadLine()) != null)
        {
            foreach(char ch in line)
            {
                switch (ch)
                {
                    case ',':
                        // " 확인
                        if (isQuote)
                        {
                            tempStr += ch;
                        }
                        else
                        {
                            //다음 텍스트로
                            // 앞의 "삭제
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
                        // " 확인
                        if (isQuote)
                        {
                            tempStr += ch;      // 텍스트에 " 추가
                            isQuote = false;
                        }
                        else
                        {
                            isOneQuote = true;
                            isQuote = true;
                        }
                        break;
                    default:
                        tempStr += ch;      // 텍스트에 문자 추가
                        break;
                }
            }

            if (!isQuote)
            {
                // 다음 라인
                // 앞의 "삭제
                if (isOneQuote)
                {
                    tempStr = tempStr.Remove(tempStr.Length - 1);
                    isOneQuote = false;
                }
                tempLine.Add(tempStr);  // 라인에 텍스트 추가
                result.Add(tempLine);       // 전체 리스트에 라인 추가
                tempLine = new List<string>();
                tempStr = "";
            }
            else
            {
                // 줄바꿈
                tempStr += "\n";
            }
        }

        return result;
    }
}
