using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class BatteryGolemAnalytics : MonoBehaviour {

	static float startTime = 0;
	static bool completedGame = false;
	static Dictionary<string, object> message = new Dictionary<string, object>();

	public static void SendGameCompletedEvent() {
		message.Clear();
		message["playtime"] = Time.time - startTime;
		message["version"] = BatteryGolemVersion.FormatVersion(BatteryGolemVersion.CURRENT);

		Analytics.CustomEvent("game_complete", message);

		completedGame = true;
	}

	public static void SendGameStartedEvent() {
		message.Clear();
		message["version"] = BatteryGolemVersion.FormatVersion(BatteryGolemVersion.CURRENT);
		message["completed_game"] = completedGame;

		Analytics.CustomEvent("game_started", message);

		startTime = Time.time;
	}

	public static void SendGameQuitEvent() {
		message.Clear();
		message["version"] = BatteryGolemVersion.FormatVersion(BatteryGolemVersion.CURRENT);
		message["completed_game"] = completedGame;

		Analytics.CustomEvent("game_quit", message);
	}
	
}
