#if UNITY_EDITOR
using UnityEditor;

namespace UnityToolbox.Internal
{
	[InitializeOnLoad]
	public class UnityToolboxFeatures
	{
		private const string AutoSaveMenuItemKey = "Tools/UnityToolbox/AutoSave on play";
		private const string CleanupEmptyDirectoriesMenuItemKey = "Tools/UnityToolbox/Clear empty directories On Save";
		private const string IPrepareMenuItemKey = "Tools/UnityToolbox/Run Prepare on play";
		private const string CheckForUpdatesKey = "Tools/UnityToolbox/Check for updates on start";

		static UnityToolboxFeatures()
		{
			AutoSaveIsEnabled = AutoSaveIsEnabled;
			CleanupEmptyDirectoriesIsEnabled = CleanupEmptyDirectoriesIsEnabled;
			IPrepareIsEnabled = IPrepareIsEnabled;
			CheckForUpdatesEnabled = CheckForUpdatesEnabled;
		}


		#region AutoSave

		private static bool AutoSaveIsEnabled
		{
			get { return UnityToolboxSettings.AutoSaveEnabled; }
			set
			{
				{
					UnityToolboxSettings.AutoSaveEnabled = value;
					AutoSaveFeature.IsEnabled = value;
				}
			}
		}

		[MenuItem(AutoSaveMenuItemKey, priority = 100)]
		private static void AutoSaveMenuItem()
		{
			AutoSaveIsEnabled = !AutoSaveIsEnabled;
		}

		[MenuItem(AutoSaveMenuItemKey, true)]
		private static bool AutoSaveMenuItemValidation()
		{
			Menu.SetChecked(AutoSaveMenuItemKey, AutoSaveIsEnabled);
			return true;
		}

		#endregion


		#region CleanupEmptyDirectories

		private static bool CleanupEmptyDirectoriesIsEnabled
		{
			get { return UnityToolboxSettings.CleanEmptyDirectoriesFeature; }
			set
			{
				{
					UnityToolboxSettings.CleanEmptyDirectoriesFeature = value;
					CleanEmptyDirectoriesFeature.IsEnabled = value;
				}
			}
		}

		[MenuItem(CleanupEmptyDirectoriesMenuItemKey, priority = 100)]
		private static void CleanupEmptyDirectoriesMenuItem()
		{
			CleanupEmptyDirectoriesIsEnabled = !CleanupEmptyDirectoriesIsEnabled;
		}

		[MenuItem(CleanupEmptyDirectoriesMenuItemKey, true)]
		private static bool CleanupEmptyDirectoriesMenuItemValidation()
		{
			Menu.SetChecked(CleanupEmptyDirectoriesMenuItemKey, CleanupEmptyDirectoriesIsEnabled);
			return true;
		}

		#endregion


		#region IPrepare

		private static bool IPrepareIsEnabled
		{
			get { return UnityToolboxSettings.PrepareOnPlaymode; }
			set
			{
				{
					UnityToolboxSettings.PrepareOnPlaymode = value;
					IPrepareFeature.IsEnabled = value;
				}
			}
		}

		[MenuItem(IPrepareMenuItemKey, priority = 100)]
		private static void IPrepareMenuItem()
		{
			IPrepareIsEnabled = !IPrepareIsEnabled;
		}

		[MenuItem(IPrepareMenuItemKey, true)]
		private static bool IPrepareMenuItemValidation()
		{
			Menu.SetChecked(IPrepareMenuItemKey, IPrepareIsEnabled);
			return true;
		}

		#endregion
		
		
		#region Check For Updates

		private static bool CheckForUpdatesEnabled
		{
			get { return UnityToolboxSettings.CheckForUpdates; }
			set
			{
				{
					UnityToolboxSettings.CheckForUpdates = value;
					UnityToolboxUpdateWindow.AutoUpdateCheckIsEnabled = value;
				}
			}
		}

		[MenuItem(CheckForUpdatesKey, priority = 100)]
		private static void CheckForUpdatesMenuItem()
		{
			CheckForUpdatesEnabled = !CheckForUpdatesEnabled;
		}

		[MenuItem(CheckForUpdatesKey, true)]
		private static bool CheckForUpdatesMenuItemValidation()
		{
			Menu.SetChecked(CheckForUpdatesKey, CheckForUpdatesEnabled);
			return true;
		}

		#endregion
	}
}
#endif
