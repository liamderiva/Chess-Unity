using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece {

    public override bool[,] LegalMove()
    {
        bool[,] r = new bool[8,8];  //array of true possible moves on 8x8 board

        //move up left
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);

        //move up right
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);

        //move down left
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);

        //move down right
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);

        //move right up
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        //move right down
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        //move left up
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        //move up left
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        ChessPiece c;   //checker for legal moves

        if(x >= 0 && x < 8 && y >= 0 && y < 8)  //check if x and y are within board array
        {
            c = Manager.Instance.ChessPieces[x, y]; //check for target position
            if(c == null)   //if position is clear
            {
                r[x, y] = true; //move is legal
            }else if(isWhite != c.isWhite)  //check if enemy piece
            {
                r[x, y] = true; //move is legal
            }
        }
    }

}
