using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindTheBirds
{
    public class ChangeAvatarListBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerAvatarApplierBehaviour _avatarList;
        [SerializeField] private Character _playerCharacter;

        public void ChangeAvatarList(PlayerAvatarListScriptable avatarList)
        {
            //Set existing avatar list to the new list
            _avatarList.avatarList = avatarList;

            //Apply new avatar list to player character 
            _playerCharacter.InterpretCharacterData(avatarList.GetPlayerCharacterData());
        }

}
}

