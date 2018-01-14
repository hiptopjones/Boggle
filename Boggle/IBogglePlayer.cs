using System;

namespace Boggle
{
    public interface IBogglePlayer
    {
        string Name { get; }
        Guid Id { get; }
        void Start();
        void Solve(BoggleBoard board);
    }
}