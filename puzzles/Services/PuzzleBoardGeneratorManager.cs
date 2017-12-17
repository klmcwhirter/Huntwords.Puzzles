using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using puzzles.Models;
using puzzles.Repositories;

namespace puzzles.Services
{
    /// <summary>
    /// This class manages populating the PuzzleBoardCache by prefilling the cache via the FillQueues method
    /// and registering to the cache's events to refill the cache as it is depleted.
    /// </summary>
    public class PuzzleBoardGeneratorManager
    {
        /// <summary>
        /// PuzzleBoardCache instance that holds the generated PuzzleBoard instances
        /// </summary>
        /// <returns></returns>
        protected PuzzleBoardCache Cache { get; }
        /// <summary>
        /// Factory method to activate generators
        /// </summary>
        /// <remarks>
        /// See http://autofac.readthedocs.io/en/latest/resolve/relationships.html#dynamic-instantiation-func-b
        /// </remarks>
        /// <returns></returns>
        protected Func<IGenerator<PuzzleBoard>> GeneratorFactory { get; }
        /// <summary>
        /// ILogger instance for this class
        /// </summary>
        /// <returns></returns>
        protected ILogger<PuzzleBoardGeneratorManager> Logger { get; set; }
        /// <summary>
        /// Factory method to activate puzzle repository instances
        /// </summary>
        /// <remarks>
        /// See http://autofac.readthedocs.io/en/latest/resolve/relationships.html#dynamic-instantiation-func-b
        /// </remarks>
        /// <returns></returns>
        protected Func<IPuzzlesRepository> PuzzleRepositoryFactory { get; }

        /// <summary>
        /// Construct a PuzzleBoardGeneratorManager
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="generatorFactory"></param>
        /// <param name="puzzleRepositoryFactory"></param>
        /// <param name="logger"></param>
        public PuzzleBoardGeneratorManager(
            PuzzleBoardCache cache,
            Func<IGenerator<PuzzleBoard>> generatorFactory,
            Func<IPuzzlesRepository> puzzleRepositoryFactory,
            ILogger<PuzzleBoardGeneratorManager> logger
        )
        {
            Cache = cache;
            GeneratorFactory = generatorFactory;
            PuzzleRepositoryFactory = puzzleRepositoryFactory;
            Logger = logger;

            Cache.PuzzleBoardQueueEmpty += (sender, id) => FillQueuesPriority(id, false);
            Cache.PuzzleBoardDequeued += (sender, args) => FillQueues(false);
        }

        /// <summary>
        /// Guard that tells whether we are in the process of filling or not
        /// </summary>
        protected bool isFilling = false;
        /// <summary>
        /// Lock around isFilling guard
        /// </summary>
        /// <returns></returns>
        protected object isFillingLock = new object();
        /// <summary>
        /// Property for isFilling that bounds usage with the isFillingLock
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Attempts to generate a puzzle for id outside of the FillQueues loop
        /// </summary>
        /// <param name="id"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public async void FillQueuesPriority(int id, bool verbose)
        {
            Logger.LogInformation($"FillQueuesPriority({id}, {verbose}) starting");

            var board = await GenerateAsync(id, verbose);
            Cache.Enqueue(board);

            FillQueues(verbose);

            Logger.LogInformation($"FillQueuesPriority({id}, {verbose}) exiting");
        }

        /// <summary>
        /// Loops over puzzles and if the cache is not full for that id generate a PuzzleBoard and place it in the cache
        /// </summary>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public async void FillQueues(bool verbose)
        {
            Logger.LogInformation("FillQueues starting");

            if (!IsFilling)
            {
                IsFilling = true;

                await Task.Run(async () =>
                {
                    var puzzleRepository = PuzzleRepositoryFactory();
                    do
                    {
                        foreach (var puzzle in puzzleRepository.GetAll().ToList())
                        {
                            if (!Cache.CacheFull(puzzle.Id))
                            {
                                Logger.LogInformation($"Cache is not full for puzzleId={puzzle.Id}");
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

        /// <summary>
        /// Generate a PuzzleBoard and add it to the cache
        /// </summary>
        /// <param name="puzzle"></param>
        /// <param name="verbose"></param>
        protected void AddPuzzleBoard(Puzzle puzzle, bool verbose)
        {
            Logger.LogInformation($"Replenishing cache for puzzleId={puzzle.Id}");
            var generator = GeneratorFactory();
            var board = generator.Generate(puzzle, verbose);
            Cache.Enqueue(board);
            Logger.LogInformation($"Replenishing cache for puzzleId={puzzle.Id} - DONE.");
        }

        /// <summary>
        /// Generate a PuzzleBoard asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public async Task<PuzzleBoard> GenerateAsync(int id, bool verbose)
        {
            var puzzleRepository = PuzzleRepositoryFactory();
            var puzzle = puzzleRepository.Get(id);
            var generator = GeneratorFactory();
            return await Task.Run(() => generator.Generate(puzzle, verbose));
        }
    }
}
