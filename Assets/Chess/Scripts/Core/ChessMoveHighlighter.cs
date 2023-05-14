using Chess.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMoveHighlighter : MonoBehaviour
{
    public enum PieceType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    [SerializeField] private PieceType _type;
    [SerializeField] private string _tag;


    private void OnMouseDown()
    {
        ChessBoardPlacementHandler.Instance.ClearHighlights();
        HighlightPossibleMoves();
    }

    private void HighlightPossibleMoves()
    {
        switch (_type)
        {
            case PieceType.King:
                HighlightKingMoves();
                break;
            case PieceType.Queen:
                HighlightQueenMoves();
                break;
            case PieceType.Bishop:
                HighlightBishopMoves();
                break;
            case PieceType.Knight:
                HighlightKnightMoves();
                break;
            case PieceType.Rook:
                HighlightRookMoves();
                break;
            case PieceType.Pawn:
                HighlightPawnMoves();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HighlightKingMoves()
    {
        // Getting the row and column values for the king's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        // Highlight the king's possible moves
        HighlightKingMove(row - 1, col - 1);
        HighlightKingMove(row - 1, col);
        HighlightKingMove(row - 1, col + 1);
        HighlightKingMove(row, col - 1);
        HighlightKingMove(row, col + 1);
        HighlightKingMove(row + 1, col - 1);
        HighlightKingMove(row + 1, col);
        HighlightKingMove(row + 1, col + 1);
    }


    private void HighlightKingMove(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightQueenMoves()
    {
        HighlightBishopMoves();
        HighlightRookMoves();
    }

    private void HighlightBishopMoves()
    {
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var bishopCol = chessPlayerPlacementHandler.column;
        var bishopRow = chessPlayerPlacementHandler.row;

        ChessBoardPlacementHandler.Instance.GetTile(bishopRow, bishopCol);

        var row = bishopRow;
        var col = bishopCol;

        HighlightMovesInDirection(row, col, -1, -1);
        HighlightMovesInDirection(row, col, -1, 1);
        HighlightMovesInDirection(row, col, 1, -1);
        HighlightMovesInDirection(row, col, 1, 1);
    }


    private void HighlightMovesInDirection(int row, int col, int rowDirection, int colDirection)
    {
        var curRow = row + rowDirection;
        var curCol = col + colDirection;
        while (IsValidTile(curRow, curCol))
        {
            if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(curRow, curCol, _tag))
            {
                return;
            }
            ChessBoardPlacementHandler.Instance.Highlight(curRow, curCol);
            curRow += rowDirection;
            curCol += colDirection;
        }
    }

    private void HighlightKnightMoves()
    {
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var knightCol = chessPlayerPlacementHandler.column;
        var knightRow = chessPlayerPlacementHandler.row; 

        ChessBoardPlacementHandler.Instance.GetTile(knightRow, knightCol);

        HighlightKnightMove(knightRow + 2, knightCol + 1);
        HighlightKnightMove(knightRow + 2, knightCol - 1);
        HighlightKnightMove(knightRow - 2, knightCol + 1);
        HighlightKnightMove(knightRow - 2, knightCol - 1);
        HighlightKnightMove(knightRow + 1, knightCol + 2);
        HighlightKnightMove(knightRow + 1, knightCol - 2);
        HighlightKnightMove(knightRow - 1, knightCol + 2);
        HighlightKnightMove(knightRow - 1, knightCol - 2);
    }

    private void HighlightKnightMove(int row, int col)
    {
        if (IsValidTile(row, col))
        {
            if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag))
            {
                ChessBoardPlacementHandler.Instance.Highlight(row, col);
            }
        }
    }

    private void HighlightRookMoves()
    {
        // Getting the row and column values for the rook's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        // Highlight the rook's possible moves
        HighlightMovesInDirection(row, col, -1, 0); // up
        HighlightMovesInDirection(row, col, 1, 0); // down
        HighlightMovesInDirection(row, col, 0, -1); // left
        HighlightMovesInDirection(row, col, 0, 1); // right
    }


    private void HighlightPawnMoves()
    {
        // Getting the row and column values for the pawn's tile using the ChessPlayerPlacementHandler script
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        if (_tag == "White")
        {
            HighlightWhitePawnMove(row - 1, col);
            HighlightWhitePawnAttackMove(row - 1, col - 1);
            HighlightWhitePawnAttackMove(row - 1, col + 1);
            if (row == 6) HighlightWhitePawnMove(row - 2, col);
        }
        else if (_tag == "Black")
        {
            HighlightBlackPawnMove(row + 1, col);
            HighlightBlackPawnAttackMove(row + 1, col - 1);
            HighlightBlackPawnAttackMove(row + 1, col + 1);
            if (row == 1) HighlightBlackPawnMove(row + 2, col);
        }
    }


    private void HighlightWhitePawnMove(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightBlackPawnMove(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightWhitePawnAttackMove(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightBlackPawnAttackMove(int row, int col)
    {
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private bool IsInvalidTile(int row, int col)
    {
        return !IsValidTile(row, col) || ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag);
    }

    private bool IsValidTile(int row, int col)
    {
        return row >= 0 && row <= 7 && col >= 0 && col <= 7;
    }

}


