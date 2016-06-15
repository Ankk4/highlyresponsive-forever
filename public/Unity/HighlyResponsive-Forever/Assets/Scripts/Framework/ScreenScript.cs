using UnityEngine; 
using System.Collections; 


public class ScreenScript : MonoBehaviour 
{ 
    public static Vector2 GetAspectRatio()
    { 
        float f = (float)Screen.width / (float)Screen.height; 
        int i = 0; 
        while (true) 
        { 
            i++; 
            if (System.Math.Round(f* i, 2) == Mathf.RoundToInt(f* i)) 
            break; 
        } 
        return new Vector2((float)System.Math.Round(f* i, 2), i); 
    } 

    public static Vector2 GetScreenSize() // If object is 1x1 unit in size 
    { 
        /*Bounds b = new Bounds(); 
        Vector2 aspect = GetAspectRatio(); 
        float orthoSize = Camera.main.orthographicSize; 
        b.center = Vector3.zero; 
        b.max = new Vector3(orthoSize / aspect.y * aspect.x, orthoSize); 
        b.min = -b.max; 
        return b;*/ 

        // Force set aspect ratio to be 9 by 16 
        Vector2 aspect = /*new Vector2(9, 16);*/GetAspectRatio(); 
        float orthoSize = Camera.main.orthographicSize; 
        return new Vector2( 
            (orthoSize / aspect.y* aspect.x* 2), 
            (orthoSize* 2) 
            ); 
    } 

    public static Vector2 GetScreenSizeByObject(Vector2 origSize, float pixelPerUnit) // If object is NOT 1x1 unit in size 
    { 
        /*Bounds b = new Bounds(); 
        b.center = Vector3.zero; 
            b.max = new Vector3(, orthoSize); 
        b.min = -b.max;*/ 

        Vector2 aspect = GetAspectRatio(); 
        float orthoSize = Camera.main.orthographicSize; 
            return new Vector2( (orthoSize / aspect.y* aspect.x* 2) / (origSize.x / pixelPerUnit), 
            (orthoSize* 2) / (origSize.y / pixelPerUnit) 
        ); 
    } 
} 
