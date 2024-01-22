using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


namespace WorldGeneration
{
    namespace Element2WorldGeneration
    {
        public class Element2WorldGenTilePlacer : WorldGenTilePlacer
        {
            private Tilemap outputTilemap;



            public override void Init ( WorldGenFloor floor ) {
                
            }
            

            
            public override void PlaceFloorGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;
            }

            public override void PlaceFloorWallTiles ( Vector3Int[ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;
            }
        


            public override void PlaceRoomGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;
            }

            public override void PlaceRoomWallTiles ( Vector3Int[ ] points, Tilemap outputTilemap ) {
                foreach ( Vector3Int point in points ) 
                    outputTilemap.SetTile ( point, DefaultTile );
            }
        


            
            public override void PlaceCorridorGroundTiles ( Vector3Int [ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;
            }

            public override void PlaceCorridorWallTiles ( Vector3Int [ ] points, Tilemap outputTilemap ) {
                this.outputTilemap = outputTilemap;
            }
        }
    }
}

