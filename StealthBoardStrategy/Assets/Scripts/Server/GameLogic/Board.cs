using System;
using System.Collections.Generic;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;

namespace StealthBoardStrategy.Server.GameLogic {
    [Serializable]
    public class Board {
        // Player2のボード:右手系
        // Player1のボード:左手系
        public const int BOARDSIZE = 3;
        public GroundTile[] GroundTiles1;
        public GroundTile[] GroundTiles2;

        public Board(){
            GroundTiles1 = new GroundTile[BOARDSIZE*BOARDSIZE];
            GroundTiles2 = new GroundTile[BOARDSIZE*BOARDSIZE];
            for(int i = 0; i < BOARDSIZE; i++){
                GroundTiles1[i] = new GroundTile();
                GroundTiles2[i] = new GroundTile();
            }
        }

        public GroundTile GetGroundTile (Players player, int x, int y) {
            if (x >= 0 && x < BOARDSIZE && y >= 0 && y < BOARDSIZE) {
                return GetGroundTilesList (player) [x + BOARDSIZE * y];
            } else {
                throw new IndexOutOfRangeException ();
            }
        }
        private GroundTile[] GetGroundTilesList (Players player) {
            switch (player) {
                case Players.Player1:
                    return GroundTiles1;
                case Players.Player2:
                    return GroundTiles2;
                default:
                    throw new FormatException ();
            }
        }
    }
}