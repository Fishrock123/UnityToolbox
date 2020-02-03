#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityToolbox.Internal
{
	[InitializeOnLoad]
	public class UnityToolboxUpdateWindow : EditorWindow
	{
		public static bool AutoUpdateCheckIsEnabled = true;

		private static UnityToolboxVersion _installedVersion;
		private static UnityToolboxVersion _latestVersion;

		private static EditorWindow _windowInstance;
		

		static UnityToolboxUpdateWindow()
		{
			if (AutoUpdateCheckIsEnabled)
			{
				UnityToolboxUtilities.GetUnityToolboxLatestVersionAsync(version =>
				{
					_installedVersion = UnityToolboxUtilities.GetUnityToolboxInstalledVersion();
					_latestVersion = version;
					if (!_installedVersion.VersionsMatch(_latestVersion))
					{
						var versions = "Installed version: " + _installedVersion.AsSting + ". Latest version: " + _latestVersion.AsSting;
						Debug.Log("It's time to update UnityToolbox :)! Use \"Tools/UnityToolbox/Update UnityToolbox\". " + versions);
					}
				});
			}
		}

		[MenuItem("Tools/UnityToolbox/Update UnityToolbox")]
		private static void MuBoxUpdateMenuItem()
		{
			_windowInstance = GetWindow<UnityToolboxUpdateWindow>();
			_windowInstance.titleContent = new GUIContent("Update UnityToolbox");
		}

		private void OnEnable()
		{
			_windowInstance = this;

			_installedVersion = UnityToolboxUtilities.GetUnityToolboxInstalledVersion();
			UnityToolboxUtilities.GetUnityToolboxLatestVersionAsync(version =>
			{
				_latestVersion = version;
				if (_windowInstance != null) _windowInstance.Repaint();
			});
		}

		
		private void OnGUI()
		{
			EditorGUILayout.LabelField("You are using " + (UnityToolboxUtilities.InstalledViaUPM ? "PackageManager version!" : "Git version!"));
			if (!UnityToolboxUtilities.InstalledViaUPM) EditorGUILayout.LabelField("PackageManager version is easier to update ;)");
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Current version: " + (_installedVersion == null ? "..." : _installedVersion.AsSting));
			EditorGUILayout.LabelField("Latest version: " + (_latestVersion == null ? "..." : _latestVersion.AsSting));

			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Update GIT packages", EditorStyles.toolbarButton))
				{
					if (!UnityToolboxUtilities.UpdateGitPackages()) 
						ShowNotification(new GUIContent("There is no git packages installed"));
				}

				if (GUILayout.Button("Open Git releases page", EditorStyles.toolbarButton))
				{
					UnityToolboxUtilities.OpenUnityToolboxGitInBrowser();
				}
			}
		}
	}
}
#endif
