using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimeCelShaderEditorTransparent : ShaderGUI
{

	bool showCustomInspector = true;

	float opacity;

	//ColorFoldoutProperties
	bool colorFoldout = true;
	Color tint;
	Color shadowColor;

	//Surface Foldout
	bool surfaceFoldout = true;
	bool environmentShadows;
	float shadowRange;
	float shadowSharpness;
	float shadowIntensity;
	bool dontUseSpecularTexture;
	float specularBaseValue;
	float specularRange;
	float specularIntensity;
	float pointLightIntensity;


	//Outline Foldout
	bool outlineFoldout = true;
	Color outlineColor;
	bool texturedOutline;
	bool vCOutline;
	float outlineWidth;
	float outlineWidthMultiplier;

	//Emisive Foldout
	bool emisiveFoldout = true;
	Color emisiveColor;

	//Fresnel foldout
	bool fresnelFoldout = true;
	bool fresnel;
	Color fresnelColor;
	float fresnelExponent;
	float fresnelBias;


	//Textures foldout
	bool texturesFoldout = true;
	float textureLightness;
	Texture2D mainTexture;
	Vector2 mainTextureTiling;
	Vector2 mainTextureOffset;
	Texture2D specularTexture;
	Vector2 specularTextureTiling;
	Vector2 specularTextureOffset;
	Texture2D outlineColorTexture;
	Vector2 outlineColorTextureTiling;
	Vector2 outlineColorTextureOffset;
	Texture2D emisiveTexture;
	Vector2 emisiveTextureTiling;
	Vector2 emisiveTextureOffset;

	//
	Font customFont;
	Texture titleTex;
	Texture colorTex;
	Texture surfaceTex;
	Texture outlineTex;
	Texture texturesTex;
	GUIStyle foldoutTitleStyle;
	GUILayoutOption[] colorsLayout;
	GUILayoutOption[] texturesLayout;
	GUIStyle titleColorStyle;
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
			titleTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/TitleDiffuse.png", typeof(Texture2D)) as Texture2D;
			//ColorsImage
			colorTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Colors.png", typeof(Texture2D)) as Texture2D;
			//FoamImage
			surfaceTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Surface.png", typeof(Texture2D)) as Texture2D;
			//WavesImage
			outlineTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Outline.png", typeof(Texture2D)) as Texture2D;
			//TexturesImage
			texturesTex = AssetDatabase.LoadAssetAtPath(GetAssetPath() + "Interface/Textures.png", typeof(Texture2D)) as Texture2D;
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
			//Texture layout
			texturesLayout = new GUILayoutOption[4];
			texturesLayout[0] = GUILayout.MaxHeight(65);
			texturesLayout[1] = GUILayout.MinHeight(50);
			texturesLayout[2] = GUILayout.MinWidth(50);
			texturesLayout[3] = GUILayout.MaxWidth(65);
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
			opacity = EditorGUILayout.Slider("Opacity Cutoff", targetMat.GetFloat("_Cutoff"), 0, 1);
			#region Colors Foldout
			GUIContent colorFoldoutTitle = new GUIContent("Colors", colorTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			colorFoldout = EditorGUILayout.Foldout(colorFoldout, colorFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (colorFoldout)
			{
				GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				tint = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_MainTexTint"), false, false, false, colorsLayout);
				shadowColor = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_ShadowColor"), false, false, false, colorsLayout);
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
				if (targetMat.GetFloat("_EnvironmentShadows") == 0) { ESB = false; } else { ESB = true; }
				environmentShadows = EditorGUILayout.Toggle("Environment Shadows", ESB);
				shadowRange = EditorGUILayout.Slider("Shadow Range", targetMat.GetFloat("_ShadowRange"), 0, 1);
				shadowSharpness = EditorGUILayout.Slider("Shadow Sharpness", targetMat.GetFloat("_ShadowSharpness"), 0, 1);
				shadowIntensity = EditorGUILayout.Slider("Shadow Intensity", targetMat.GetFloat("_ShadowIntensity"), 0, 1);
				bool SVB = true;
				if (targetMat.GetFloat("_DontUseSpecularTexture") == 0) { SVB = false; } else { SVB = true; }
				dontUseSpecularTexture = EditorGUILayout.Toggle("Dont Use Specular/Normal Texture", SVB);
				if (dontUseSpecularTexture)
				{
					specularBaseValue = EditorGUILayout.Slider("Specular Base Value", targetMat.GetFloat("_SpecularBaseValue"), 0, 1);
				}
				specularRange = EditorGUILayout.Slider("Specular Range", targetMat.GetFloat("_SpecularRange"),0.9f,1);
				specularIntensity = EditorGUILayout.Slider("Specular Intensity", targetMat.GetFloat("_SpecularMult"),0,1);
				pointLightIntensity = EditorGUILayout.Slider("Point Light Intensity", targetMat.GetFloat("_PointLightIntensity"), 0, 1);
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion
			#region Outline
			GUIContent outlineFoldoutTitle = new GUIContent("Outline Options", outlineTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			outlineFoldout = EditorGUILayout.Foldout(outlineFoldout, outlineFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (outlineFoldout)
			{
				GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				outlineColor = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_OutlineColorTint"), false, false, false, colorsLayout);
				GUILayout.EndHorizontal();
				GUILayout.Space(10);
				EditorGUILayout.HelpBox("Outline color acts as a tint when Textured Outline is ON", MessageType.Info);
				bool OTB = true;
				if (targetMat.GetFloat("_UseTexturedOutline") == 0) { OTB = false; } else { OTB = true; }
				texturedOutline = EditorGUILayout.Toggle("Textured Outline", OTB);
				EditorGUILayout.HelpBox("In order to use VC Outline properly, you will need to calculate the outline values with the VCCalculator script on scripts folder", MessageType.Info);
				bool VCOB = true;
				if (targetMat.GetFloat("_VCOutline") == 0) { VCOB = false; } else { VCOB = true; }
				vCOutline = EditorGUILayout.Toggle("VC Outline",VCOB);
				outlineWidth = EditorGUILayout.FloatField("Outline Width", targetMat.GetFloat("_OutlineWidth"));
				outlineWidthMultiplier = EditorGUILayout.FloatField("Outline Width Multiplier", targetMat.GetFloat("_OutlineWidthMultiplier"));
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion
			#region Emisive
			GUIContent emisiveFoldoutTitle = new GUIContent("Emisive Options", outlineTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			emisiveFoldout = EditorGUILayout.Foldout(emisiveFoldout, emisiveFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (emisiveFoldout)
			{
				GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				emisiveColor = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_EmisiveColor"), false, false, true, colorsLayout);
				GUILayout.EndHorizontal();
				EditorGUILayout.HelpBox("To deactivate Emisive, just remove the emisive texture", MessageType.Info);
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion
			#region Fresnel
			GUIContent fresnelFoldoutTitle = new GUIContent("Fresnel Options", outlineTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			fresnelFoldout = EditorGUILayout.Foldout(fresnelFoldout, fresnelFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (fresnelFoldout)
			{
				GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				fresnelColor = EditorGUILayout.ColorField(new GUIContent(""), targetMat.GetColor("_FresnelColor"), false, false, false, colorsLayout);
				GUILayout.EndHorizontal();
				bool FB = true;
				if (targetMat.GetFloat("_Fresnel") == 0) { FB = false; } else { FB = true; }
				fresnel = EditorGUILayout.Toggle("Fresnel", FB);
				fresnelExponent = EditorGUILayout.FloatField("Fresnel Exponent", targetMat.GetFloat("_FresnelExponent"));
				fresnelBias = EditorGUILayout.FloatField("Fresnel Bias", targetMat.GetFloat("_FresnelBias"));
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion
			#region Textures
			GUIContent texturesFoldoutTitle = new GUIContent("Textures", texturesTex);
			GUILayout.BeginHorizontal("box");
			GUILayout.BeginVertical();
			texturesFoldout = EditorGUILayout.Foldout(texturesFoldout, texturesFoldoutTitle, true, foldoutTitleStyle);
			GUILayout.Space(10);

			if (texturesFoldout)
			{
				EditorGUILayout.LabelField("Main Texture");
				textureLightness = EditorGUILayout.FloatField("Texture Lightness", targetMat.GetFloat("_TextureLightness"));
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Space(15f);
				mainTextureTiling = EditorGUILayout.Vector2Field("Tiling", targetMat.GetTextureScale("_MainTex"));
				mainTextureOffset = EditorGUILayout.Vector2Field("Offset", targetMat.GetTextureOffset("_MainTex"));
				EditorGUILayout.EndVertical();
				mainTexture = EditorGUILayout.ObjectField(targetMat.GetTexture("_MainTex"), typeof(Texture2D), false, texturesLayout) as Texture2D;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.LabelField("Specular/Normal Texture");
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Space(15f);
				specularTextureTiling = EditorGUILayout.Vector2Field("Tiling", targetMat.GetTextureScale("_SpecularTexture"));
				specularTextureOffset = EditorGUILayout.Vector2Field("Offset", targetMat.GetTextureOffset("_SpecularTexture"));
				EditorGUILayout.EndVertical();
				specularTexture = EditorGUILayout.ObjectField(targetMat.GetTexture("_SpecularTexture"), typeof(Texture2D), false, texturesLayout) as Texture2D;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.LabelField("Outline Texture");
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Space(15f);
				outlineColorTextureTiling = EditorGUILayout.Vector2Field("Tiling", targetMat.GetTextureScale("_OutlineColor"));
				outlineColorTextureOffset = EditorGUILayout.Vector2Field("Offset", targetMat.GetTextureOffset("_OutlineColor"));
				EditorGUILayout.EndVertical();
				outlineColorTexture = EditorGUILayout.ObjectField(targetMat.GetTexture("_OutlineColor"), typeof(Texture2D), false, texturesLayout) as Texture2D;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.LabelField("Emisive Texture");
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUILayout.Space(15f);
				emisiveTextureTiling = EditorGUILayout.Vector2Field("Tiling", targetMat.GetTextureScale("_EmisiveTexture"));
				emisiveTextureOffset = EditorGUILayout.Vector2Field("Offset", targetMat.GetTextureOffset("_EmisiveTexture"));
				EditorGUILayout.EndVertical();
				emisiveTexture = EditorGUILayout.ObjectField(targetMat.GetTexture("_EmisiveTexture"), typeof(Texture2D), false, texturesLayout) as Texture2D;
				EditorGUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			#endregion

			if (EditorGUI.EndChangeCheck() || (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
			{
				targetMat.SetFloat("_Cutoff", opacity);
				targetMat.SetColor("_MainTexTint", tint);
				targetMat.SetColor("_ShadowColor", shadowColor);

				//Surface
				if (environmentShadows) { targetMat.EnableKeyword("_ENVIRONMENTSHADOWS_ON"); targetMat.SetFloat("_EnvironmentShadows", 1); } else { targetMat.DisableKeyword("_ENVIRONMENTSHADOWS_ON"); targetMat.SetFloat("_EnvironmentShadows", 0); }
				targetMat.SetFloat("_ShadowRange", shadowRange);
				targetMat.SetFloat("_ShadowSharpness", shadowSharpness);
				targetMat.SetFloat("_ShadowIntensity", shadowIntensity);
				if (dontUseSpecularTexture) { targetMat.EnableKeyword("_DONTUSESPECULARTEXTURE_ON"); targetMat.SetFloat("_DontUseSpecularTexture", 1); } else { targetMat.DisableKeyword("_DONTUSESPECULARTEXTURE_ON"); targetMat.SetFloat("_DontUseSpecularTexture", 0); }
				targetMat.SetFloat("_SpecularBaseValue", specularBaseValue);
				targetMat.SetFloat("_SpecularRange", specularRange);
				targetMat.SetFloat("_SpecularMult", specularIntensity);
				targetMat.SetFloat("_PointLightIntensity", pointLightIntensity);

				//Outline
				targetMat.SetColor("_OutlineColorTint", outlineColor);
				if (texturedOutline) { targetMat.SetFloat("_UseTexturedOutline", 1); } else { targetMat.SetFloat("_UseTexturedOutline", 0); }
				if (vCOutline) { targetMat.SetFloat("_VCOutline", 1); } else { targetMat.SetFloat("_VCOutline", 0); }
				targetMat.SetFloat("_OutlineWidth", outlineWidth);
				targetMat.SetFloat("_OutlineWidthMultiplier", outlineWidthMultiplier);

				//Emisive
				targetMat.SetColor("_EmisiveColor", emisiveColor);

				//Fresnel
				if (fresnel) { targetMat.SetFloat("_Fresnel", 1); } else { targetMat.SetFloat("_Fresnel", 0); }
				targetMat.SetColor("_FresnelColor", fresnelColor);
				targetMat.SetFloat("_FresnelExponent", fresnelExponent);
				targetMat.SetFloat("_FresnelBias", fresnelBias);



				//Textures
				targetMat.SetFloat("_TextureLightness", textureLightness);
				targetMat.SetTexture("_MainTex", mainTexture);
				targetMat.SetTextureScale("_MainTex", mainTextureTiling);
				targetMat.SetTextureOffset("_MainTex", mainTextureOffset);

				targetMat.SetTexture("_SpecularTexture", specularTexture);
				targetMat.SetTextureScale("_SpecularTexture", specularTextureTiling);
				targetMat.SetTextureOffset("_SpecularTexture", specularTextureOffset);

				targetMat.SetTexture("_OutlineColor", outlineColorTexture);
				targetMat.SetTextureScale("_OutlineColor", outlineColorTextureTiling);
				targetMat.SetTextureOffset("_OutlineColor", outlineColorTextureOffset);

				targetMat.SetTexture("_EmisiveTexture", emisiveTexture);
				targetMat.SetTextureScale("_EmisiveTexture", emisiveTextureTiling);
				targetMat.SetTextureOffset("_EmisiveTexture", emisiveTextureOffset);
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
