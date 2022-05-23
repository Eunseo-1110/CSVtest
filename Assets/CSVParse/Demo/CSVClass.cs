using System.Collections.Generic;

public class CSVClass
{
	public class Data
	{
		public int inx;
		public int[] numbers;
		public float speed;
		public string testSctipt;

	}

	public List<Data> DataList { get; private set; }

	public void Load(string csv)
	{
		DataList = new List<Data>();
		List<List<string>> csvList = CSVParse.Parse(csv);
		string[] tempArr = null;

		for (int i = 1; i < csvList.Count; i++)
		{
			List<string> tempList = csvList[i];
			Data data = new Data();
            
			data.inx = int.Parse(tempList[0]);
			tempArr = tempList[1].Split(',');
			data.numbers = new int[tempArr.Length];
			for (int j = 0; j < tempArr.Length; j++)
			{
				data.numbers[j] = int.Parse(tempArr[j]);
			}
			data.speed = float.Parse(tempList[2]);
			data.testSctipt = tempList[3];

			DataList.Add(data);
		}
	}
}