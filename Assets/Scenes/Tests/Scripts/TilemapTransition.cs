using System.Collections;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tests
{
    public class TilemapTransition : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;
        [SerializeField] Tilemap altTilemap;
        [SerializeField] float transitionDuration = 0.5f;
        // [SerializeField] float relativeSpeed = 1f;

        [Header("Stats")]
        [SerializeField] float elapsedTime;
        [SerializeField] float time;
        [SerializeField] float alpha;

        private float relativeSpeed;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            relativeSpeed = 1f / transitionDuration;
            StartCoroutine(Co_FadeInAltTilemap());
        }

        private (float elapsed, float remaining) CalcTime(float startAlpha, float elapsedTime, float speed)
        {
            var t1 = 1f - startAlpha + elapsedTime * relativeSpeed;
            var t2 = startAlpha + elapsedTime * relativeSpeed;
            return (t1, t2);
        }

        private IEnumerator FadeOutTilemap(Tilemap thisTilemap)
        {
            var startAlpha = thisTilemap.color.a;
            elapsedTime = 0;

            while (thisTilemap.color.a != 0f)
            {
                var time = 1f - startAlpha + elapsedTime * relativeSpeed;
                // time = CalcTime(startAlpha, elapsedTime, relativeSpeed).elapsed;
                alpha = Mathf.Lerp(1f, 0f, time);
                thisTilemap.color = new Color(1f, 1f, 1f, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator FadeInTilemap(Tilemap thisTilemap)
        {
            var startAlpha = thisTilemap.color.a;
            elapsedTime = 0;

            while (thisTilemap.color.a != 1f)
            {
                var time = startAlpha + elapsedTime * relativeSpeed;
                // time = CalcTime(startAlpha, elapsedTime, relativeSpeed).remaining;
                alpha = Mathf.Lerp(0f, 1f, time);
                thisTilemap.color = new Color(1f, 1f, 1f, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        // private IEnumerator Co_FadeInTilemap()
        // {
        //     var startAlpha = altTilemap.color.a;
        //     elapsedTime = 0;

        //     while (altTilemap.color.a != 0f)
        //     {
        //         // var time = 1f - startAlpha + elapsedTime * relativeSpeed;
        //         time = CalcTime(startAlpha, elapsedTime, relativeSpeed).elapsed;
        //         alpha = Mathf.Lerp(1f, 0f, time);
        //         altTilemap.color = new Color(1f, 1f, 1f, alpha);
        //         elapsedTime += Time.deltaTime;
        //         yield return null;
        //     }

        //     startAlpha = tilemap.color.a;
        //     elapsedTime = 0;

        //     while (tilemap.color.a != 1f)
        //     {
        //         // var time = startAlpha + elapsedTime * relativeSpeed;
        //         time = CalcTime(startAlpha, elapsedTime, relativeSpeed).remaining;
        //         alpha = Mathf.Lerp(0f, 1f, time);
        //         tilemap.color = new Color(1f, 1f, 1f, alpha);
        //         elapsedTime += Time.deltaTime;
        //         yield return null;
        //     }

        //     StartCoroutine(Co_FadeInAltTilemap());
        // }

        // private IEnumerator Co_FadeInAltTilemap()
        // {
        //     var startAlpha = tilemap.color.a;
        //     elapsedTime = 0;

        //     while (tilemap.color.a != 0f)
        //     {
        //         // var time = 1f - startAlpha + elapsedTime * relativeSpeed;
        //         time = CalcTime(startAlpha, elapsedTime, relativeSpeed).elapsed;
        //         alpha = Mathf.Lerp(1f, 0f, time);
        //         tilemap.color = new Color(1f, 1f, 1f, alpha);
        //         elapsedTime += Time.deltaTime;
        //         yield return null;
        //     }

        //     startAlpha = altTilemap.color.a;
        //     elapsedTime = 0;

        //     while (altTilemap.color.a != 1f)
        //     {
        //         // var time = startAlpha + elapsedTime * relativeSpeed;
        //         time = CalcTime(startAlpha, elapsedTime, relativeSpeed).remaining;
        //         alpha = Mathf.Lerp(0f, 1f, time);
        //         altTilemap.color = new Color(1f, 1f, 1f, alpha);
        //         elapsedTime += Time.deltaTime;
        //         yield return null;
        //     }

        //     StartCoroutine(Co_FadeInTilemap());
        // }

        private IEnumerator Co_FadeInAltTilemap()
        {
            yield return FadeOutTilemap(tilemap);
            yield return FadeInTilemap(altTilemap);
            StartCoroutine(Co_FadeInTilemap());
        }

        private IEnumerator Co_FadeInTilemap()
        {
            yield return FadeOutTilemap(altTilemap);
            yield return FadeInTilemap(tilemap);
            StartCoroutine(Co_FadeInAltTilemap());
        }
    }
}