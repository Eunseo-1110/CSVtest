using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSVTemplate
{
    // Ŭ���� ��ü �ڵ�
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

    // �迭 �������/ 0 = ����Ʈ �ε���, 1 = �����̸�, 2 = �ڷ��� 
    const string arrMemberTemplate =
@"tempArr = tempList[{0}].Split(',');
            data.{1} = new {2}[tempArr.Length];
            for (int j = 0; j < tempArr.Length; j++)
            {{
                data.{1}[j] = {2}.Parse(tempArr[j]);
            }}
            ";

    // ����/ 0 = �ڷ���, 1 = �̸�
    const string memberTemplate = "public {0} {1};\n\t\t";

    // ���� Ÿ��
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
        string code = classTemplate;    // ��� �ڵ�

        List<List<string>> csvList = CSVParse.Parse(csv);   // csv����Ʈ
        List<string> memberNameList = csvList[0];       // ������� �̸� ����Ʈ

        string members = "";    // �������
        string lodeMember = ""; // ������ csv �о����

        // ������� �� ��ŭ �ݺ�
        for (int i = 0; i < memberNameList.Count; i++)
        {
            string type = "";

            ETypeState typeState = ETypeState.None; //���� Ÿ��

            // �����͸�ŭ �ݺ�
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
                    // �迭 Ȯ��
                    // ","�� ����
                    string[] str = tempList[i].Split(',');
                    bool isFloat = true;
                    bool isNum = true;

                    // ","�� ���� �迭 �ݺ�
                    foreach (string item in str)
                    {
                        // int Ȯ��
                        if (!int.TryParse(item, out int temp))
                        {
                            isNum = false;
                            // float Ȯ��
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

            // Ÿ�Կ� ���� �������ڿ�, csv�б⹮�ڿ� �߰�
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

            string tempMember = String.Format(memberTemplate, type, memberNameList[i]); // ���� ����
            members += tempMember;
        }

        // �ڵ忡 �߰�
        code = code.Replace("&&CLASSNAME&&", className);
        code = code.Replace("&&MEMBERS&&", members);
        code = code.Replace("&&LOADDATA&&", lodeMember);

        return code;
    }
}
