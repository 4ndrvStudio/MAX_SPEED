namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;


    [RequireComponent(typeof(Selectable))]
    public class PointerUpdateMainMenuDescription : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!EventSystem.current.alreadySelecting)
            {
                MainMenuScreen.Instance.SetMenuDescription(gameObject.name);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this.GetComponent<Selectable>().OnPointerExit(null);
        }
    }
}