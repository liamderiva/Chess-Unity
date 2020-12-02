using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece {

    public override bool[,] LegalMove() //override legal move for pawns
    {
        bool[,] r = new bool[8, 8]; //array of true possible moves on 8x8 board

        ChessPiece c1, c2;  //checkers for legal moves

        int[] e = Manager.Instance.EnPassent;   //temp array for enPassent move

        //-----white moves
        if (isWhite)
        {
            //forward
            if(CurrentY != 7)   //if not at end of board
            {
                c1 = Manager.Instance.ChessPieces[CurrentX, CurrentY + 1];  //check position ahead

                if(c1 == null)  //check if position ahead is clear or not
                {
                    r[CurrentX, CurrentY + 1] = true;   //legal move added to r array
                }
            }

            //forward on first move
            if(CurrentY == 1)   //if at starting position
            {
                c1 = Manager.Instance.ChessPieces[CurrentX, CurrentY + 1];  //check 1st space ahead
                c2 = Manager.Instance.ChessPieces[CurrentX, CurrentY + 2];  //check 2nd space ahead

                if(c1 == null && c2 == null)    //check if both spaces ahead are clear
                {
                    r[CurrentX, CurrentY + 2] = true;   //legal move added to r array
                }
            }

            //take left
            if(CurrentX != 0 && CurrentY != 7)  //check if not on far left of board or end of board
            {
                
                if(e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;   //legal move added to r array                
                }

                c1 = Manager.Instance.ChessPieces[CurrentX - 1, CurrentY + 1];  //check piece in left diagonal position
                if(c1 != null && !c1.isWhite)   //only takes if there is an enemy piece(black)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;   //legal move added to r array
                }
            }

            //take right
            if (CurrentX != 7 && CurrentY != 7)  //check if not on far left of board or end of board
            {
                
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;   //legal move added to r array                
                }

                c1 = Manager.Instance.ChessPieces[CurrentX + 1, CurrentY + 1];  //check piece in right diagonal position
                if (c1 != null && !c1.isWhite)   //only takes if there is an enemy piece(black)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;   //legal move added to r array 
                }
            }
        }
        else//-----for black pieces
        {
            //forward
            if (CurrentY != 0)   //if not at bottom of board
            {
                c1 = Manager.Instance.ChessPieces[CurrentX, CurrentY - 1];  //check position ahead

                if (c1 == null)  //check if position ahead is clear or not
                {
                    r[CurrentX, CurrentY - 1] = true;   //legal move added to r array
                }
            }

            //forward on first move
            if (CurrentY == 6)   //if at starting position
            {
                c1 = Manager.Instance.ChessPieces[CurrentX, CurrentY - 1];  //check 1st space ahead
                c2 = Manager.Instance.ChessPieces[CurrentX, CurrentY - 2];  //check 2nd space ahead

                if (c1 == null && c2 == null)    //check if both spaces ahead are clear
                {
                    r[CurrentX, CurrentY - 2] = true;   //legal move added to r array
                }
            }

            //take left
            if (CurrentX != 0 && CurrentY != 0)  //check if not on far left of board or bottom of board
            {

                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;   //legal move added to r array                
                }

                c1 = Manager.Instance.ChessPieces[CurrentX - 1, CurrentY - 1];  //check piece in left diagonal position
                if (c1 != null && c1.isWhite)   //only takes if there is an enemy piece(white)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;   //legal move added to r array
                }
            }

            //take right
            if (CurrentX != 7 && CurrentY != 0)  //check if not on far left of board or bottom of board
            {

                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;   //legal move added to r array                
                }

                c1 = Manager.Instance.ChessPieces[CurrentX + 1, CurrentY - 1];  //check piece in right diagonal position
                if (c1 != null && c1.isWhite)   //only takes if there is an enemy piece(white)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;   //legal move added to r array 
                }
            }
        }

        return r;
    }

}
