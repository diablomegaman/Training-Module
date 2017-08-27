using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadPicture : MonoBehaviour {

    public RawImage Image;
    private Texture2D tex;
    private string url = "http://localhost:81/OneDetroitDarklogoLong.jpg";

    IEnumerator Start()
    {
        tex = new Texture2D(16, 16, TextureFormat.RGB24, false);

        WWW w = new WWW(url);
        yield return true;

        if (w.error != null)
        {
            print("pic could not load" + w.error);
        }
        else
        {
            w.LoadImageIntoTexture(tex);
            Image.texture = tex;
        }
    }

}
