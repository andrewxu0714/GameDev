using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManagement : MonoBehaviour {
	public TerrainManagement.terrainType tileTerrainType;
	public TerrainManagement.featureType tileFeatureType;
	public TerrainManagement terrainManager;

	// Use this for initialization
	void Start () {
		if (tileTerrainType == TerrainManagement.terrainType.grass) {
			Instantiate (terrainManager.spriteGrassTile, this.transform);
		}
		if (tileFeatureType == TerrainManagement.featureType.obstruction) {
			Instantiate (terrainManager.spriteGrassObstruction, this.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
