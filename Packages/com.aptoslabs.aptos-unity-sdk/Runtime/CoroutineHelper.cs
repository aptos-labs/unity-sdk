using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Aptos
{
    /// <summary>
    /// Helper class for working with coroutines.
    /// </summary>
    /// <example>
    /// yield return CoroutineHelper.Await(async () =>
    /// {
    ///     var client = new AptosClient();
    ///
    ///     using (
    ///         var res = await client.SomeMethod(...)
    ///     )
    ///     {
    ///         // Handle response
    ///     }
    /// });
    /// </example>
    public static class CoroutineHelper
    {
        /// <summary>
        /// Await the completion of a task and throw any exceptions.
        /// </summary>
        /// <example>
        /// var client = new AptosClient();
        /// yield return CoroutineHelper.Await(client.SomeAsyncMethod(...));
        /// </example>
        /// <param name="task">The task to await.</param>
        /// <returns>An IEnumerator that can be yielded to.</returns>
        public static IEnumerator Await(Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// Await the completion of a task and throw any exceptions.
        /// </summary>
        /// <example>
        /// yield return CoroutineHelper.Await(async () =>
        /// {
        ///     var client = new AptosClient();
        ///
        ///     using (
        ///         var res = await client.SomeMethod(...)
        ///     )
        ///     {
        ///         // Handle response
        ///     }
        /// });
        /// </example>
        /// <param name="taskDelegate">The task delegate to await.</param>
        /// <returns>An IEnumerator that can be yielded to.</returns>
        public static IEnumerator Await(Func<Task> taskDelegate)
        {
            return Await(taskDelegate.Invoke());
        }
    }
}
