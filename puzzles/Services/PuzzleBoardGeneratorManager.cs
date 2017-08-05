using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using puzzles.Models;
using puzzles.Repositories;

namespace puzzles.Services
{
    public class PuzzleBoardGeneratorManager
    {
        protected PuzzleBoardCache Cache { get; }
        protected IGenerator<PuzzleBoard> Generator { get; }
        protected ILogger<PuzzleBoardGeneratorManager> Logger { get; set; }
        protected IPuzzlesRepository PuzzleRepository { get; }

        public PuzzleBoardGeneratorManager(
            PuzzleBoardCache cache,
            IGenerator<PuzzleBoard> generator,
            IPuzzlesRepository puzzleRepository,
            ILogger<PuzzleBoardGeneratorManager> logger
        )
        {
            Cache = cache;
            Generator = generator;
            PuzzleRepository = puzzleRepository;
            Logger = logger;

            Cache.PuzzleBoardQueueEmpty += (sender, id) => FillQueuesPriority(id, false);
            Cache.PuzzleBoardDequeued += (sender, args) => FillQueues(false);
        }

        protected bool isFilling = false;
        protected object isFillingLock = new object();
        protected bool IsFilling
        {
            get
            {
                lock (isFillingLock)
                {
                    return isFilling;
                }
            }
            set
            {
                lock (isFillingLock)
                {
                    isFilling = value;
                }
            }
        }

        public async void FillQueuesPriority(int id, bool verbose)
        {
            Logger.LogInformation($"FillQueuesPriority({id}, {verbose}) starting");

            var board = await GenerateAsync(id, verbose);
            Cache.Enqueue(board);

            FillQueues(verbose);

            Logger.LogInformation($"FillQueuesPriority({id}, {verbose}) exiting");
        }

        public async void FillQueues(bool verbose)
        {
            Logger.LogInformation("FillQueues starting");

            if (!IsFilling)
            {
                IsFilling = true;

                await Task.Run(async () =>
                {
                    do
                    {
                        foreach (var puzzle in PuzzleRepository.GetAll().ToList())
                        {
                            if (!Cache.CacheFull(puzzle.Id))
                            {
                                await Task.Run(() => AddPuzzleBoard(puzzle, verbose));
                            }
                        }
                    } while (Cache.Keys.Any((k) => !Cache.CacheFull(k)));
                });

                IsFilling = false;
            }
            else
            {
                Logger.LogInformation("FillQueues already filling");
            }

            Logger.LogInformation("FillQueues exiting");
        }

        protected void AddPuzzleBoard(Puzzle puzzle, bool verbose)
        {
            Logger.LogInformation($"Replenishing cache for puzzleId={puzzle.Id}");
            var board = Generator.Generate(puzzle, verbose);
            Cache.Enqueue(board);
            Logger.LogInformation($"Replenishing cache for puzzleId={puzzle.Id} - DONE.");
        }

        public async Task<PuzzleBoard> GenerateAsync(int id, bool verbose)
        {
            var puzzle = PuzzleRepository.Get(id);
            return await Task.Run(() => Generator.Generate(puzzle, verbose));
        }
    }
}
