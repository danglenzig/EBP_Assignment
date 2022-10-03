using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimeCelShaderGlassEditor : ShaderGUI
{

	bool showCustomInspector = true;

	//ColorFoldoutProperties
	bool colorFoldout = true;
	Color color;

	//Surface Foldout
	bool surfaceFoldout = true;
	float opacity;
	bool UVs;
	float linesOpacity;
	float linesDistance;
	float linesWidth;
	float scaleMultiplier;
	Vector3 camPosition;

	//
	Font customFont;
	Texture titleTex;
	Texture colorTex;
	Texture surfaceTex;
	GUIStyle foldoutTitleStyle;
	GUILayoutOption[] colorsLayout;
	bool cache;

	void GetCache()
	{
		if (cache == false)
		{
			cache = true;
			#region SetCustomAssets
			//CustomFont
			customFont = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Simple.ttf", typeof(Font)) as Font;
			//TitleImage
			titleTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/TitleGlass.png", typeof(Texture2D)) as Texture2D;
			//ColorsImage
			colorTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Colors.png", typeof(Texture2D)) as Texture2D;
			//FoamImage
			surfaceTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Surface.png", typeof(Texture2D)) as Texture2D;
			#endregion
			#region GUIStyles Templates
			//Foldouts
			foldoutTitleStyle = new GUIStyle("Foldout");
			foldoutTitleStyle.font = customFont;
			foldoutTitleStyle.fontSize = 20;
			foldoutTitleStyle.fixedHeight = 20;
			foldoutTitleStyle.margin.left = 15;
			//Option
			colorsLayout = new GUILayoutOption[4];
			colorsLayout[0] = GUILayout.MaxHeight(50);
			colorsLayout[1] = GUILayout.MinHeight(40);
			colorsLayout[2] = GUILayout.MinWidth(20);
			colorsLayout[3] = GUILayout.MaxWidth(500);

			#endregion
		}
	}

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		showCustomInspector = EditorGUILayout.Toggle("Use Custom Inspector", showCustomInspector);
		if (!showCustomInspector)
		{
			base.OnGUI(materialEditor, properties);
		}
		else
		{
			EditorGUI.BeginChangeCheck();
			Material targetMat = materialEditor.target as Material;
			Undo.RecordObject(targetMat, null);
			GetCache();
			#region TitleImage
			GUIContent title = new GUIContent(titleTex);

			GUILayoutOption[] titleOptions = new GUILayoutOption[4];//Change some options
			titleOptions[0] = GUILayout.MaxWidth(300);
			titleOptions[1] = GUILayout.MaxHeight(150);
			titleOptions[2] = GUILayout.MinWidth(200);
			titleOptions[3] = GUILayout.MinHeight(150);
			//Draw
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(title, titleOptions);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			#endregion
			#region Colors Foldout
			GUIContent colorFoldoutTitle = new GUIContent("Colors", colorTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			colorFoldout = EditorGUILayout.Foldout(colorFoldout, colorFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (colorFoldout)
			{
				GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				color = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_Color"), false, false, false, colorsLayout);
				GUILayout.EndHorizontal();
				GUILayout.Space(10);
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion
			#region Surface Foldout
			GUIContent surfaceFoldoutTitle = new GUIContent("Surface Options", surfaceTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			surfaceFoldout = EditorGUILayout.Foldout(surfaceFoldout, surfaceFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (surfaceFoldout)
			{
				bool ESB = true;
				if (targetMat.GetFloat("_UVs") == 0) { ESB = false; } else { ESB = true; }
				UVs = EditorGUILayout.Toggle("UVs", ESB);
				opacity = EditorGUILayout.Slider("Glass Opacity", targetMat.GetFloat("_Opacity"), 0, 1);
				linesOpacity = EditorGUILayout.Slider("Lines Opacity", targetMat.GetFloat("_LinesOpacity"), 0, 1);
				linesDistance = EditorGUILayout.FloatField("Lines Distance", targetMat.GetFloat("_LinesDistance"));
				linesWidth = EditorGUILayout.Slider("Lines Width", targetMat.GetFloat("_LinesWidth"),0,1);
				scaleMultiplier = EditorGUILayout.Slider("Scale Multiplier", targetMat.GetFloat("_ScaleMultiplier"),0,1);
				camPosition = EditorGUILayout.Vector3Field("Camera Position", targetMat.GetVector("_CamPosition"));
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion

			if (EditorGUI.EndChangeCheck() || (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
			{
				targetMat.SetColor("_Color", color);

				//Surface
				if (UVs) { targetMat.SetFloat("_UVs", 1); } else { targetMat.SetFloat("_UVs", 0); }
				targetMat.SetFloat("_Opacity", opacity);
				targetMat.SetFloat("_LinesOpacity", linesOpacity);
				targetMat.SetFloat("_LinesDistance", linesDistance);
				targetMat.SetFloat("_LinesWidth", linesWidth);
				targetMat.SetFloat("_ScaleMultiplier", scaleMultiplier);
				targetMat.SetVector("_CamPosition", camPosition);
			}
		}
	}
	string GetAssetPath()
	{
		string name = this.ToString();
		string[] temp = AssetDatabase.FindAssets(name, null);
		return AssetDatabase.GUIDToAssetPath(temp[0]).Remove(AssetDatabase.GUIDToAssetPath(temp[0]).Length - (name.Length + 3));
	}
}
