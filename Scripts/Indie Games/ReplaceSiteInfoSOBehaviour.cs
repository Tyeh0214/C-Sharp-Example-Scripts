using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindTheBirds
{
    public class ReplaceSiteInfoSOBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneField _newTargetScene;
        [SerializeField] private SiteInfoScriptableObject[] _siteInfoScriptableObjects;

        [ContextMenu("Debug/Set Target Scene")]
        public void SetTargetScene()
        {
            //For each target scene in the array of site Info SOs, replace with the new desired target scene
            foreach (SiteInfoScriptableObject siteInfoSO in _siteInfoScriptableObjects)
            {
                siteInfoSO.targetScene = _newTargetScene;
                Debug.LogWarning("Target Scene Set " + siteInfoSO.GetTargetScene.SceneName);
            }
        }
    }
}

