using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Chess960GameTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.Chess960Game();

        Assert.AreEqual(true, intFace.chess960);
    }

    [Test]
    public void StandardGameTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.chess960 = true;
        intFace.StandardGame();

        Assert.AreEqual(false, intFace.chess960);
    }

    [Test]
    public void Chess960BoardSetupTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.Chess960Game();

        Assert.AreEqual(0, Chess.InitialSpawn(intFace.chess960));
    }

    [Test]
    public void StandardBoardSetupTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.StandardGame();

        Assert.AreEqual(1, Chess.InitialSpawn(intFace.chess960));
    }

    [Test]
    public void Chess960BishopOppositeColorsTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.Chess960Game();
        Chess.PiecesWhite.Clear();
        Chess.PiecesBlack.Clear();
        Chess.InitialSpawn(intFace.chess960);
        var bishops = Chess.PiecesWhite.FindAll(x => x.name.Contains("bishopW"));

        var pos1 = bishops[0].transform.position.x;
        var pos2 = bishops[1].transform.position.x;

        bool oppositeColors = (pos1 % 2 != pos2 % 2);

        Assert.AreEqual(true, oppositeColors);
    }

    [Test]
    public void StandardBoardSetupBishopTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.StandardGame();
        Chess.PiecesWhite.Clear();
        Chess.PiecesBlack.Clear();
        Chess.InitialSpawn(intFace.chess960);
        var bishopsW = Chess.PiecesWhite.FindAll(x => x.name.Contains("bishopW"));
        var bishopsB = Chess.PiecesBlack.FindAll(x => x.name.Contains("bishopB"));

        var pos1 = bishopsW[0].transform.position.x;
        var pos2 = bishopsW[1].transform.position.x;

        var pos3 = bishopsB[0].transform.position.x;
        var pos4 = bishopsB[1].transform.position.x;

        bool correctSpots = (pos1 == pos3 && pos2 == pos4);

        Assert.AreEqual(true, correctSpots);
    }

    [Test]
    public void Chess960BoardSetupKingRookTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.Chess960Game();
        Chess.PiecesWhite.Clear();
        Chess.PiecesBlack.Clear();
        Chess.InitialSpawn(intFace.chess960);
        var rooks = Chess.PiecesWhite.FindAll(x => x.name.Contains("rookW"));
        var king = Chess.PiecesWhite.Find(x => x.name.Contains("kingW"));

        var pos1 = rooks[0].transform.position.x;
        var pos2 = rooks[1].transform.position.x;
        var pos3 = king.transform.position.x;

        bool kingBetween = (pos1 < pos3 && pos3 < pos2);

        Assert.AreEqual(true, kingBetween);
    }

    [Test]
    public void Chess960BoardSetupBlackWhiteIdenticalTest()
    {
        GameObject game = new GameObject();
        Interface intFace = game.AddComponent<Interface>();

        intFace.Chess960Game();
        Chess.PiecesWhite.Clear();
        Chess.PiecesBlack.Clear();
        Chess.InitialSpawn(intFace.chess960);

        var bishopsW = Chess.PiecesWhite.FindAll(x => x.name.Contains("bishopW"));
        var bishopsB = Chess.PiecesBlack.FindAll(x => x.name.Contains("bishopB"));

        var queenW = Chess.PiecesWhite.Find(x => x.name.Contains("queenW"));
        var queenB = Chess.PiecesBlack.Find(x => x.name.Contains("queenB"));

        var knightsW = Chess.PiecesWhite.FindAll(x => x.name.Contains("knightW"));
        var knightsB = Chess.PiecesBlack.FindAll(x => x.name.Contains("knightB"));

        var rooksW = Chess.PiecesWhite.FindAll(x => x.name.Contains("rookW"));
        var rooksB = Chess.PiecesBlack.FindAll(x => x.name.Contains("rookB"));

        var kingW = Chess.PiecesWhite.Find(x => x.name.Contains("kingW"));
        var kingB = Chess.PiecesBlack.Find(x => x.name.Contains("kingB"));

        var w1 = bishopsW[0].transform.position.x;
        var w2 = bishopsW[1].transform.position.x;
        var w3 = queenW.transform.position.x;
        var w4 = knightsW[0].transform.position.x;
        var w5 = knightsW[1].transform.position.x;
        var w6 = rooksW[0].transform.position.x;
        var w7 = rooksW[1].transform.position.x;
        var w8 = kingW.transform.position.x;

        var b1 = bishopsB[0].transform.position.x;
        var b2 = bishopsB[1].transform.position.x;
        var b3 = queenB.transform.position.x;
        var b4 = knightsB[0].transform.position.x;
        var b5 = knightsB[1].transform.position.x;
        var b6 = rooksB[0].transform.position.x;
        var b7 = rooksB[1].transform.position.x;
        var b8 = kingB.transform.position.x;

        bool blackWhiteIdentical = (w1 == b1 && w2 == b2 && w3 == b3 && w4 == b4 && w5 == b5 && w6 == b6 && w7 == b7 && w8 == b8);

        Assert.AreEqual(true, blackWhiteIdentical);
    }

    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator UnitTestsWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
