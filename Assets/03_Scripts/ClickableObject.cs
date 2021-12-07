using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Timeline;
 
public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public GameObject previousButton, nextButton;
    public float previousSpeed;
    public AnimationManager animManager;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
		{
            previousButton.SetActive(true);
            nextButton.SetActive(false);
            gameObject.SetActive(false);
            animManager.speed = previousSpeed;
        }
    }
}
