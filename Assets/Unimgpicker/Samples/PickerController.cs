using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kakera
{
    public class PickerController : MonoBehaviour
    {
		public GameObject canvas;
        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
		private RawImage imageRenderer;
		public Button[] buttonlist;



        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
				StartCoroutine(LoadImage(path, imageRenderer));
            };
			buttonlist=canvas.GetComponentsInChildren<Button> ();
			buttonlist [1].gameObject.SetActive (false);
			buttonlist [2].gameObject.SetActive (false);
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "lalala", 1024);
			//Debug.Log (canvas.GetComponentsInChildren<Button> () .GetType());
			buttonlist [0].gameObject.SetActive (false);
			buttonlist [1].gameObject.SetActive (true);
			buttonlist [2].gameObject.SetActive (true);
        }

		private IEnumerator LoadImage(string path, RawImage output)
        {
            var url = "file://" + path;

            var www = new WWW(url);
            yield return www;
			Debug.Log (url);
            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

            //output.material.mainTexture = texture;
			output.texture = texture;

        }
		public void Send()
		{
			buttonlist [0].gameObject.SetActive (true);
			buttonlist [1].gameObject.SetActive (false);
			buttonlist [2].gameObject.SetActive (false);
			StartCoroutine(UploadImage());

		}
		public void Back(){
			buttonlist [0].gameObject.SetActive (true);
			buttonlist [1].gameObject.SetActive (false);
			buttonlist [2].gameObject.SetActive (false);
		}
		private IEnumerator UploadImage(){
			Texture2D TheTexture;
			TheTexture = imageRenderer.texture as Texture2D;
			WWWForm wwwF = new WWWForm ();
			wwwF.AddBinaryData ("image", TheTexture.EncodeToJPG() );
			WWW www1 = new WWW ("http://35.196.236.27:3000/postcard/", wwwF);
			yield return www1;
			Debug.Log (www1.text);

		}
    }
}