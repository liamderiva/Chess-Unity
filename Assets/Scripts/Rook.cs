using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece {

    public override bool[,] LegalMove()
    {
        bool[,] r = new bool[8, 8]; //array of true possible moves on 8x8 board

        ChessPiece c;   //checker for legal moves
        int i;  //for moves that can go a non fixed distance

        //
        //-----ALL WHILE LOOPS FOR NON FIXED DISTANCE MOVES
        //

        //move right
        i = CurrentX;   //take current x position
        while (true)
        {
            i++;    //increment x position 
            if(i >= 8)  //if out of bounds move (right side of board)
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, CurrentY];  //check for target position
            
            if(c == null)   //if there is no piece
            {
                r[i, CurrentY] = true;  //move is legal
            }
            else//if there is a piece
            {
                if(c.isWhite != isWhite)    //if there is an enemy piece
                {
                    r[i, CurrentY] = true;  //move is legal
                }

                break;  //stops at taken piece position
            }
        }

        //move left
        i = CurrentX;   //take current x position
        while (true)
        {
            i--;    //decerement x position
            if (i < 0)  //if out of bounds move (left side of board)
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, CurrentY];  //check for target position

            if (c == null)   //if there is no piece
            {
                r[i, CurrentY] = true;  //move is legal
            }
            else//if there is a piece
            {
                if (c.isWhite != isWhite)    //if there is an enemy piece
                {
                    r[i, CurrentY] = true;  //move is legal
                }

                break;  //stops at taken piece position
            }
        }

        //move up
        i = CurrentY;   //take current y position
        while (true)
        {
            i++;    //increment y position
            if (i >= 8) //if out of bounds move (top of board)
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[CurrentX, i];  //check for target position

            if (c == null)   //if there is no piece
            {
                r[CurrentX, i] = true;  //move is legal
            }
            else//if there is a piece
            {
                if (c.isWhite != isWhite)    //if there is an enemy piece
                {
                    r[CurrentX, i] = true;  //move is legal
                }

                break;  //stops at taken piece position
            }
        }

        //move down
        i = CurrentY;   //take current y position
        while (true)
        {
            i--;    //increment y position
            if (i < 0) //if out of bounds move (bottom of board)
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[CurrentX, i];  //check for target position

            if (c == null)   //if there is no piece
            {
                r[CurrentX, i] = true;  //move is legal
            }
            else//if there is a piece
            {
                if (c.isWhite != isWhite)    //if there is an enemy piece
                {
                    r[CurrentX, i] = true;  //move is legal
                }

                break;  //stops at taken piece position
            }
        }

        return r;
    }

}
