using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Timeline
{
    public class AnimationManager : MonoBehaviour
    {
        [Header("Timeline Settings"), Space]
        [SerializeField] private int _timelineStartFrame;
		[SerializeField] private float _frameRate;

        [Header("Fecha inicio"), Tooltip("El primer día en terreno respecto al inicio de la linea de tiempo/animación."), Space]
        //Date
        public DateStruct startingDate;
        public DateTime currentTime;
        DateTime dateTime;

        //UI
        public Slider slider;

        [Header("Animators"), Space]
        //AnimationFrameObject
        public AnimatorStruct[] animatorObjects;
        private float _timelineLengthInFrames;
        private float _timelineEndFrame;
        private float _normalizedTimelineLengthInSeconds;

		//Timeline
		public TMP_Text textoFecha;
        public TMP_Text textoVelocidad;

        //speed
        [HideInInspector] public float speed;
        [Header("Toggle debug")]
        public bool debug;

        private IEnumerator coroutine;

		void Awake()
        {
            foreach (AnimatorStruct a in animatorObjects)
            {
                AnimatorClipInfo[] clipInfo = a.anim.GetCurrentAnimatorClipInfo(0);
                a.anim.speed = 0;
                float timelineEndFrame = Mathf.RoundToInt(clipInfo[0].clip.length * _frameRate + a.startFrame);
                if (debug)
                    Debug.Log("Timeline End Frame:" + timelineEndFrame);

                if (timelineEndFrame >= _timelineEndFrame)
                    _timelineEndFrame = timelineEndFrame;
            }
        }

		private void Start()
		{
            _timelineLengthInFrames = _timelineEndFrame - _timelineStartFrame;

            //Esto toma la duración desde el startFrame hasta el frame más alto de las animaciones como referencia de la duración del timeline.
            //Normalizado para manejar este valor directamente con el slider
            _normalizedTimelineLengthInSeconds = 1 / (_timelineLengthInFrames / 30);

            //año, mes, día
            dateTime = new DateTime(startingDate.year, startingDate.month, startingDate.day, new GregorianCalendar());
            speed = 1;
        }

		public void SetSpeed(float speed)
        {
            if (debug)
                Debug.Log("Button pressed!");
            this.speed = speed;
        }

        public void StartAnimation()
		{
            coroutine = UpdateAnimation();
            StartCoroutine(coroutine);
		}

        private IEnumerator UpdateAnimation()
		{
			while (true)
			{
                foreach (AnimatorStruct a in animatorObjects)
                {
                    if (a.anim == null) yield break;
                    AnimatorClipInfo[] clipInfo;
                    clipInfo = a.anim.GetCurrentAnimatorClipInfo(0);

                    float animOffset = (a.startFrame - _timelineStartFrame) / _timelineLengthInFrames;
                    float animMultiplier = _timelineLengthInFrames / Mathf.RoundToInt(clipInfo[0].clip.length * _frameRate);


                    a.anim.Play(clipInfo[0].clip.name, 0, Mathf.Clamp((slider.normalizedValue - animOffset) * animMultiplier, 0, 1));
                }

                //El slider avanza en base al tiempo, independiente de los frames.
                slider.normalizedValue += _normalizedTimelineLengthInSeconds * Time.deltaTime * speed;
                //Debug.Log(slider.normalizedValue);
                textoFecha.text = Timer.ValToAnimDate(dateTime, slider.normalizedValue, _timelineLengthInFrames / 30).ToString("dd MMM yyyy");
                if (speed > 1)
                    textoVelocidad.text = speed.ToString() + " días/segundo";
                else if (speed == 1)
                    textoVelocidad.text = speed.ToString() + " día/segundo";
                else
                    textoVelocidad.text = (speed * 24).ToString() + " horas/segundo";
                yield return null;
            }
        }
    }
}