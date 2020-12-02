using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour {  //base class for all chess pieces

    //all pieces will inherit these properties

    public int CurrentX { set; get; }   //current x of piece
    public int CurrentY { set; get; }   //current y of piece
    public bool isWhite;    //determines colour of chesspiece as a boolean
    public int CurrentWeight = 0;  //value of piece at current position

    public void SetPosition(int x, int y)   //sets the properties CurrentX/Y to ints x/y
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] LegalMove()  //check for legal moves
    {
        return new bool[8, 8];  //return an array of size 8,8 
    }

    public void SetWeight(int x)
    {
        CurrentWeight = x;
    }

    public bool[,] GetEatMoves()    //moves that can be made to take an enemy piece
    {
        bool[,] r = new bool[8, 8];

        r = CheckMoves(LegalMove(), EatMoves());    //checks legal moves against eat moves 

        return r;   //returns array of true/false where true is a takeable piece
    }

    public bool[,] EatMoves()   //returns an array of all pieces that can be taken by the ai
    {
        bool[,] r = new bool[8, 8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //AllowedMoves = ChessPieces[j, i].LegalMove();
                if (AiTakeCheck(i, j) != null)
                {
                    r[i, j] = AiTakeCheck(i, j);
                    //Debug.Log(EatMoves[j, i]);
                }
            }
        }
        return r;
    }

    public ChessPiece AiTakeCheck(int x, int y) //checks for white pieces on board
    {
        ChessPiece c = Manager.Instance.ChessPieces[x, y];   //new chesspiece at current position

        if (c != null && c.isWhite)  //check for white piece
        {
            return c; //return white piece
        }
        else
        {
            return null;
        }

    }

    public virtual bool[,] CheckMoves(bool[,] a, bool[,] b)  //(basically an "and" for just true values
    {
        bool[,] r = new bool[8, 8];

        for (int i = 0; i < a.GetLength(0); i++)
        {
            for(int j = 0; j < a.GetLength(1); j++)
            {
                if(a[i, j] && b[i, j])
                {
                    r[i, j] = true;
                }
            }
        }
        return r;
    }

    private ChessPiece Target(bool[,] a, bool[,] b)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                if (a[i, j] && b[i, j])
                {
                    ChessPiece c = Manager.Instance.ChessPieces[i, j];
                    return c;
                }
            }
        }
        return null;
    }

}
