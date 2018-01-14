using System;

namespace Boggle
{
    internal interface IBoggleServer
    {
        BoggleResult Play(IBogglePlayer player, int randomSeed = 0, int boardSize = BoggleRules.BoardSizeDefault);
        int Score(Guid playerId, Guid boardId, string word);
    }
}