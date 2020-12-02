using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece {

    public override bool[,] LegalMove()
    {
        bool[,] r = new bool[8, 8];

        ChessPiece c;// checker for legal moves
        int i, j;   //for non fixed distance

        //
        //-----ALL WHILE LOOPS FOR NON FIXED DISTANCE MOVES
        //

        //move up left
        i = CurrentX;   //current x position
        j = CurrentY;   //current y position
        while (true)
        {
            i--;    //move all the way
            j++;    //move all the way
            if(i < 0 || j >= 8) //check if out of bounds
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, j]; //check for target position
            if(c == null)   //if position is clear
            {
                r[i, j] = true; //move is legal
            }
            else
            {
                if(isWhite != c.isWhite)    //if there is an enemy piece
                {
                    r[i, j] = true; //move is legal
                }
                break;  //stops at taken piece position
            }
        }

        //move up right
        i = CurrentX;   //current x position
        j = CurrentY;   //current y position
        while (true)
        {
            i++;    //move all the way
            j++;    //move all the way
            if (i >= 8 || j >= 8) //check if out of bounds
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, j]; //check for target position
            if (c == null)   //if position is clear
            {
                r[i, j] = true; //move is legal
            }
            else
            {
                if (isWhite != c.isWhite)    //if there is an enemy piece
                {
                    r[i, j] = true; //move is legal
                }
                break;  //stops at taken piece position
            }
        }

        //move down left
        i = CurrentX;   //current x position
        j = CurrentY;   //current y position
        while (true)
        {
            i--;    //move all the way
            j--;    //move all the way
            if (i < 0 || j < 0) //check if out of bounds
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, j]; //check for target position
            if (c == null)   //if position is clear
            {
                r[i, j] = true; //move is legal
            }
            else
            {
                if (isWhite != c.isWhite)    //if there is an enemy piece
                {
                    r[i, j] = true; //move is legal
                }
                break;  //stops at taken piece position
            }
        }

        //move down right
        i = CurrentX;   //current x position
        j = CurrentY;   //current y position
        while (true)
        {
            i++;    //move all the way
            j--;    //move all the way
            if (i >= 8 || j < 0) //check if out of bounds
            {
                break;  //stop at edge
            }

            c = Manager.Instance.ChessPieces[i, j]; //check for target position
            if (c == null)   //if position is clear
            {
                r[i, j] = true; //move is legal
            }
            else
            {
                if (isWhite != c.isWhite)    //if there is an enemy piece
                {
                    r[i, j] = true; //move is legal
                }
                break;  //stops at taken piece position
            }
        }

        return r;
    }

}
