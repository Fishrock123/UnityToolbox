#if UNITY_EDITOR
using UnityToolbox.EditorTools;
using UnityEngine;
using System.IO;

namespace UnityToolbox.Internal
{
    /// <summary>
    /// SO is needed to determine the path to this script.
    /// Thereby it's used to get relative path to UnityToolbox
    /// </summary>
    public class UnityToolboxInternalPath : ScriptableObject
    {
        /// <summary>
        /// Absolute path to UnityToolbox folder
        /// </summary>
        public static DirectoryInfo UnityToolboxDirectory
        {
            get
            {
                if (_directoryChecked) return _UnityToolboxDirectory;
                
                var internalPath = MyEditor.GetScriptAssetPath(Instance);
                var scriptDirectory = new DirectoryInfo(internalPath);

                // Script is in UnityToolbox/Tools/Internal so we need to get dir two steps up in hierarchy
                if (scriptDirectory.Parent == null || scriptDirectory.Parent.Parent == null)
                {
                    _directoryChecked = true;
                    return null;
                }

                _UnityToolboxDirectory = scriptDirectory.Parent.Parent;
                _directoryChecked = true;
                return _UnityToolboxDirectory;
            }
        }

        private static DirectoryInfo _UnityToolboxDirectory;
        private static bool _directoryChecked;

        private static UnityToolboxInternalPath Instance
        {
            get
            {
                if (_instance != null) return _instance;
                return _instance = CreateInstance<UnityToolboxInternalPath>();
            }
        }

        private static UnityToolboxInternalPath _instance;
    }
}
#endif
