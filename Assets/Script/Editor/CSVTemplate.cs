using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSVTemplate
{
    // 클래스 전체 코드
    const string classTemplate =
@"using System.Collections.Generic;

public class &&CLASSNAME&&
{
    public class Data
    {
        &&MEMBERS&&
    }

    public List<Data> DataList { get; private set; }

    public void Load(string csv)
    {
        DataList = new List<Data>();
        List<List<string>> csvList = CSVParse.Parse(csv);
        string[] tempArr = null;

        for (int i = 1; i < csvList.Count; i++)
        {
            Data data = new Data();

            List<string> tempList = csvList[i];
            
            &&LOADDATA&&

            DataList.Add(data);
        }
    }
}
";

    // 배열 멤버변수/ 0 = 리스트 인덱스, 1 = 변수이름, 2 = 자료형 
    const string arrMemberTemplate =
@"tempArr = tempList[{0}].Split(',');
            data.{1} = new {2}[tempArr.Length];
            for (int j = 0; j < tempArr.Length; j++)
            {{
                data.{1}[j] = {2}.Parse(tempArr[j]);
            }}
            ";

    // 변수/ 0 = 자료형, 1 = 이름
    const string memberTemplate = "public {0} {1};\n\t\t";

    // 변수 타입
    // int, float, int[], float[], string
    enum ETypeState
    {
        None,
        IntState,
        FloatState,
        IntArrState,
        FloatArrState,
        StringState,
    }

    public static string Generate(string csv, string className)
    {
        string code = classTemplate;    // 결과 코드

        List<List<string>> csvList = CSVParse.Parse(csv);   // csv리스트
        List<string> memberNameList = csvList[0];       // 멤버변수 이름 리스트

        string members = "";    // 멤버변수
        string lodeMember = ""; // 변수에 csv 읽어오기

        // 멤버변수 수 만큼 반복
        for (int i = 0; i < memberNameList.Count; i++)
        {
            string type = "";

            ETypeState typeState = ETypeState.None; //변수 타입

            // 데이터만큼 반복
            for (int j = 1; j < csvList.Count; j++)
            {
                List<string> tempList = csvList[j];

                if (int.TryParse(tempList[i], out int intres))
                {
                    if(typeState != ETypeState.StringState
                        && typeState != ETypeState.FloatState
                        && typeState != ETypeState.IntArrState
                        && typeState != ETypeState.FloatArrState)
                        typeState = ETypeState.IntState;
                }
                else if (tempList[i].Contains(","))
                {
                    // 배열 확인
                    // ","로 나눔
                    string[] str = tempList[i].Split(',');
                    bool isFloat = true;
                    bool isNum = true;

                    // ","로 나눈 배열 반복
                    foreach (string item in str)
                    {
                        // int 확인
                        if (!int.TryParse(item, out int temp))
                        {
                            isNum = false;
                            // float 확인
                            if (!float.TryParse(item, out float floatres))
                            {
                                isFloat = false;
                            }
                        }
                    }

                    if (isNum)
                    {
                        if(typeState != ETypeState.FloatArrState)
                            typeState = ETypeState.IntArrState;
                    }
                    else if (isFloat)
                    {
                        typeState = ETypeState.FloatArrState;
                    }
                }
                else if (float.TryParse(tempList[i], out float floatres))
                {
                    // float
                    if (typeState != ETypeState.FloatArrState
                        && typeState != ETypeState.IntArrState)
                        typeState = ETypeState.FloatState;
                }
                else
                {
                    // string
                    typeState = ETypeState.StringState;
                }
            }

            // 타입에 따라 변수문자열, csv읽기문자열 추가
            switch (typeState)
            {
                case ETypeState.IntState:
                    type = "int";
                    lodeMember += String.Format("data.{0} = int.Parse(tempList[{1}]);\n\t\t\t", memberNameList[i], i);
                    break;
                case ETypeState.FloatState:
                    type = "float";
                    lodeMember += String.Format("data.{0} = float.Parse(tempList[{2}]);\n\t\t\t", memberNameList[i], "(float)", i);
                    break;
                case ETypeState.IntArrState:
                    type = "int[]";
                    lodeMember += String.Format(arrMemberTemplate, i, memberNameList[i], "int");
                    break;
                case ETypeState.FloatArrState:
                    type = "float[]";
                    lodeMember += String.Format(arrMemberTemplate, i, memberNameList[i], "float");
                    break;
                case ETypeState.StringState:
                    type = "string";
                    lodeMember += String.Format("data.{0} = tempList[{2}];\n\t\t\t", memberNameList[i], "(float)", i);
                    break;
            }

            string tempMember = String.Format(memberTemplate, type, memberNameList[i]); // 변수 문자
            members += tempMember;
        }

        // 코드에 추가
        code = code.Replace("&&CLASSNAME&&", className);
        code = code.Replace("&&MEMBERS&&", members);
        code = code.Replace("&&LOADDATA&&", lodeMember);

        return code;
    }
}
