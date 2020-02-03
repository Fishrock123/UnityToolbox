﻿#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityToolbox.Internal
{
	[Serializable]
	public class UnityToolboxSettings : ScriptableObject
	{
		[SerializeField] private bool _autoSaveEnabled = true;
		[SerializeField] private bool _cleanEmptyDirectoriesFeature = true;
		[SerializeField] private bool _prepareOnPlaymode = true;
		[SerializeField] private bool _checkForUpdates = true;

		public static bool AutoSaveEnabled
		{
			get { return Instance._autoSaveEnabled; }
			set
			{
				if (Instance._autoSaveEnabled == value) return;
				Instance._autoSaveEnabled = value;
				Save();
			}
		}

		public static bool CleanEmptyDirectoriesFeature
		{
			get { return Instance._cleanEmptyDirectoriesFeature; }
			set
			{
				if (Instance._cleanEmptyDirectoriesFeature == value) return;
				Instance._cleanEmptyDirectoriesFeature = value;
				Save();
			}
		}

		public static bool PrepareOnPlaymode
		{
			get { return Instance._prepareOnPlaymode; }
			set
			{
				if (Instance._prepareOnPlaymode == value) return;
				Instance._prepareOnPlaymode = value;
				Save();
			}
		}

		public static bool CheckForUpdates
		{
			get { return Instance._checkForUpdates; }
			set
			{
				if (Instance._checkForUpdates == value) return;
				Instance._checkForUpdates = value;
				Save();
			}
		}

		#region Instance

		private static UnityToolboxSettings Instance
		{
			get
			{
				if (_instance != null) return _instance;
				_instance = LoadOrCreate();
				return _instance;
			}
		}

		private static readonly string Directory = "ProjectSettings";
		private static readonly string Path = Directory + "/UnityToolboxSettings.asset";
		private static UnityToolboxSettings _instance;

		private static void Save()
		{
			var instance = _instance;
			if (!System.IO.Directory.Exists(Directory)) System.IO.Directory.CreateDirectory(Directory);
			try
			{
				UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] {instance}, Path, true);
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to save UnityToolboxSettings!\n" + ex);
			}
		}

		private static UnityToolboxSettings LoadOrCreate()
		{
			var settings = !File.Exists(Path) ? CreateNewSettings() : LoadSettings();
			if (settings == null)
			{
				DeleteFile(Path);
				settings = CreateNewSettings();
			}

			settings.hideFlags = HideFlags.HideAndDontSave;

			return settings;
		}


		private static UnityToolboxSettings LoadSettings()
		{
			UnityToolboxSettings settingsInstance;
			try
			{
				settingsInstance = (UnityToolboxSettings) UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(Path)[0];
			}
			catch (Exception ex)
			{
				Debug.LogError("Unable to read UnityToolboxSettings, set to defaults" + ex);
				settingsInstance = null;
			}

			return settingsInstance;
		}

		private static UnityToolboxSettings CreateNewSettings()
		{
			_instance = CreateInstance<UnityToolboxSettings>();
			Save();

			return _instance;
		}

		private static void DeleteFile(string path)
		{
			if (!File.Exists(path)) return;

			var attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);

			File.Delete(path);
		}

		#endregion
	}
}
#endif
