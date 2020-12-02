using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece {

    //
    //-----QUEEN MOVEMENT USES BOTH ROOK AND BISHOP MOVEMENT 
    //

    public override bool[,] LegalMove()
    {
        bool[,] r = new bool[8, 8]; //array of true possible moves on 8x8 board

        ChessPiece c;   //checker for legal moves
        int i, j;  //for moves that can go a non fixed distance

        //
        //-----ALL WHILE LOOPS FOR NON FIXED DISTANCE MOVES
        //

        //
        //-----ROOK-LIKE MOVEMENT (UP DOWN LEFT RIGHT)
        //

        //move right
        i = CurrentX;   //take current x position
        while (true)
        {
            i++;    //increment x position 
            if (i >= 8)  //if out of bounds move (right side of board)
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

        //
        //-----BISHOP-LIKE MOVEMENT (DIAGONALS)
        //

        //move up left
        i = CurrentX;   //current x position
        j = CurrentY;   //current y position
        while (true)
        {
            i--;    //move all the way
            j++;    //move all the way
            if (i < 0 || j >= 8) //check if out of bounds
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
