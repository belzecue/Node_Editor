using UnityEngine;
using UnityEditor;
public static class PropertyDrawerExtension
{
	public static void DrawDefaultProperty(this PropertyDrawer propertyDrawer,Rect position, SerializedProperty property, GUIContent label)
	{
		if(property.propertyType == SerializedPropertyType.Boolean)
		{
			property.boolValue = EditorGUI.Toggle(position,label,property.boolValue);
		}
		if(property.propertyType == SerializedPropertyType.Integer)
		{
			property.intValue = EditorGUI.IntField(position,label,property.intValue);
		}
		if(property.propertyType == SerializedPropertyType.Float)
		{
			property.floatValue = EditorGUI.FloatField(position,label,property.floatValue);
		}
	}
}
