using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extns
    {
    public static IEnumerator Tweeng( this float duration,
               System.Action<float> var, float aa, float zz, System.Action callback)
        {
        float sT = Time.time;
        float eT = sT + duration;
        
        while (Time.time < eT)
            {
            float t = (Time.time-sT)/duration;
            var( Mathf.SmoothStep(aa, zz, t) );
            yield return null;
            }
        
        var(zz);
        if(callback != null) { callback(); }
        }
        

    public static IEnumerator Tweeng( this float duration,
               System.Action<Vector3> var, Vector3 aa, Vector3 zz, System.Action callback )
        {
        float sT = Time.time;
        float eT = sT + duration;
        
        while (Time.time < eT)
            {
            float t = (Time.time-sT)/duration;
            var( Vector3.Lerp(aa, zz, t ) );
            yield return null;
            }
        
        var(zz);
        if(callback != null) { callback(); }
        }
}