@CustomEditor (TriPlanarTerrainScript)

class TriPlanarTerrainScriptEditor extends Editor {

	var terScript : TriPlanarTerrainScript;
	var terrain : Terrain;
	var terDat : TerrainData;
	var showTextures : boolean[] = new boolean[8];
	var showHelp : boolean = false;
	
	override function OnInspectorGUI () {

		terScript = target;
		
		if ( terrain == null ) {
			EditorGUI.indentLevel = 0;
			GUILayout.Label ( "Attach this script to a terrain." );
			terrain = terScript.GetComponent ( Terrain );
		}
		
		if ( ( terrain != null ) && ( terDat == null ) ) {
			EditorGUI.indentLevel = 0;
			GUILayout.Label ( "Attach terrain data to this terrain." );
			terDat = terrain.terrainData;
		}
		
		if ( terDat != null ) {
			EditorGUI.indentLevel = 0;
			showHelp = EditorGUILayout.Foldout(showHelp, "Help" );
			if ( showHelp ) {
				EditorGUI.indentLevel = 1;
				GUILayout.Label (
				"  Diffuse textures:\n   These are assigned in the regular Terrain script.\n   They are shown here only for reference.\n\n  Specular/gloss textures:\n   Specular value is taken from the red channel.\n   Gloss value is taken from the green channel.\n   The blue channel is unused."
				);
			}
			for ( var i : int = 0; i < terDat.splatPrototypes.length; i++ ) {
				EditorGUI.indentLevel = 0;
				showTextures[i] = EditorGUILayout.Foldout(showTextures[i], "Layer " + i + " (" + terDat.splatPrototypes[i].texture.name + ")" );
				if ( showTextures[i] ) {
					EditorGUI.indentLevel = 1;
					terScript.tilesPerMeter[i] = EditorGUILayout.Slider ( "Tiling Amount", terScript.tilesPerMeter[i], 0.1, 100.0 );
					EditorGUILayout.ObjectField ( "Diffuse", terDat.splatPrototypes[i].texture, Texture, false);
					terScript.bumpTextures[i] = EditorGUILayout.ObjectField ( "Normal", terScript.bumpTextures[i], Texture, false);
					terScript.specTextures[i] = EditorGUILayout.ObjectField ( "Spec / Gloss", terScript.specTextures[i], Texture, false);
					EditorGUILayout.Space ();
				}
			}
		}
		
		terScript.setTerrainValues ();
	}
}