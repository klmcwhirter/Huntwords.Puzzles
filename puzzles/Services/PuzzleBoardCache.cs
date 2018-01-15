using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using puzzles.Models;

namespace puzzles.Services
{
    public struct PuzzleBoardDequeuedArgs
    {
        public PuzzleBoard PuzzleBoard { get; }

        public PuzzleBoardDequeuedArgs(PuzzleBoard board)
        {
            PuzzleBoard = board;
        }
    }

    public class PuzzleBoardCache : ConcurrentDictionary<int, ConcurrentQueue<PuzzleBoard>>
    {
        public event EventHandler<PuzzleBoardDequeuedArgs> PuzzleBoardDequeued;
        public event EventHandler<int> PuzzleBoardQueueEmpty;

        protected ILogger<PuzzleBoardCache> Logger { get; }
        protected PuzzleBoardGeneratorOptions Options { get; }

        public PuzzleBoardCache(
            IOptions<PuzzleBoardGeneratorOptions> options,
            ILogger<PuzzleBoardCache> logger
        )
        {
            Options = options.Value;
            Logger = logger;
        }

        public bool CacheFull(int id)
        {
            var rc = false;

            if (this.ContainsKey(id) && this[id].Count > 0)
            {
                rc = this[id].Count >= Options.CacheSize;
            }

            return rc;
        }

        public void Enqueue(PuzzleBoard board)
        {
            var id = board.Puzzle.Id;

            if (!this.ContainsKey(id))
            {
                this[id] = new ConcurrentQueue<PuzzleBoard>();
            }

            if (this[id].Count < Options.CacheSize)
            {
                this[id].Enqueue(board);
            }
        }

        public PuzzleBoard Dequeue(int id)
        {
            PuzzleBoard board = null;
            if (this.ContainsKey(id) && this[id].Count > 0)
            {
                if (!this[id].TryDequeue(out board))
                {
                    board = null;
                }
                else
                {
                    Task.Run(() => PuzzleBoardDequeued?.Invoke(this, new PuzzleBoardDequeuedArgs(board)));
                }
            }

            if (board == null)
            {
                PuzzleBoardQueueEmpty?.Invoke(this, id);
                Task.Delay(10000).ContinueWith((t) =>
                {
                    if (this.ContainsKey(id) && this[id].Count > 0)
                    {
                        this[id].TryDequeue(out board);
                    }

                    if (board == null)
                    {
                        Logger.LogInformation($"Board for Puzzle {id} did not appear in time.");
                    }
                });
            }

            return board;
        }
    }
}