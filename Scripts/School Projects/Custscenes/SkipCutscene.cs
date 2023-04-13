using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    private TriggerCutscene _cutscene;
    private bool _skipable = false;

    private void Start()
    {
        _cutscene = GetComponent<TriggerCutscene>();
    }

    private void OnSkipCutscene()
    {
        if (!_skipable) return;
        _cutscene.Reset();
        _skipable = false;
    }

    public void SetCutscene(GameObject cutscene)
    {
        _cutscene = cutscene.GetComponent<TriggerCutscene>();
        _skipable = true;
    }
}
