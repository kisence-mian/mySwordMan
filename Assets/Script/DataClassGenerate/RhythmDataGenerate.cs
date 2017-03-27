using System;
using UnityEngine;

//RhythmDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class RhythmDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_Content; //韵律内容

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("RhythmData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("RhythmDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_Content = data.GetString("Content");
	}
}
