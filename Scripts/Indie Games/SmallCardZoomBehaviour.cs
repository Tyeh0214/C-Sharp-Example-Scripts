using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FindTheBirds
{
    public class SmallCardZoomBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _fullsizeImageRenderer;
        [SerializeField] private SwitchBehaviour _fullSizeImageDisplaySwitch;

        private BirdData _birdData;
        private ExtendedCardDataApplierBehaviour _extendedCardDataApplierBehaviour;

        public UnityEvent _showExpandedCardEvent;

        private void Awake()
        {
            //On awake, grab the Extended Card Data Applier component and store it in a variable
            _extendedCardDataApplierBehaviour = GetComponent<ExtendedCardDataApplierBehaviour>();
        }

        public void SetBirdData(BirdData birdData)
        {
            //Sets the birdData scriptable object to the new desired one
            _birdData = birdData;
            ShowCard();
        }

        private void ShowCard()
        {
            //If the birdData SO does have an extended card SO, apply and show it
            if (_birdData.GetExtendedCardData != null)
            {
                _extendedCardDataApplierBehaviour.ApplyAndShow(_birdData);
            }
            else
            {
                //Else, run function below
                SmallCardOverride();
            }
        }

        public void SmallCardOverride()
        {
            //Sets the sprite renderer image to the BirdCardImage
            _fullsizeImageRenderer.sprite = _birdData.GetBirdCardImage;
            //Display the image on screen
            _fullSizeImageDisplaySwitch.SwitchOn();
        }
    }
}

