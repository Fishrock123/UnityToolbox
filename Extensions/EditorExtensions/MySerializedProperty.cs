#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnityToolbox.EditorTools
{
	public static class MySerializedProperty
	{
		/// <summary>
		/// Get array of property childs, if parent property is array
		/// </summary>
		public static SerializedProperty[] AsArray(this SerializedProperty property)
		{
			List<SerializedProperty> items = new List<SerializedProperty>();
			for (int i = 0; i < property.arraySize; i++)
				items.Add(property.GetArrayElementAtIndex(i));
			return items.ToArray();
		}

		/// <summary>
		/// Get array of property childs casted to specific type
		/// </summary>
		public static T[] AsArray<T>(this SerializedProperty property)
		{
			var propertiesArray = property.AsArray();
			return propertiesArray.Select(s => s.objectReferenceValue).OfType<T>().ToArray();
		}

		/// <summary>
		/// Get array of property childs, if parent property is array
		/// </summary>
		public static IEnumerable<SerializedProperty> AsIEnumerable(this SerializedProperty property)
		{
			for (int i = 0; i < property.arraySize; i++)
				yield return property.GetArrayElementAtIndex(i);
		}

		/// <summary>
		/// Replace array contents of SerializedProperty with another array 
		/// </summary>
		public static void ReplaceArray(this SerializedProperty property, Object[] newElements)
		{
			property.arraySize = 0;
			property.serializedObject.ApplyModifiedProperties();
			property.arraySize = newElements.Length;
			for (var i = 0; i < newElements.Length; i++)
			{
				property.GetArrayElementAtIndex(i).objectReferenceValue = newElements[i];
			}

			property.serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// If property is array, insert new element at the end and get it as a property
		/// </summary>
		public static SerializedProperty NewElement(this SerializedProperty property)
		{
			int newElementIndex = property.arraySize;
			property.InsertArrayElementAtIndex(newElementIndex);
			return property.GetArrayElementAtIndex(newElementIndex);
		}


		/// <summary>
		/// Get string representation of serialized property
		/// </summary>
		public static string AsStringValue(this SerializedProperty property)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.String:
					return property.stringValue;

				case SerializedPropertyType.Character:
				case SerializedPropertyType.Integer:
					if (property.type == "char") return Convert.ToChar(property.intValue).ToString();
					return property.intValue.ToString();

				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue != null ? property.objectReferenceValue.ToString() : "null";

				case SerializedPropertyType.Boolean:
					return property.boolValue.ToString();

				case SerializedPropertyType.Enum:
					return property.enumNames[property.enumValueIndex];

				default:
					return string.Empty;
			}
		}


		/// <summary>
		/// Get FieldInfo out of SerializedProperty
		/// </summary>
		public static FieldInfo GetFieldInfo(this SerializedProperty property)
		{
			var targetObject = property.serializedObject.targetObject;
			var targetType = targetObject.GetType();
			return targetType.GetField(property.propertyPath);
		}

		/// <summary>
		/// Is specific attribute defined on SerializedProperty
		/// </summary>
		/// <param name="property"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsAttributeDefined<T>(this SerializedProperty property) where T : Attribute
		{
			var fieldInfo = property.GetFieldInfo();
			if (fieldInfo == null) return false;
			return Attribute.IsDefined(fieldInfo, typeof(T));
		}

        #region SerializedProperty Get Parent

        // Found here http://answers.unity.com/answers/425602/view.html
        // Update here https://gist.github.com/AdrienVR/1548a145c039d2fddf030ebc22f915de to support inherited private members.
        /// <summary>
        /// Get parent object of SerializedProperty
        /// </summary>
        public static object GetParent(this SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal)).Replace("[", "").Replace("]", ""));
                    obj = GetValueAt(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }

            return obj;
        }

        private static object GetValue(object source, string name)
        {
            if (source == null)
                return null;

            foreach (var type in GetHierarchyTypes(source.GetType()))
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);
            }
            return null;
        }
        
        private static IEnumerable<Type> GetHierarchyTypes(Type sourceType)
        {
	        yield return sourceType;
	        while (sourceType.BaseType != null)
	        {
		        yield return sourceType.BaseType;
		        sourceType = sourceType.BaseType;
	        }
        }

        private static object GetValueAt(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            if (enumerable == null) return null;

            var enm = enumerable.GetEnumerator();
            while (index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }

        #endregion
    }
}
#endif
