using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;


public class InitWithDefault : MonoBehaviour
{
  // Start is called before the first frame update
  private void Start()
  {
    UnityServices.InitializeAsync();
    AnalyticsService.Instance.StartDataCollection();
  }
}