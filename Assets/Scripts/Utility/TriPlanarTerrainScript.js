var bumpTextures : Texture2D[] = new Texture2D[8];
var specTextures : Texture2D[] = new Texture2D[8];
var tilesPerMeter : float[] = new float[8];
var terDat : TerrainData;

private var blankNormal : Texture2D;
private var blankSpec : Texture2D;

function Start () {
	setTerrainValues ();
}

function setTerrainValues () {

	if ( blankNormal == null ) {
		blankNormal = generateBlankNormal ();
	}
	
	if ( blankSpec == null ) {
		blankSpec = generateBlankSpec ();
	}

	if ( ( GetComponent ( Terrain ) != null ) && ( terDat == null ) ) {
		terDat = GetComponent ( Terrain ).terrainData;
	}
	
	if ( terDat != null ) {
		var splatLength : int = terDat.splatPrototypes.length;
		for ( var i : int = 0; i < splatLength; i++ ) {
			if ( bumpTextures[i] != null ) {
				Shader.SetGlobalTexture ( "_BumpMap" + i, bumpTextures[i] );
			}
			else {
				Shader.SetGlobalTexture ( "_BumpMap" + i, blankNormal );
			}
			
			if ( specTextures[i] != null ) {
				Shader.SetGlobalTexture ( "_SpecMap" + i, specTextures[i] );
			}
			else {
				Shader.SetGlobalTexture ( "_SpecMap" + i, blankSpec );
			}
			
			if ( tilesPerMeter[i] != null ) {
				Shader.SetGlobalFloat ( "_TerrainTexScale" + i, ( 1.0 / tilesPerMeter[i] ) );
			}
			else {
				Shader.SetGlobalFloat ( "_TerrainTexScale" + i, 1.0 );
			}
		}
		while ( i < 8 ) {
			bumpTextures[i] = null;
			specTextures[i] = null;
			tilesPerMeter[i] = 1.0;
			i++;
		}
	}
}

function generateBlankNormal ():Texture2D {
	var texture = new Texture2D ( 16, 16, TextureFormat.ARGB32, false );
	var cols = texture.GetPixels32 ( 0 );
	var colsLength = cols.Length;
	for( var i : int = 0; i < colsLength; i++ ) {
		cols[i] = Color ( 0, 0.5, 0, 0.5 );
	}
	texture.SetPixels32 ( cols, 0 );
	texture.Apply ( false );
	texture.Compress ( false );
	return texture;
}

function generateBlankSpec ():Texture2D {
	var texture = new Texture2D ( 16, 16, TextureFormat.RGB24, false );
	var cols = texture.GetPixels ( 0 );
	var colsLength = cols.Length;
	for( var i : int = 0; i < colsLength; i++ ) {
		cols[i] = Color ( 0.1, 0.1, 0, 0 );
	}
	texture.SetPixels ( cols, 0 );
	texture.Apply ( false );
	texture.Compress ( false );
	return texture;
}