using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteItem : ReusingScrollItemBase 
{

    public override void SetContent(int index, Dictionary<string, object> data)
    {
        SetText("Text_poemName", (string)data["poemName"]);
        SetText("Text_author", (string)data["author"]);
    }
}
