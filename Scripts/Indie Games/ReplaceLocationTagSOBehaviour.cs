using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindTheBirds
{
    public class ReplaceLocationTagSOBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneField _newTargetScene;
        [SerializeField] private LocationTagSO _locationTagScriptableObject;

        [ContextMenu("Debug/Set Target Scene")]
        public void SetTargetScene()
        {
            //Sets the target scene in the Location Tag scriptable object to the new desired scene
            _locationTagScriptableObject.locationScene = _newTargetScene;
        }
    }
}
