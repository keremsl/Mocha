using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BhorGames.Mocha
{
    public class Mocha : MonoBehaviour
    {
        public enum ANIM_STYLE
        {
            MOVE,
            SCALE,
            ROTATE,
            FADE
        }

        public enum ANIM_TYPE
        {
            ONE_TIME,
            LOOP
        }

        public bool isPlaying; // is animaton playing?
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // Animation Curve That Determines How To Move UI Element(Ease In,Ease Out etc..)
        public ANIM_STYLE animationType; // Animaton style Move,Scale,Rotate,Fade
        public ANIM_TYPE animationT;
        public bool playOnAwake = true;

        /* MOVE VARIABLES */
        public Vector2 move_finalDestination;
        public float move_duration;
        public bool move_EndOfAnimation;
        public Vector2 first_position;
        public UnityEvent move_endEvent; // Functions that invoke after move animation finished


        /* SCALE VARIABLES */
        public Vector3 scale_finalScale;
        public float scale_duration;
        public bool scale_EndOfAnimation;
        public Vector3 first_scale;
        public UnityEvent scale_endEvent; // Functions that invoke after scale animation finished


        /* ROTATE VARIABLES */
        public Vector3 rotate_finalRotation;
        public float rotate_duration;
        public bool rotate_EndOfAnimation;
        public Vector3 first_rotation;
        public UnityEvent rotate_endEvent; // Functions that invoke after rotate animation finished
                                           /* ------------- */

        /* FADE VARIABLES */
        public float fade_finalAlpha;
        public float fade_duration;
        public bool fade_endOfAnimation;
        public float first_alpha;
        public UnityEvent fade_endEvent; // Functions that invoke after rotate animation finished
                                         /* ------------- */
        Image selfImage;
        private void Awake()
        {
            isPlaying = false;
            first_position = GetComponent<RectTransform>().anchoredPosition;
            first_scale = GetComponent<RectTransform>().localScale;
            first_rotation = GetComponent<RectTransform>().eulerAngles;
            first_alpha = GetComponent<Image>() != null ? GetComponent<Image>().color.a : float.NaN;
            if (first_alpha == float.NaN)
            {
                Debug.LogError("Fade can only available for game objects that have image component");
            }
        }

        private void Start()
        {
            if (playOnAwake)
            {
                Play();
            }
            selfImage = transform.GetComponent<Image>() != null ? transform.GetComponent<Image>() : null; // Control for Image Component
        }

        public IEnumerator Move(RectTransform self, float duration) // Move State
        {
            isPlaying = true;
            float localDuration = 0;
            switch (animationT)
            {
                case ANIM_TYPE.ONE_TIME:
                    localDuration = 0f;
                    while (!Approximation(self.anchoredPosition, move_finalDestination, 0.01f))
                    {
                        localDuration += Time.fixedDeltaTime / 2;
                        self.anchoredPosition = new Vector2(Mathf.SmoothStep(self.anchoredPosition.x, move_finalDestination.x, curve.Evaluate(localDuration / duration)),
                        Mathf.SmoothStep(self.anchoredPosition.y, move_finalDestination.y, curve.Evaluate(localDuration / duration)));
                        yield return null;
                    }
                    self.anchoredPosition = move_finalDestination;

                    if (move_EndOfAnimation)
                        move_endEvent.Invoke();
                    break;
                case ANIM_TYPE.LOOP:
                    while (true)
                    {
                        localDuration = 0f;
                        while (!Approximation(self.anchoredPosition, move_finalDestination, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.anchoredPosition = new Vector2(Mathf.SmoothStep(self.anchoredPosition.x, move_finalDestination.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.anchoredPosition.y, move_finalDestination.y, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.anchoredPosition = move_finalDestination;
                        localDuration = 0f;
                        while (!Approximation(self.anchoredPosition, first_position, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.anchoredPosition = new Vector2(Mathf.SmoothStep(self.anchoredPosition.x, first_position.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.anchoredPosition.y, first_position.y, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.anchoredPosition = first_position;

                        if (move_EndOfAnimation)
                            move_endEvent.Invoke();
                    }
            }


            isPlaying = false;
            yield return null;
        }
        public void MoveUI(RectTransform self, float duration)
        {
            StartCoroutine(Move(self, duration));
        }
        public IEnumerator Scale(RectTransform self, float duration) // Scale State
        {
            isPlaying = true;
            float localDuration = 0;
            switch (animationT)
            {
                case ANIM_TYPE.ONE_TIME:
                    while (!Approximation(self.localScale, scale_finalScale, 0.01f))
                    {
                        localDuration += Time.fixedDeltaTime / 2;
                        self.localScale = new Vector3(Mathf.SmoothStep(self.localScale.x, scale_finalScale.x, curve.Evaluate(localDuration / duration)),
                        Mathf.SmoothStep(self.localScale.y, scale_finalScale.y, curve.Evaluate(localDuration / duration)),
                        Mathf.SmoothStep(self.localScale.z, scale_finalScale.z, curve.Evaluate(localDuration / duration)));
                        yield return null;
                    }
                    self.localScale = scale_finalScale;
                    break;
                case ANIM_TYPE.LOOP:
                    while (true)
                    {
                        localDuration = 0f;
                        while (!Approximation(self.localScale, scale_finalScale, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.localScale = new Vector3(Mathf.SmoothStep(self.localScale.x, scale_finalScale.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.localScale.y, scale_finalScale.y, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.localScale.z, scale_finalScale.z, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.localScale = scale_finalScale;
                        localDuration = 0f;
                        while (!Approximation(self.localScale, first_scale, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.localScale = new Vector3(Mathf.SmoothStep(self.localScale.x, first_scale.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.localScale.y, first_scale.y, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.localScale.z, first_scale.z, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.localScale = first_scale;
                    }

            }

            if (scale_EndOfAnimation)
                scale_endEvent.Invoke();

            isPlaying = false;
            yield return null;
        }

        public void FadeUI(RectTransform self, float duration)
        {
            StartCoroutine(Fade(self, duration));
        }

        public void ScaleUI(RectTransform self, float duration)
        {
            StartCoroutine(Scale(self, duration));
        }

        public IEnumerator Rotate(RectTransform self, float duration)
        {
            isPlaying = true;
            float localDuration = 0;
            switch (animationT)
            {
                case ANIM_TYPE.ONE_TIME:
                    while (!Approximation(self.eulerAngles, rotate_finalRotation, 0.01f))
                    {
                        localDuration += Time.fixedDeltaTime / 2;
                        self.eulerAngles = new Vector3(Mathf.SmoothStep(self.eulerAngles.x, rotate_finalRotation.x, curve.Evaluate(localDuration / duration)),
                        Mathf.SmoothStep(self.eulerAngles.y, rotate_finalRotation.y, curve.Evaluate(localDuration / duration)),
                        Mathf.SmoothStep(self.eulerAngles.z, rotate_finalRotation.z, curve.Evaluate(localDuration / duration)));
                        yield return null;
                    }
                    self.eulerAngles = rotate_finalRotation;
                    break;
                case ANIM_TYPE.LOOP:
                    while (true)
                    {
                        localDuration = 0f;
                        while (!Approximation(self.eulerAngles, rotate_finalRotation, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.eulerAngles = new Vector3(Mathf.SmoothStep(self.eulerAngles.x, rotate_finalRotation.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.eulerAngles.y, rotate_finalRotation.y, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.eulerAngles.z, rotate_finalRotation.z, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.eulerAngles = rotate_finalRotation;
                        localDuration = 0f;
                        while (!Approximation(self.eulerAngles, first_rotation, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            self.eulerAngles = new Vector3(Mathf.SmoothStep(self.eulerAngles.x, first_rotation.x, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.eulerAngles.y, first_rotation.y, curve.Evaluate(localDuration / duration)),
                            Mathf.SmoothStep(self.eulerAngles.z, first_rotation.z, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        self.eulerAngles = first_rotation;
                    }

            }

            if (rotate_EndOfAnimation)
                rotate_endEvent.Invoke();

            isPlaying = false;
            yield return null;
        }
        public IEnumerator Fade(RectTransform self, float duration)
        {
            isPlaying = true;
            float localDuration = 0;
            if (selfImage == null)
            {
                Debug.LogError("Fade can only available for game objects that have image component");
                yield break;
            } // Custom Error Handling

            switch (animationT)
            {
                case ANIM_TYPE.ONE_TIME:
                    while (!Approximation(selfImage.color.a, fade_finalAlpha, 0.01f))
                    {
                        localDuration += Time.fixedDeltaTime / 2;
                        selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b,
                        Mathf.SmoothStep(selfImage.color.a, fade_finalAlpha, curve.Evaluate(localDuration / duration)));
                        yield return null;
                    }
                    selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b, fade_finalAlpha);
                    break;
                case ANIM_TYPE.LOOP:
                    while (true)
                    {
                        localDuration = 0f;
                        while (!Approximation(selfImage.color.a, fade_finalAlpha, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b,
                            Mathf.SmoothStep(selfImage.color.a, fade_finalAlpha, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b, fade_finalAlpha);
                        localDuration = 0f;
                        while (!Approximation(selfImage.color.a, first_alpha, 0.01f))
                        {
                            localDuration += Time.fixedDeltaTime / 2;
                            selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b,
                            Mathf.SmoothStep(selfImage.color.a, first_alpha, curve.Evaluate(localDuration / duration)));
                            yield return null;
                        }
                        selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b, first_alpha);
                    }

            }

            if (rotate_EndOfAnimation)
                rotate_endEvent.Invoke();

            isPlaying = false;
            yield return null;
        }
        public void RotateUI(RectTransform self, float duration)
        {
            StartCoroutine(Rotate(self, duration));
        }

        public bool Approximation(object a1, object a2, float value)
        {
            bool isApprox = true;
            if (a1 is Vector2 && a2 is Vector2)
            {
                if (Vector2.Distance((Vector2)a1, (Vector2)a2) < value)
                    isApprox = true;
                else
                    isApprox = false;
            }
            else if (a1 is Vector3 && a2 is Vector3)
            {
                if (Vector3.Distance((Vector3)a1, (Vector3)a2) < value)
                    isApprox = true;
                else
                    isApprox = false;
            }
            else if (a1 is float && a2 is float)
            {
                if (Mathf.Abs((float)a1 - (float)a2) < value)
                    isApprox = true;
                else
                    isApprox = false;
            }
            return isApprox;
        }



        public void Play()
        {
            StopAllCoroutines();
            switch (animationType)
            {
                case Mocha.ANIM_STYLE.MOVE:
                    GetComponent<Mocha>().MoveUI(GetComponent<RectTransform>(), move_duration);
                    break;
                case Mocha.ANIM_STYLE.SCALE:
                    GetComponent<Mocha>().ScaleUI(GetComponent<RectTransform>(), scale_duration);
                    break;
                case Mocha.ANIM_STYLE.ROTATE:
                    GetComponent<Mocha>().RotateUI(GetComponent<RectTransform>(), rotate_duration);
                    break;
                case Mocha.ANIM_STYLE.FADE:
                    GetComponent<Mocha>().FadeUI(GetComponent<RectTransform>(), fade_duration);
                    break;
                default:
                    break;
            };
        }
        
        public void ReversePlay()
        {
            StopAllCoroutines();
            StartCoroutine(Reverse());
        }

        IEnumerator Reverse()
        {
           // if (!isPlaying)
           // {
                switch (animationType)
                {
                    case Mocha.ANIM_STYLE.MOVE:
                        float localDuration_move = 0f;
                        while (!Approximation(GetComponent<RectTransform>().anchoredPosition, first_position, 0.01f))
                        {
                            localDuration_move += Time.fixedDeltaTime / 2;
                            GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.SmoothStep(GetComponent<RectTransform>().anchoredPosition.x, first_position.x, curve.Evaluate(localDuration_move / move_duration)),
                            Mathf.SmoothStep(GetComponent<RectTransform>().anchoredPosition.y, first_position.y, curve.Evaluate(localDuration_move / move_duration)));
                            yield return null;
                        }
                        GetComponent<RectTransform>().anchoredPosition = first_position;
                        break;
                    case Mocha.ANIM_STYLE.SCALE:
                        float localDuration_scale = 0f;
                        while (!Approximation(GetComponent<RectTransform>().localScale, first_scale, 0.01f))
                        {
                            localDuration_scale += Time.fixedDeltaTime / 2;
                            GetComponent<RectTransform>().localScale = new Vector3(Mathf.SmoothStep(GetComponent<RectTransform>().localScale.x, first_scale.x, curve.Evaluate(localDuration_scale / scale_duration)),
                            Mathf.SmoothStep(GetComponent<RectTransform>().localScale.y, first_scale.y, curve.Evaluate(localDuration_scale / scale_duration)),
                            Mathf.SmoothStep(GetComponent<RectTransform>().localScale.z, first_scale.z, curve.Evaluate(localDuration_scale / scale_duration)));
                            yield return null;
                        }
                        GetComponent<RectTransform>().localScale = first_scale;
                        break;
                    case Mocha.ANIM_STYLE.ROTATE:
                        float localDuration_rotation = 0f;
                        while (!Approximation(GetComponent<RectTransform>().eulerAngles, first_rotation, 0.01f))
                        {
                            localDuration_rotation += Time.fixedDeltaTime / 2;
                            GetComponent<RectTransform>().eulerAngles = new Vector3(Mathf.SmoothStep(GetComponent<RectTransform>().eulerAngles.x, first_rotation.x, curve.Evaluate(localDuration_rotation / rotate_duration)),
                            Mathf.SmoothStep(GetComponent<RectTransform>().eulerAngles.y, first_rotation.y, curve.Evaluate(localDuration_rotation / rotate_duration)),
                            Mathf.SmoothStep(GetComponent<RectTransform>().eulerAngles.z, first_rotation.z, curve.Evaluate(localDuration_rotation / rotate_duration)));
                            yield return null;
                        }
                        GetComponent<RectTransform>().eulerAngles = first_rotation;
                        break;
                    case Mocha.ANIM_STYLE.FADE:
                        float localDuration_fade = 0f;
                        while (!Approximation(selfImage.color.a, first_alpha, 0.01f))
                        {
                            localDuration_fade += Time.fixedDeltaTime / 2;
                            selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b,
                            Mathf.SmoothStep(selfImage.color.a, first_alpha, curve.Evaluate(localDuration_fade / fade_duration)));
                            yield return null;
                        }
                        selfImage.color = new Color(selfImage.color.r, selfImage.color.g, selfImage.color.b, first_alpha);
                        break;
                    default:
                        break;
                }
            }
       // }
    }
}