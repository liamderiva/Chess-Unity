using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager Instance { set; get; } //new manager instance (accessible from anywhere)
    private bool[,] AllowedMoves { set; get; }  //array of moves to check if a move is legal or not
    private bool[,] PossibleMoves { set; get; }
    public ChessPiece[,] ChessPieces { set; get; }  //chess piece array (stores all pieces in game) 
                                                    //and holds position on board

    private ChessPiece SelectedChessPiece;  //if a chess piece is selected

    private const float TILE_SIZE = 1.0f;   //size of each tile on board
    private const float TILE_OFFSET = 0.5f; //width and height of each tile

    private int SelectionX = -1;    //initial selection value for x position on board
    private int SelectionZ = -1;    //initial selection value for z position on board

    public List<GameObject> chessPiecePrefabs;  //list for prefabs of chess pieces
    private List<GameObject> activeChessPiece; //list for active chess pieces

    private Material previousMaterial;  //material for previously selected piece
    public Material selectedMaterial;   //material for currently selected piece

    //unused EnPassent tile storage
    public int[] EnPassent { set; get; }

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);   //orientation for black pieces

    public bool whiteTurn = true;   //white starts and also to restrict choice of pieces on each turn

    public Dictionary<ChessPiece, bool[,]> bestMoves = new Dictionary<ChessPiece, bool[,]>();

    //---------------------------------
    //------initialisation of game-----
    //---------------------------------
    void Start()
    {
        Instance = this;
        Debug.Log("Started");   //debug.log test
        SpawnAllChessPieces();  //fill board
    }

    //-----general update of the whole scene
    private void Update()
    {
        UpdateSelection();
        DrawBoard();

        if (Input.GetMouseButtonDown(0) && whiteTurn)    //check for left mouse click
        {
            if (SelectionX >= 0 && SelectionZ >= 0)   //if clicking on the board
            {
                if (SelectedChessPiece == null)  //have not selected a chess piece yet
                {
                    //select
                    SelectChessPiece(SelectionX, SelectionZ);
                }
                else   //if we have selected a chess piece
                {
                    //move
                    MoveChessPiece(SelectionX, SelectionZ);
                }
            }
        }

        if (!whiteTurn)    //Computer's turn to move
        {

            CalculateBestMoveMultiDepth(2);

            if (bestMoves.Count != 0)
            {

                int randomMove = Random.Range(0, bestMoves.Count);

                ChessPiece chosenPiece = null;
                //bool[,] possibleMoves = new bool[8, 8];

                chosenPiece = bestMoves.ElementAt(randomMove).Key;   //piece chosen to move
                PossibleMoves = bestMoves.ElementAt(randomMove).Value;    //possible moves for chosen piece

                //
                //to determine a move to make
                //
                List<int> moveX = new List<int>();
                List<int> moveY = new List<int>();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (PossibleMoves[i, j])
                        {
                            moveX.Add(i);
                            moveY.Add(j);
                        }
                    }
                }

                int randomCoord = Random.Range(0, moveX.Count);
                int randomMoveX = moveX.ElementAt(randomCoord);
                int randomMoveY = moveY.ElementAt(randomCoord);
                //
                //--------------------------------------
                //
               
                Debug.Log(randomMoveX + " " + randomMoveY);

                if (SelectedChessPiece == null)  //have not selected a chess piece yet
                {
                    //select
                    AiSelect(chosenPiece.CurrentX, chosenPiece.CurrentY);
                    AiMove(randomMoveX, randomMoveY);
                }
                bestMoves.Clear();

            }
        }
    }

    //-----checks if a piece is selected or not
    private void SelectChessPiece(int x, int y)
    {
        if (ChessPieces[x, y] == null)    //check if a piece is selected
        {
            return; //do nothing
        }

        if (ChessPieces[x, y].isWhite != whiteTurn)   //check if valid piece has been selected on each turn(white or black)
        {
            return; //do nothing
        }

        bool canMove = false;   //boolean to check if a piece can move or not
        AllowedMoves = ChessPieces[x, y].LegalMove();   //moves that can be done

        //-----loop to check if a piece has any valid moves 
        //-----if not, then piece is not selected when clicked on
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (AllowedMoves[i, j])
                {
                    canMove = true;
                }
            }
        }

        if (!canMove)
        {
            return;
        }

        SelectedChessPiece = ChessPieces[x, y];     //valid piece selected and registered

        previousMaterial = SelectedChessPiece.GetComponent<MeshRenderer>().material;    //take default material of selected piece
        selectedMaterial.mainTexture = previousMaterial.mainTexture;    //assign previous material to selected piece
        SelectedChessPiece.GetComponent<MeshRenderer>().material = selectedMaterial;    //apply that material to the piece
        BoardHighlights.Instance.HighlightAllowedMoves(AllowedMoves);
    }

    //-----checks if the move is allowed and moves the piece if it is
    private void MoveChessPiece(int x, int y)
    {
        if (AllowedMoves[x, y]) //determine if move is legal
        {

            ChessPiece c = ChessPieces[x, y];   //new chesspiece at target position

            if (c != null && c.isWhite != whiteTurn) //if there is an enemy piece  it is not current players turn
            {
                //take the piece

                //if King is taken
                if (c.GetType() == typeof(King))
                {
                    EndGame();  //reset board
                }
                activeChessPiece.Remove(c.gameObject);  //removes active chess piece
                Destroy(c.gameObject);  //destroys chess piece object
            }
            if (SelectedChessPiece.GetType() == typeof(Pawn))   //check if pawn
            {     
                if(y == 7)
                {
                    activeChessPiece.Remove(SelectedChessPiece.gameObject);
                    Destroy(SelectedChessPiece.gameObject);
                    SpawnChessPieceWhite(1, x, y, -90);
                    SelectedChessPiece = ChessPieces[x, y];
                }
            }
            //
            //ENPASSENT MOVE NOT WORKING------
            //
            /*
            if(x == EnPassent[0] && y == EnPassent[1])  //for taking in enPassent move
            {
                if(whiteTurn)  //white team taking black piece
                {
                    c = ChessPieces[x, y - 1];  //for piece behind white piece
                    activeChessPiece.Remove(c.gameObject);  //removes active chess piece
                    Destroy(c.gameObject);  //destroys chess piece object
                }
                else//if black team taking white piece
                {
                    c = ChessPieces[x, y + 1];  //for piece behind black piece
                    activeChessPiece.Remove(c.gameObject);  //removes active chess piece
                    Destroy(c.gameObject);  //destroys chess piece object
                }
            }
            EnPassent[0] = -1;  //initialize enPassent moves
            EnPassent[1] = -1;
            if (SelectedChessPiece.GetType() == typeof(Pawn))    //check if pawn 
            {
                if(y == 7)
                {
                    activeChessPiece.Remove(SelectedChessPiece.gameObject);  //removes active chess piece
                    Destroy(SelectedChessPiece.gameObject);  //destroys chess piece object
                    SpawnChessPieceWhite(1, x, y, 10);
                    SelectedChessPiece = ChessPieces[x, y];
                }
                else if (y == 0)
                {
                    activeChessPiece.Remove(SelectedChessPiece.gameObject);  //removes active chess piece
                    Destroy(SelectedChessPiece.gameObject);  //destroys chess piece object
                    SpawnChessPieceWhite(7, x, y, 10);
                    SelectedChessPiece = ChessPieces[x, y];
                }


                if (SelectedChessPiece.CurrentY == 1 && y == 3)  //if in enPassent position(for white player)
                {
                    EnPassent[0] = x;   //sets first index to x
                    EnPassent[1] = y - 1;   //sets second index to y before the piece to be taken
                }
                else if (SelectedChessPiece.CurrentY == 6 && y == 4)  //if in enPassent position(for black player)
                {
                    EnPassent[0] = x;   //sets first index to x
                    EnPassent[1] = y + 1;   //sets second index to y before the piece to be taken
                }
            }*/

            ChessPieces[SelectedChessPiece.CurrentX, SelectedChessPiece.CurrentY] = null;   //sets current position to null so 
                                                                                            //that piece can be moved
            SelectedChessPiece.transform.position = GetTileCentre(x, y);    //moves the piece
            SelectedChessPiece.SetPosition(x, y);   //sets the current position of piece in memory
            ChessPieces[x, y] = SelectedChessPiece;     //re-places piece into array
            whiteTurn = !whiteTurn;     //each time a piece is moved, turn changes
        }

        SelectedChessPiece.GetComponent<MeshRenderer>().material = previousMaterial;    //resets piece to default colour
        BoardHighlights.Instance.HideHighlights();
        SelectedChessPiece = null;  //if move is not legal, no piece is selected
    }

    //-----------------------------------------------
    //-----updates mouse coordinates (Selection)-----
    //-----------------------------------------------
    private void UpdateSelection()
    {

        if (!Camera.main)   //check if the main camera is in use
        {
            return;
        }

        RaycastHit hit; //variable to detect mouse collision
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //variable to receive object collision via ray
        if (Physics.Raycast(ray, out hit, 25.0f, LayerMask.GetMask("ChessPlane")))  //check for collision between mouse and collison plane and output the hit coordinates
        {
            //Debug.Log(hit.point);   //test for position of mouse on board
            SelectionX = (int)hit.point.x;  //assign current mouse x position 
            SelectionZ = (int)hit.point.z;  //assign current mouse z position
        }
        else
        {
            SelectionX = -1;    //if mouse is not on the board reset to -1 (out of bounds)
            SelectionZ = -1;    //same as above
        }

    }

    //-----instantiation of white chess pieces on board
    private void SpawnChessPieceWhite(int index, int x, int y, int weight)
    {   //go is new piece           //determines type of chess piece, gets centre of tile
        GameObject go = Instantiate(chessPiecePrefabs[index], GetTileCentre(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        ChessPieces[x, y] = go.GetComponent<ChessPiece>();  //gets ChessPiece component from newly instantiated object
        ChessPieces[x, y].SetPosition(x, y);    //sets current position of each piece in ChessPiece array (board rep)
        ChessPieces[x, y].SetWeight(weight);
        activeChessPiece.Add(go);   //adds piece to list of active pieces
    }

    //-----instantiation of black chess pieces on board
    private void SpawnChessPieceBlack(int index, int x, int y, int weight)
    {                                                                          //flips black pieces to face correctly
        GameObject go = Instantiate(chessPiecePrefabs[index], GetTileCentre(x, y), orientation) as GameObject;
        go.transform.SetParent(transform);
        ChessPieces[x, y] = go.GetComponent<ChessPiece>();
        ChessPieces[x, y].SetPosition(x, y);
        ChessPieces[x, y].SetWeight(weight);
        activeChessPiece.Add(go);
    }

    //-----instantiation of all chess pieces on the board
    private void SpawnAllChessPieces()
    {
        activeChessPiece = new List<GameObject>();  //new list for chess pieces
        ChessPieces = new ChessPiece[8, 8]; //initialises ChessPiece array (board rep)
        EnPassent = new int[2] { -1, -1 }; //declare array of enPassent move

        //---instantiating white pieces
        SpawnChessPieceWhite(0, 4, 0, -1000);//king
        SpawnChessPieceWhite(1, 3, 0, -90);//queen    
        SpawnChessPieceWhite(2, 0, 0, -50);//rook1
        SpawnChessPieceWhite(2, 7, 0, -50);//rook2
        SpawnChessPieceWhite(3, 1, 0, -30);//knight1
        SpawnChessPieceWhite(3, 6, 0, -30);//knight2
        SpawnChessPieceWhite(4, 2, 0, -30);//bishop1
        SpawnChessPieceWhite(4, 5, 0, -30);//bishop2

        for (int i = 0; i < 8; i++)  //loop to spawn all pawns
        {
            SpawnChessPieceWhite(5, i, 1, -10);
        }

        //---instantiating black pieces
        SpawnChessPieceBlack(6, 4, 7, 1000);//king
        SpawnChessPieceBlack(7, 3, 7, 90);//queen    
        SpawnChessPieceBlack(8, 0, 7, 50);//rook1
        SpawnChessPieceBlack(8, 7, 7, 50);//rook2
        SpawnChessPieceBlack(9, 1, 7, 30);//knight1
        SpawnChessPieceBlack(9, 6, 7, 30);//knight2
        SpawnChessPieceBlack(10, 2, 7, 30);//bishop1
        SpawnChessPieceBlack(10, 5, 7, 30);//bishop2

        for (int i = 0; i < 8; i++)  //loop to spawn all pawns
        {
            SpawnChessPieceBlack(11, i, 6, 10);
        }
    }

    //-------------------------------------------------------------
    //-----gets centre of tile for pieces to be insantiated on-----
    //-------------------------------------------------------------
    private Vector3 GetTileCentre(int x, int z)
    {
        Vector3 origin = Vector3.zero;  //holds value of centre of each tile
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;  //determines centre for x axis
        origin.z += (TILE_SIZE * z) + TILE_OFFSET;  //determines centre for z axis
        return origin;  //return vector for centre of tile
    }

    //---------------------------------------
    //-----draws chess board in 3d plane-----
    //---------------------------------------   
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * 8;  //sets width vector of board
        Vector3 heightLine = Vector3.forward * 8;   //sets height vector of board

        for (int i = 0; i < 9; i++)  //loop to instatiate board on x axis of visual plane
        {
            Vector3 start = Vector3.forward * i;    //start position of line being drawn
            Debug.DrawLine(start, start + widthLine);   //line drawing from start position to next square
            for (int j = 0; j < 9; j++)  //loop to instatiate board on z axis of visual plane
            {
                start = Vector3.right * j;  //same as above for z axis
                Debug.DrawLine(start, start + heightLine);  //same as above for z axis
            }
        }

        //draw on the selection to indicate where the mouse is 
        /*if(SelectionX >= 0 && SelectionZ >= 0)
        {
            //horizontal lines to form a cross
            Debug.DrawLine(
                Vector3.forward * SelectionZ + Vector3.right * SelectionX,
                Vector3.forward * (SelectionZ + 1) + Vector3.right * (SelectionX + 1));

            Debug.DrawLine(
                Vector3.forward * (SelectionZ + 1) + Vector3.right * SelectionX,
                Vector3.forward * SelectionZ + Vector3.right * (SelectionX + 1));
        }*/
    }

    //-----temporary win/reset board function
    private void EndGame()
    {
        if (whiteTurn)
        {
            Debug.Log("white wins");
        }
        else
        {
            Debug.Log("black wins");
        }

        foreach (GameObject go in activeChessPiece)
        {
            Destroy(go);
        }
        whiteTurn = false;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessPieces();
    }

    //-----checks if a piece is selected or not
    private void AiSelect(int x, int y)
    {
        if (ChessPieces[x, y] == null)    //check if a piece is selected
        {
            return; //do nothing
        }

        if (ChessPieces[x, y].isWhite != whiteTurn)   //check if valid piece has been selected on each turn(white or black)
        {
            return; //do nothing
        }

        bool canMove = false;   //boolean to check if a piece can move or not
        AllowedMoves = ChessPieces[x, y].CheckMoves(ChessPieces[x, y].LegalMove(), PossibleMoves); //moves that can be done

        //-----loop to check if a piece has any valid moves 
        //-----if not, then piece is not selected when clicked on
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (AllowedMoves[i, j])
                {
                    canMove = true;
                }
            }
        }

        if (!canMove)
        {
            return;
        }

        SelectedChessPiece = ChessPieces[x, y];     //valid piece selected and registered

        previousMaterial = SelectedChessPiece.GetComponent<MeshRenderer>().material;    //take default material of selected piece
        selectedMaterial.mainTexture = previousMaterial.mainTexture;    //assign previous material to selected piece
        SelectedChessPiece.GetComponent<MeshRenderer>().material = selectedMaterial;    //apply that material to the piece
        BoardHighlights.Instance.HighlightAllowedMoves(AllowedMoves);   //highlights allowed moves when selected
    }

    public static void Print2DArray(bool[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Debug.Log(matrix[i, j] + " x:" + i + " y:" + j + "\t");
            }
        }
    }

    public static bool CompareArrays(bool[,] a, bool[,] b)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                if (a[i, j] && b[i, j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private ChessPiece Target(bool[,] a, bool[,] b)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                if (a[i, j] && b[i, j])
                {
                    ChessPiece c = ChessPieces[i, j];
                    return c;
                }
            }
        } return null;
    }

    //-----checks if the move is allowed and moves the piece if it is
    private void AiMove(int x, int y)
    {
        if (AllowedMoves[x, y]) //determine if move is legal
        {

            ChessPiece c = ChessPieces[x, y];   //new chesspiece at target position

            if (c != null && c.isWhite != whiteTurn) //if there is an enemy piece  it is not current players turn
            {
                //take the piece

                //if King is taken
                if (c.GetType() == typeof(King))
                {
                    EndGame();  //reset board
                }
                activeChessPiece.Remove(c.gameObject);  //removes active chess piece
                Destroy(c.gameObject);  //kill(me) chess piece object
            }
            if (SelectedChessPiece.GetType() == typeof(Pawn))   //check if pawn
            {
                if (y == 0)
                {
                    activeChessPiece.Remove(SelectedChessPiece.gameObject);
                    Destroy(SelectedChessPiece.gameObject);
                    SpawnChessPieceWhite(7, x, y, 90);
                    SelectedChessPiece = ChessPieces[x, y];
                }
            }

            ChessPieces[SelectedChessPiece.CurrentX, SelectedChessPiece.CurrentY] = null;   //sets current position to null so 
                                                                                            //that piece can be moved
            SelectedChessPiece.transform.position = GetTileCentre(x, y);    //moves the piece
            SelectedChessPiece.SetPosition(x, y);   //sets the current position of piece in memory
            ChessPieces[x, y] = SelectedChessPiece;     //re-places piece into array
            whiteTurn = !whiteTurn;     //each time a piece is moved, turn changes
        }

        SelectedChessPiece.GetComponent<MeshRenderer>().material = previousMaterial;    //resets piece to default colour
        BoardHighlights.Instance.HideHighlights();
        SelectedChessPiece = null;  //if move is not legal, no piece is selected
    }

    private void MakeMove(int x, int y, ChessPiece c)
    {

        if(ChessPieces[x, y] != null)
        {
            ChessPieces[x, y] = null;
        }
        ChessPieces[x, y] = c;
        ChessPieces[c.CurrentX, c.CurrentY] = null;

    }

    private void UnMakeMove(int x, int y, int backX, int backY, ChessPiece c, ChessPiece t)
    {

        ChessPieces[x, y] = null;
        ChessPieces[backX, backY] = c;

        if (t != null) //if there was a taken piece
        {
            ChessPieces[x, y] = t; //reinstance taken piece at its original position
        }

    }

    private int EvaluateBoard()
    {
        int boardWeight = 0;

        foreach(ChessPiece c in ChessPieces)
        {
            if(c != null)
            {
                boardWeight += c.CurrentWeight;
            }
        }
        return boardWeight;
    }

    //
    //MULTI DEPTH SEARCH 
    //

    private int CalculateBestMoveMultiDepth(int depth, int alpha = int.MinValue, int beta = int.MaxValue, bool isMaximisingPlayer = true, bool isBlackTurn = true, int count = 0)
    {
        int moveValue;

        if(depth == 0)  //evaluates at final possible move
        {
            moveValue = EvaluateBoard();//!isBlackTurn);
            Debug.Log(moveValue);
            return moveValue;
        }

        int bestMoveValue;

        if (isMaximisingPlayer)
        {
            bestMoveValue = -10000;
        }
        else
        {
            bestMoveValue = 10000;
        }

        foreach (ChessPiece c in ChessPieces)    //checking each chesspiece on the board
        {

            if (c != null  && !c.isWhite == isBlackTurn) //only if there is a piece and it is black
            {
                int backX = c.CurrentX; //coords for unmake move
                int backY = c.CurrentY;
                bool[,] possibleMoves = c.LegalMove();  //get the current chesspiece's legal moves 

                bool[,] bestCoords = new bool[8, 8];

                //loop through allowed moves and only making moves with the legal moves
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (possibleMoves[i, j])
                        {
                            ChessPiece TakenPiece = null;
                            if (ChessPieces[i, j] != null)
                            {
                                TakenPiece = ChessPieces[i, j];  //initialise taken piece object
                            }
                            
                            //Debug.Log(i + " " + j);
                            MakeMove(i, j, c);  //make move

                            moveValue = CalculateBestMoveMultiDepth(depth - 1, alpha, beta, !isMaximisingPlayer, !isBlackTurn, count + 1);

                            //Debug.Log(moveValue);

                            if (isMaximisingPlayer) //max
                            {
                                if (moveValue > bestMoveValue)  //if move made is worth more than current best move
                                {
                                    bestMoveValue = moveValue;  //assign new best move value
                                    if(count == 0)
                                    {
                                        bestMoves.Clear();  //clear dictionary in case "weaker" moves have been added
                                        bestCoords[i, j] = true;    //set coords of best move
                                        bestMoves.Add(c, bestCoords);   //add piece and move to dictionary
                                    }
                                    
                                }
                                if (moveValue == bestMoveValue)  //if move made is of equal value add to dictionary
                                {
                                    bestMoveValue = moveValue;
                                    if(count == 0)
                                    {
                                        bestCoords[i, j] = true;
                                        bestMoves[c] = bestCoords;  //reassign best coords with an added new best move
                                    }
                                    
                                }
                                alpha = Mathf.Max(alpha, moveValue);    //alpha check value
                            }
                            else if(!isMaximisingPlayer)//(!isMaximisingPlayer) "min"
                            {
                                if (moveValue < bestMoveValue)  //if move made is worth more than current best move
                                {
                                    bestMoveValue = moveValue;  //assign new best move value
                                }
                                if(moveValue == bestMoveValue)
                                {
                                    bestMoveValue = moveValue;
                                }
                                beta = Mathf.Min(beta, moveValue);
                            }

                            UnMakeMove(i, j, backX, backY, c, TakenPiece);  //unmake move

                            if(beta <= alpha)   //check for alpha beta pruning
                            {
                                Debug.Log("prune: " + alpha + " " + beta);
                                //prune
                                break;  //down
                            }
                        }
                    }
                }
            }
        }
        Debug.Log(bestMoveValue);
        return bestMoveValue;
    }



}