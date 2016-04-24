using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour {

	public string folder = "/Users/ootaisao/ScreenshotFolder";
	public int frameRate = 5;

	[SerializeField] bool m_captureFlag = false;

	void Start() {
		// Set the playback framerate (real time will not relate to game time after this).
		Time.captureFramerate = frameRate;

		// Create the folder
		System.IO.Directory.CreateDirectory(folder);
	}


	void Update() {
		if(m_captureFlag){
			// Append filename to folder name (format is '0005 shot.png"')
			string name = string.Format("{0}/{1:D04} shot.png", folder, Time.frameCount);

			// Capture the screenshot to the specified file.
			Application.CaptureScreenshot(name);
		}
	}
}
