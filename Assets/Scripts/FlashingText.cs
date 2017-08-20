using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingText : MonoBehaviour
{
    enum FadePhase { FadeIn, Stay, FadeOut };

    public float fadeInTime = 0.4F;
    public float stayTime = 0.2F;
    public float fadeOutTime = 0.4F;

    Text text;
    FadePhase stage;
    float delay;
    float r;
    float g;
    float b;
    float a;
    float alphaStep;

	void Start()
    {
        text = gameObject.GetComponent<Text>();
        stage = FadePhase.FadeIn;
        delay = fadeInTime;
        r = text.color.r;
        g = text.color.g;
        b = text.color.b;
        a = 0;
        text.color = new Color( r, g, b, a );
        alphaStep = 1 / fadeInTime;
	}
	
	void Update()
    {
        delay -= Time.deltaTime;

        switch( stage )
        {
            case FadePhase.FadeIn:
                a += ( alphaStep * Time.deltaTime );
                text.color = new Color( r, g, b, a );
                if( delay < 0 )
                {
                    stage = FadePhase.Stay;
                    delay = stayTime;
                }
                break;
            case FadePhase.Stay:
                if( delay < 0 )
                {
                    stage = FadePhase.FadeOut;
                    delay = fadeOutTime;
                    alphaStep = 1 / fadeOutTime;
                }
                break;
            case FadePhase.FadeOut:
                a -= ( alphaStep * Time.deltaTime );
                text.color = new Color( r, g, b, a );
                if( delay < 0 )
                {
                    stage = FadePhase.FadeIn;
                    delay = fadeInTime;
                    alphaStep = 1 / fadeInTime;
                }
                break;
        }
	}
}
