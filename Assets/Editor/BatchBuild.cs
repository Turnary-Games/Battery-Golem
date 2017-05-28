using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BatchBuild {

	[MenuItem("Batch Build/Batch Build Standalone (OS X, Windows, Linux)")]
	public static void BuildGame() {
		// Get filename
		string path = EditorUtility.SaveFilePanel("Choose location of built games", "", "", "");
		if (path.Length == 0) return;

		string filename;
		if (Path.GetExtension(path) == ".All files")
			filename = Path.GetFileNameWithoutExtension(path);
		else filename = Path.GetFileName(path);

		path = Path.Combine(Path.GetDirectoryName(path), filename);

		Debug.Log("Starting batch build...");

		_BuildGame(path, filename, BuildTarget.StandaloneWindows);
		_BuildGame(path, filename, BuildTarget.StandaloneWindows64);
		_BuildGame(path, filename, BuildTarget.StandaloneOSXIntel);
		_BuildGame(path, filename, BuildTarget.StandaloneOSXIntel64);
		_BuildGame(path, filename, BuildTarget.StandaloneLinux);
		string f = _BuildGame(path, filename, BuildTarget.StandaloneLinux64);


		Debug.Log("Batch build complete!");
		EditorUtility.DisplayDialog("Batch Build", "Building for Windows 32bit, Windows 64bit, OS X 32bit, OS X 64bit, Linux 32bit, Linux 64bit complete!", "OK");
		EditorUtility.RevealInFinder(f);
	}

	[MenuItem("Batch Build/Batch Build and Zip Standalones (OS X, Windows, Linux)")]
	public static void BuildGameAndZip() {
		// Get filename
		string path = EditorUtility.SaveFilePanel("Choose location of built games", "", "", "");
		if (path.Length == 0) return;

		string filename;
		if (Path.GetExtension(path) == ".All files")
			filename = Path.GetFileNameWithoutExtension(path);
		else filename = Path.GetFileName(path);

		Debug.Log("Starting batch build...");

		_BuildGameAndZip(path, filename, BuildTarget.StandaloneWindows);
		_BuildGameAndZip(path, filename, BuildTarget.StandaloneWindows64);
		_BuildGameAndZip(path, filename, BuildTarget.StandaloneOSXIntel);
		_BuildGameAndZip(path, filename, BuildTarget.StandaloneOSXIntel64);
		_BuildGameAndZip(path, filename, BuildTarget.StandaloneLinux);
		string f = _BuildGameAndZip(path, filename, BuildTarget.StandaloneLinux64);


		Debug.Log("Batch build complete!");
		EditorUtility.DisplayDialog("Batch Zip Build", "Building and zipping for Windows 32bit, Windows 64bit, OS X 32bit, OS X 64bit, Linux 32bit, Linux 64bit complete!", "OK");
		EditorUtility.RevealInFinder(f);
	}

	private static string _BuildGame(string path, string filename, BuildTarget target) {

		BuildPlayerOptions options = new BuildPlayerOptions();
		string ext = null;
		options.scenes = EditorBuildSettings.scenes
			.Where(s => s.enabled)
			.Select(s => s.path)
			.ToArray();
		options.target = target;

		switch (target) {
			case BuildTarget.StandaloneWindows: ext = "_win32.exe"; break;
			case BuildTarget.StandaloneWindows64: ext = "_win64.exe"; break;
			case BuildTarget.StandaloneOSXIntel: ext = "_osx32.app"; break;
			case BuildTarget.StandaloneOSXIntel64: ext = "_osx64.app"; break;
			case BuildTarget.StandaloneLinux: ext = "_linux32.x86"; break;
			case BuildTarget.StandaloneLinux64: ext = "_linux64.x86_64"; break;
			default: throw new System.Exception("Unsupported build target type \"" + target.ToString() + "\"");
		}

		string folder = path + Path.GetFileNameWithoutExtension(ext);
		Directory.CreateDirectory(folder);
		options.locationPathName = Path.Combine(path + Path.GetFileNameWithoutExtension(ext), filename + ext);

		string err = BuildPipeline.BuildPlayer(options);

		if (!string.IsNullOrEmpty(err))
			Debug.LogError("Error on building for target " + target.ToString() + "\n" + err);
		else
			Debug.Log("Successfully build for target " + target.ToString());

		return folder;
	}

	private static string _BuildGameAndZip(string path, string filename, BuildTarget target) {
		string folder = _BuildGame(path, filename, target);
		string zipname = folder + ".zip";

		ZipUtil.ZipFolder(zipname, folder);
		Directory.Delete(folder, true);

		return zipname;
	}

}