using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManagement : MonoBehaviour {

	public GameObject wall;
	public enum terrainType {grass, rock};
	public enum featureType {plains, mountain, obstruction};
	public GameObject terrainTile;
	public terrainType[,] terrainMap;
	public featureType[,] featureMap;
	public GameObject spriteGrassTile;
	public GameObject spriteGrassObstruction;

	// Use this for initialization
	void Start () {
		terrainMap = terrainGeneration (32, 32);
		featureMap = featureGeneration (32, 32);
		spawnTerrain (terrainMap, featureMap);
		spawnWalls ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private terrainType[,] terrainGeneration(int width, int length) {
		terrainType[,] tempMap = new terrainType[length, width];
		for (int i = 0; i<tempMap.GetLength(0); i++) {
			for (int j = 0; j < tempMap.GetLength (1); j++) {
				tempMap[i,j] = terrainType.grass;
			}
		}
		return tempMap;
	}

	private featureType[,] featureGeneration(int width, int length) {
		featureType[,] tempMap = new featureType[length,width];
		for (int i = 0; i<tempMap.GetLength(0); i++) {
			for (int j = 0; j < tempMap.GetLength (1); j++) {
				if (Random.value > 0.95) {
					tempMap [i, j] = featureType.obstruction;
				} else {
					tempMap [i, j] = featureType.plains;
				}
			}
		}
		return tempMap;
	}

	private void spawnTerrain(terrainType[,] terrainInfo, featureType[,] featureInfo) {
		for (int i=0; i<terrainMap.GetLength(0); i++) {
			for (int j=0; j<terrainMap.GetLength(1); j++) {
				GameObject temp = Instantiate (terrainTile,new Vector3(i,0,j),Quaternion.identity);
				temp.GetComponent<TileManagement> ().tileTerrainType = terrainInfo [i, j];
				temp.GetComponent<TileManagement> ().tileFeatureType = featureInfo [i, j];
				temp.GetComponent<TileManagement> ().terrainManager = this;
			}
		}
	}

	private void spawnWalls() {
		Instantiate (wall, new Vector3 (15.5f, 0.5f, -1f), Quaternion.identity);
		Instantiate (wall, new Vector3 (15.5f, 0.5f, 32f), Quaternion.identity);
		Instantiate (wall, new Vector3 (-1f, 0.5f, 15.5f), Quaternion.AngleAxis (90f, Vector3.down));
		Instantiate (wall, new Vector3 (32f, 0.5f, 15.5f), Quaternion.AngleAxis (90f, Vector3.down));
	}
}
