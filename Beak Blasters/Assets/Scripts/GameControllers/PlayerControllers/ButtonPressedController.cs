using UnityEngine;
using UnityEngine.EventSystems;

namespace GameControllers.PlayerControllers
{
    public class ButtonPressedController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private BoostHandler _boostHandler;

        public void OnPointerDown(PointerEventData eventData)
        {
            _boostHandler.UseBoost();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _boostHandler.StopUseBoost();
        }
    }
}

