using System;
using System.Collections.Generic;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;

namespace StealthBoardStrategy.Server.GameLogic
{
    public class Board
    {
        public const int BOARDSIZE = 5;
        public GroundTile[] GroundTiles1;
        public GroundTile[] GroundTiles2;

        public GroundTile[] GetGroundTilesList(Players player){
            switch(player){
                case Players.Player1:
                    return GroundTiles1;
                case Players.Player2:
                    return GroundTiles2;
                default:
                    throw new FormatException();
            }
        }

        public GroundTile GetGroundTile(Players player, int x, int y){
            if(x >= 0 && x < BOARDSIZE && y>= 0 && y < BOARDSIZE){
                return GetGroundTilesList(player)[x + BOARDSIZE * y];
            } else{
                throw new IndexOutOfRangeException();
            }
        }
    }
}