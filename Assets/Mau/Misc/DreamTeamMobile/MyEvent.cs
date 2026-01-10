using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class MyEvent : Unity.Services.Analytics.Event
{
  public MyEvent() : base("myEvent")
  {
  }

  public string FabulousString { set { SetParameter("fabulousString", value); } }
  public int SparklingInt { set { SetParameter("sparklingInt", value); } }
  public float SpectacularFloat { set { SetParameter("spectacularFloat", value); } }
  public bool PeculiarBool { set { SetParameter("peculiarBool", value); } }

}
