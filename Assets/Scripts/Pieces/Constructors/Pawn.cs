﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the constants and methods necessary to allow the movement of the pawns.
/// </summary>
public class Pawn
{
    /// <summary>
    /// The position of the piece.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// Indicates if the part has been moved for the first time.
    /// </summary>
    readonly bool firstMove;
    /// <summary>
    /// The color of the piece.
    /// </summary>
    readonly Pieces.Colour colour;

    public Pawn(Vector2 position, bool firstMove, Pieces.Colour colour)
    {
        this.position = position;
        this.firstMove = firstMove;
        this.colour = colour;
    }

    /// <summary>
    /// List of movements that the piece can perform before eliminating the blocked movements.
    /// </summary>
    public List<Vector2> MovePositions
    {
        get
        {
            if (colour == Pieces.Colour.White)
            {
                if (position.y == 8)
                {
                    return new List<Vector2>();
                }

                // If white plays and the white king is in check, we eliminate the positions that cannot avoid checkmate.

                if (Chess.WhiteKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsWhite();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyBlackMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // If this is not the case, we return the original movement list.

                return GetMovePositionsWhite();
            }

            else
            {
                if (position.y == 1)
                {
                    return new List<Vector2>();
                }

                // If black plays and the black king is in check, we eliminate the positions that cannot avoid checkmate.

                if (Chess.BlackKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsBlack();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyWhiteMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // If this is not the case, we return the original movement list.

                return GetMovePositionsBlack();
            }
        }
    }

    /// <summary>
    /// List of positions where the piece can capture another.
    /// </summary>
    public List<Vector2> PositionsInCheck => (colour == Pieces.Colour.White) ? GetPositionsInCheckWhite() : GetPositionsInCheckBlack();

    /// <summary>
    /// List of positions where the piece is a threat to the opposing king.
    /// </summary>
    public List<Vector2> MenacingPositions => (colour == Pieces.Colour.White) ? GetMenacingPositionsWhite() : GetMenacingPositionsBlack();

    /// <summary>
    /// The value of the piece at the current position.
    /// </summary>
    public int PositionValue => (colour == Pieces.Colour.White) ? GetPositionValueWhite() : GetPositionValueBlack();

    /// <summary>
    /// List of movements that a white piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {        
        List<Vector2> tempList = new List<Vector2>();

        if (Chess.CheckSquareEmpty(new Vector2(position.x, position.y + 1)))
        {
            tempList.Add(new Vector2(position.x, position.y + 1));

            if (!firstMove && Chess.CheckSquareEmpty(new Vector2(position.x, position.y + 2)))
            {
                tempList.Add(new Vector2(position.x, position.y + 2));
            }
        }

        if (Chess.CheckSquareBlack(new Vector2(position.x - 1, position.y + 1)))
        {
            tempList.Add(new Vector2(position.x - 1, position.y + 1));
        }

        if (Chess.CheckSquareBlack(new Vector2(position.x + 1, position.y + 1)))
        {
            tempList.Add(new Vector2(position.x + 1, position.y + 1));
        }

        if (Chess.WhiteKingInCheck)
        {
            return tempList;
        }

        if (firstMove && Chess.EnPassantActive)
        {
            if (Chess.EnPassantPosition == new Vector2(position.x - 1, position.y + 1))
            {
                tempList.Add(new Vector2(position.x - 1, position.y + 1));
            }

            else if (Chess.EnPassantPosition == new Vector2(position.x + 1, position.y + 1))
            {
                tempList.Add(new Vector2(position.x + 1, position.y + 1));
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of movements that a black piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsBlack()
    {
        List<Vector2> tempList = new List<Vector2>();

        if (Chess.CheckSquareEmpty(new Vector2(position.x, position.y - 1)))
        {
            tempList.Add(new Vector2(position.x, position.y - 1));

            if (!firstMove && Chess.CheckSquareEmpty(new Vector2(position.x, position.y - 2)))
            {
                tempList.Add(new Vector2(position.x, position.y - 2));
            }
        }

        if (Chess.CheckSquareWhite(new Vector2(position.x - 1, position.y - 1)))
        {
            tempList.Add(new Vector2(position.x - 1, position.y - 1));
        }

        if (Chess.CheckSquareWhite(new Vector2(position.x + 1, position.y - 1)))
        {
            tempList.Add(new Vector2(position.x + 1, position.y - 1));
        }

        if (Chess.BlackKingInCheck)
        {
            return tempList;
        }

        if (firstMove && Chess.EnPassantActive)
        {
            if (Chess.EnPassantPosition == new Vector2(position.x - 1, position.y - 1))
            {
                tempList.Add(new Vector2(position.x - 1, position.y - 1));
            }

            else if (Chess.EnPassantPosition == new Vector2(position.x + 1, position.y - 1))
            {
                tempList.Add(new Vector2(position.x + 1, position.y - 1));
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the white piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x - 1, position.y + 1),
            new Vector2(position.x + 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8)
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the black piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x + 1, position.y - 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8)
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the white piece is a threat to the black player.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsWhite()
    {
        if (Chess.BlackKingPosition == new Vector2(position.x - 1, position.y + 1))
        {
            return new List<Vector2> { position };
        }

        else if (Chess.BlackKingPosition == new Vector2(position.x + 1, position.y + 1))
        {
            return new List<Vector2> { position };
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// List of positions where the black piece is a threat to the white player.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsBlack()
    {
        if (Chess.WhiteKingPosition == new Vector2(position.x - 1, position.y - 1))
        {
            return new List<Vector2> { position };
        }

        else if (Chess.WhiteKingPosition == new Vector2(position.x + 1, position.y - 1))
        {
            return new List<Vector2> { position };
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// The value of the white piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhite()
    {
        int[,] whiteValue =
        {
            { 00, 00, 00, 00, 00, 00, 00, 00 },
            { -05, -10, -10, 20, 20, -10, -10, -05 },
            { -05, 05, 10, 00, 00, 10, 05, -05 },
            { 00, 00, 00, -20, -20, 00, 00, 00 },
            { -05, -05, -10, -25, -25, -10, -05, -05 },
            { -10, -10, -20, -30, -30, -20, -10, -10 },
            { -50, -50, -50, -50, -50, -50, -50, -50 },
            { -90, -90, -90, -90, -90, -90, -90, -90 }
        };

        return -10 + whiteValue[(int)position.y -1, (int)position.x -1];
    }

    /// <summary>
    /// The value of the black piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { 90, 90, 90, 90, 90, 90, 90, 90 },
            { 50, 50, 50, 50, 50, 50, 50, 50 },
            { 10, 10, 20, 30, 30, 20, 10, 10 },
            { 05, 05, 10, 25, 25, 10, 05, 05 },
            { 00, 00, 00, 20, 20, 00, 00, 00 },
            { 05, -05, -10, 00, 00, -10, -05, 05 },
            { 05, 10, 10, -20, -20, 10, 10, 05 },
            { 00, 00, 00, 00, 00, 00, 00, 00 }
        };

        return 10 + blackValue[(int)position.y -1, (int)position.x -1];
    }
}