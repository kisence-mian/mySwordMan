using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExainePoemItem : ReusingScrollItemBase
{
    public override void SetContent(int index, Dictionary<string, object> data)
    {
        string content = (string)data["content"];

        if (content.Contains("space"))
        {
            SetText("Text", "");
        }
        else
        {
            SetText("Text", content);
        }
    }
}
