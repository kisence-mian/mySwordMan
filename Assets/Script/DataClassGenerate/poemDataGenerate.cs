using System;
using UnityEngine;

//poemDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class poemDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_poemName; //诗名
	public string m_author; //作者
	public string m_description; //介绍
	public string[] m_content; //内容

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("poemData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("poemDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_poemName = data.GetString("poemName");
		m_author = data.GetString("author");
		m_description = data.GetString("description");
		m_content = data.GetStringArray("content");
	}
}
