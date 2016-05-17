using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEditor;

public class BatteryGolemVersion : MonoBehaviour {

	public static readonly Version CURRENT = new Version(1, 0, 6);
	public static readonly string LATEST_URL = "https://api.github.com/repos/Turnary-Games/Battery-Golem/releases/latest";

	public Text currentText;
	public Text latestText;

	private string downloadPage;

	IEnumerator Start () {
		currentText.text = currentText.text.Replace("%VERSION%", FormatVersion(CURRENT));
		latestText.gameObject.SetActive(false);
		
		// Fetch latest version
		using (WWW www = new WWW(LATEST_URL)) {
			// Wait for HTTP request
			yield return www;

			// Error checking
			if (string.IsNullOrEmpty(www.text)) goto Failed;  // Got data
			if (!string.IsNullOrEmpty(www.error)) goto Failed; // Some error

			// Parse JSON data
			ReleaseData data = (ReleaseData)JsonUtility.FromJson(www.text, typeof(ReleaseData));
			if (data == null) goto Failed; // Failed to parse data
			
			// Trim away everything but numbers and dots
			string tag = Regex.Replace(data.tag_name, "[^0-9.^\\.]", "");
			// Parse into version object
			Version latest = new Version(tag);

			// Check if latest is newer than current
			if (CURRENT.CompareTo(latest) < 0) {
				latestText.gameObject.SetActive(true);
				latestText.text = latestText.text.Replace("%VERSION%", FormatVersion(latest));
				downloadPage = data.html_url;
			}
			goto End;

		Failed:
			print("FAILED TO CHECK LATEST VERSION!");

		End:;
		}
	}

	public void OpenGameDownloadPage() {
		if (downloadPage != null)
			Application.OpenURL(downloadPage);
	}

	string FormatVersion(Version version) {
		return string.Format("v{0}.{1}.{2}", version.Major, version.Minor, version.Build);
	}

	[Serializable]
	public class ReleaseData {
		public string url;
		public string assets_url;
		public string upload_url;
		public string html_url;
		public int id;
		public string tag_name;
		public string target_commitish;
		public string name;
		public bool draft;
		public bool prerelease;
		public string created_at;
		public string published_at;
	}

}
