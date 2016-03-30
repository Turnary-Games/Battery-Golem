using UnityEditor;

public class CustomImportSettings : AssetPostprocessor {
	public void OnPreprocessModel() {
		ModelImporter modelImporter = (ModelImporter)assetImporter;
		modelImporter.globalScale = 100;
		modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
	}
}