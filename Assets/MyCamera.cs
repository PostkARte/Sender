using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyCamera : MonoBehaviour {

	public GameObject g;

	WebCamTexture c;

	void Start () {
		c = new WebCamTexture (WebCamTexture.devices[0].name);
		GetComponent<Renderer>().material.mainTexture = c;
		c.Play ();
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0, 0, 100, 50), "拍照")){
			Texture2D t = new Texture2D(c.width, c.height);
			t.SetPixels(c.GetPixels());
			t.Apply();
			g.GetComponent<Renderer>().material.mainTexture = t;
			//          System.IO.File.WriteAllBytes("image.png", t.EncodeToPNG());  // 如果你要保存至硬碟or記憶卡的話
		}
	}

}

