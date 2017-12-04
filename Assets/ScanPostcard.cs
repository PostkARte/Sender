using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;


public class ScanPostcard : MonoBehaviour 
{
	public GameObject canvas;
	public GameObject rawImagePostcard;
	public GameObject rawImageObject;
	public Button maincanvas;
	private CanvasGroup maincanvasgroup;
	private RectTransform cameraSize;//拍攝和顯示的影像大小
	private RawImage showImage;//顯示圖片
	private WebCamTexture myCam;//接收攝影機讀取到的圖片數據
	//private string filepath = @"/Users/hsiehyuanzhang/Documents/picture.jpg";//儲存照片的路徑
	private RawImage showPostcardImage;//顯示圖片

	private void Start ()
	{
		//取得RectTransform和RawImage元件
		cameraSize = rawImageObject.GetComponent<RectTransform>();
		showImage = rawImageObject.GetComponent<RawImage>();
		maincanvasgroup = maincanvas.GetComponent<CanvasGroup>();
		showPostcardImage = rawImagePostcard.GetComponent<RawImage>();
		StartCoroutine(OpenCamera());//開啟攝影機鏡頭
		//Debug.Log(canvas.GetComponentsInChildren<Canvas>()[1].GetComponent<Canvas>().sortingOrder);
	}

	private void OnGUI ()
	{
		//若有開啟鏡頭則將拍到的畫面顯示出來
		if (myCam != null)
		{
			/* (new Rect(影像起始x軸，影像起始y軸，要顯示出來的寬度，要顯示出來的高度), 顯示的影像或圖片) */
			//			GUI.DrawTexture(new Rect(0, 0, (int)cameraSize.rect.width/2, (int)cameraSize.rect.height/2), myCam);
			showImage.texture = myCam;

		}

	}

	public void GetButton()
	{
		StartCoroutine(GetPicture());//拍照
		maincanvasgroup.alpha = 0;
	}

	private IEnumerator OpenCamera()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);//授權開啟鏡頭

		if (Application.HasUserAuthorization(UserAuthorization.WebCam))//若同意開啟攝影機
		{
			//設置攝影機截取到的影像範圍
			/* (攝影機名稱, 攝影機影像的寬度, 攝影機影像的高度, 攝影機的FPS) */
			myCam = new WebCamTexture(WebCamTexture.devices[0].name, (int)cameraSize.rect.width, (int)cameraSize.rect.height, 30);
			myCam.Play();//開啟攝影機
		}
	}

	private IEnumerator GetPicture()
	{
		yield return new WaitForEndOfFrame(); //攝影機讀取到的Frame繪製完畢後才進行拍照

		Texture2D t = new Texture2D((int)cameraSize.rect.width, (int)cameraSize.rect.height);//要保存的圖片大小
		t.ReadPixels(new Rect(0, Screen.height - (int)cameraSize.rect.height, (int)cameraSize.rect.width, (int)cameraSize.rect.height), 0, 0, true);//圖片截取的區域//圖片截取的區域
		maincanvasgroup.alpha = 1;
		/*using (FileStream fs = File.Open(filepath, FileMode.Create))
		{
			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write(t.EncodeToJPG());
		}*/
		t.Apply ();

		//Debug.Log(canvas.GetComponentsInChildren<Canvas>()[1].GetComponent<Canvas>().sortingOrder);
		canvas.GetComponentsInChildren<Canvas> () [1].GetComponent<Canvas> ().sortingOrder = 0;
		//Debug.Log(canvas.GetComponentsInChildren<Canvas>()[2]);
		canvas.GetComponentsInChildren<Canvas> () [2].GetComponent<Canvas> ().sortingOrder = 1;

		showPostcardImage.texture = t as Texture;
		//yield return StartCoroutine(Upload(t));
		Debug.Log("拍照完成！");
	}

	IEnumerator Upload(Texture2D t) {
		WWWForm wwwF = new WWWForm ();
		wwwF.AddBinaryData ("postcard", t.EncodeToJPG());
		WWW www = new WWW ("http://35.196.236.27:3000/postcard/", wwwF);
		yield return www;
		Debug.Log (www.text);
	}
	public void Send()
	{
		Texture2D t = showPostcardImage.texture as Texture2D;
		StartCoroutine(Upload(t));
		canvas.GetComponentsInChildren<Canvas> () [2].GetComponent<Canvas> ().sortingOrder = 0;
		canvas.GetComponentsInChildren<Canvas> () [3].GetComponent<Canvas> ().sortingOrder = 1;
		myCam.Stop();
	}
	public void Back()
	{
		canvas.GetComponentsInChildren<Canvas> () [1].GetComponent<Canvas> ().sortingOrder = 1;
		canvas.GetComponentsInChildren<Canvas> () [2].GetComponent<Canvas> ().sortingOrder = 0;
	}

	private void OnDisable()
	{
		myCam.Stop();//離開當前Scene後關閉攝影機
	}
}