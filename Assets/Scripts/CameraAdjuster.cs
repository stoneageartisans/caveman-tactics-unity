using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    void Start ()
    {
        float aspectRatio = Camera.main.aspect;

        if( aspectRatio > 1.7 )
        {
            Camera.main.transform.position = new Vector3( 0, 0, -8.66F );
        }
        else if( aspectRatio < 1.7 && aspectRatio > 1.51 )
        {
            Camera.main.transform.position = new Vector3( 0, 0, -8.66F );
        }
        else if( aspectRatio < 1.51 && aspectRatio > 1.49 )
        {
            Camera.main.transform.position = new Vector3( 0, 0, -9.24F );
        }
        else if( aspectRatio < 1.4 && aspectRatio > 1.3 )
        {
            Camera.main.transform.position = new Vector3( 0, 0, -10.39F );
        }
        else if( aspectRatio < 1.3 && aspectRatio > 1.2 )
        {
            Camera.main.transform.position = new Vector3( 0, 0, -11.09F );
        }
        else
        {
            Camera.main.transform.position = new Vector3( 0, 0, -10 );
        }
	}
}
