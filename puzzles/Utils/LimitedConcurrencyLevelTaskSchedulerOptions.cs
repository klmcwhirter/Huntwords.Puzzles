//-------------------------------------------------------------------------- 
//  
//  Copyright (c) Microsoft Corporation.  All rights reserved.  
//  
//  File: LimitedConcurrencyTaskScheduler.cs 
// 
//-------------------------------------------------------------------------- 

namespace System.Threading.Tasks.Schedulers
{
    /// <summary> 
    /// Provides a task scheduler that ensures a maximum concurrency level while 
    /// running on top of the ThreadPool. 
    /// </summary> 
    public class LimitedConcurrencyLevelTaskSchedulerOptions
    {
        /// <summary>The maximum concurrency level allowed by this scheduler.</summary> 
        public int MaxDegreeOfParallelism { get; set; }
    }
}
