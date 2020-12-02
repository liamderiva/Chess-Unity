using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece {

    public override bool[,] LegalMove()
    {
        bool[,] r = new bool[8, 8];

        ChessPiece c;   //checker for legal moves
        int i, j;

        //move up (all 3 possible)
        //start at top left 
        i = CurrentX - 1;
        j = CurrentY + 1;

        if(CurrentY != 7)   //boundary limit at top
        {
            for(int k = 0; k < 3; k++)  //for all 3 possible upward moves
            {
                if(i >= 0 || i < 8) //if within boundaries
                {
                    c = Manager.Instance.ChessPieces[i, j]; //check for target position
                    if (c == null)  //if there is no piece
                    {
                        r[i, j] = true; //move is legal
                    }
                    else if(isWhite != c.isWhite)   //if there is an enemy piece
                    {
                        r[i, j] = true; //move is legal
                    }
                    i++;    //loop from top left to middle to right
                }
            }
        }

        //move down (all 3 possible)
        //start at bottom left 
        i = CurrentX - 1;
        j = CurrentY - 1;

        if (CurrentY != 0)   //boundary limit at bottom
        {
            for (int k = 0; k < 3; k++)  //for all 3 possible upward moves
            {
                if (i >= 0 || i < 8) //if within boundaries
                {
                    c = Manager.Instance.ChessPieces[i, j]; //check for target position
                    if (c == null)  //if there is no piece
                    {
                        r[i, j] = true; //move is legal
                    }
                    else if (isWhite != c.isWhite)   //if there is an enemy piece
                    {
                        r[i, j] = true; //move is legal
                    }
                    i++;    //loop from top left to middle to right
                }
            }
        }

        //move left
        if(CurrentX != 0)   //boundary limit left side of board
        {
            c = Manager.Instance.ChessPieces[CurrentX - 1, CurrentY];   //check for target position
            if (c == null)  //if there is no piece
            {
                r[CurrentX - 1, CurrentY] = true;   //move is legal
            }
            else if (isWhite != c.isWhite)  //if there is an enemy piece
            {
                r[CurrentX - 1, CurrentY] = true;   //move is legal
            }
        }

        //move right
        if (CurrentX != 7)   //boundary limit left side of board
        {
            c = Manager.Instance.ChessPieces[CurrentX + 1, CurrentY];   //check for target position
            if (c == null)  //if there is no piece
            {
                r[CurrentX + 1, CurrentY] = true;   //move is legal
            }
            else if (isWhite != c.isWhite)  //if there is an enemy piece
            {
                r[CurrentX + 1, CurrentY] = true;   //move is legal
            }
        }

        return r;
    }

}
