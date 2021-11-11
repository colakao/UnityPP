using UnityEngine;
using UnityEngine.UI;
using Timeline;

public class TimeScaleManager : MonoBehaviour
{
	public Slider slider;
	public AnimationManager anim;

	public void OnPause()
	{
		Time.timeScale = 0;
	}

	public void OnResume()
	{
		Time.timeScale = 1;
	}

	public void OnRestart()
    {
		slider.normalizedValue = 0;
		Time.timeScale = 1;
    }
}
