using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataEyesService : LogInterface
{
    public string appId;
    public string channelId;
    public override void Init()
    {
        DCAgent.getInstance().initWithAppIdAndChannelId(appId, channelId);
        //DCAgent.setDebugMode(true);
    }

    public override void Log(string eventID, Dictionary<string, object> data)
    {
        Dictionary<string, string> report = new Dictionary<string, string>();

        if(data == null)
        {
            data = new Dictionary<string, object>();
        }

        foreach (var item in data)
        {
            report.Add(item.Key, item.Value.ToString());
        }

        DCEvent.onEventBegin(eventID, report);
        DCEvent.onEventEnd(eventID);
    }
}
