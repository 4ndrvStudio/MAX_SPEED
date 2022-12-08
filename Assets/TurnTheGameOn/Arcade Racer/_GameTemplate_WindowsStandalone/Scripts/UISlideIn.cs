namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    public class UISlideIn : MonoBehaviour
    {

        public enum SlideType { FromBottom, FromTop, FromLeft, FromRight }
        public SlideType slideType;
        RectTransform rectObject;
        Vector2 offscreenPosition;
        Vector2 targetPosition;
        Vector2 currentPosition;
        public AnimationCurve animCurve;
        float timer;
        public float activeFor = 0.2f;
        public bool useCustomX;
        public bool useCustomY;
        public Vector2 customOffset;
        bool isInitialized;
        float LinearDistance(float _start, float _end, float _position)
        {
            return Mathf.InverseLerp(_start, _end, _position);
        }

        void OnEnable()
        {
            Initialize();
            timer = 0f;
            currentPosition = offscreenPosition;
            rectObject.anchoredPosition = currentPosition;
        }

        private void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                rectObject = GetComponent<RectTransform>();
                currentPosition = rectObject.anchoredPosition;
                targetPosition = rectObject.anchoredPosition;
                offscreenPosition = rectObject.anchoredPosition;
                if (useCustomX) offscreenPosition.x = customOffset.x;
                    if (useCustomY) offscreenPosition.y = customOffset.y;
                if (slideType == SlideType.FromBottom)
                {
                    offscreenPosition.y = rectObject.anchoredPosition.y - (useCustomY ? offscreenPosition.y : Screen.height);

                }
                else if (slideType == SlideType.FromTop)
                {
                    offscreenPosition.y =rectObject.anchoredPosition.y + (useCustomY ? offscreenPosition.y : Screen.height);

                }
                else if (slideType == SlideType.FromLeft)
                {
                    offscreenPosition.x = rectObject.anchoredPosition.x - (useCustomX ? offscreenPosition.x :Screen.width);

                }
                else if (slideType == SlideType.FromRight)
                {
                    offscreenPosition.x = rectObject.anchoredPosition.x + (useCustomX ? offscreenPosition.x : Screen.width);
                }
            }
        }

        void OnDisable()
        {
            rectObject.anchoredPosition = targetPosition;
            timer = 0f;
        }


        void Update()
        {
            if (timer < activeFor)
            {
                timer += 1 * Time.unscaledDeltaTime;
                if (slideType == SlideType.FromBottom || slideType == SlideType.FromTop)
                {
                    currentPosition.y = Mathf.Lerp(offscreenPosition.y, targetPosition.y, animCurve.Evaluate(LinearDistance(0.0f, activeFor, timer)));
                }
                else if (slideType == SlideType.FromLeft || slideType == SlideType.FromRight)
                {
                    currentPosition.x = Mathf.Lerp(offscreenPosition.x, targetPosition.x, animCurve.Evaluate(LinearDistance(0.0f, activeFor, timer)));
                }
                rectObject.anchoredPosition = currentPosition;
            }
        }

    }
}