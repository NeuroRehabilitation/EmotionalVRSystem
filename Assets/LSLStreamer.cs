using LSL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LSLStreamer : MonoBehaviour
{
    public string StreamName;
    public string StreamType;
    public string StreamID;

    public List<string> Channels;


    private StreamOutlet Outlet;
    private bool Stream_On= true;

    private void Awake()
    {
        DontDestroyOnLoad(transform.parent);
    }

    public void StartStream()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, Channels.Count, 50, LSL.channel_format_t.cf_float32);
        XMLElement chans = streamInfo.desc().append_child("channels");
        foreach (string channel in Channels)
        {
            chans.append_child("channel").append_child_value("label", channel);
        }
        Outlet = new StreamOutlet(streamInfo);
        Stream_On=true;
    }
    public void StreamData(float[] sample)
    {
        if (Stream_On)
        {
            Outlet.push_sample(sample);
            //Debug.Log("Data Sent->"+sample);
        }
        else
        {
            //Debug.Log("Stream hasn't been started");
        }
    }

    public void StopStream()
    {
        Outlet.Close();
        Stream_On = false;
    }

}
