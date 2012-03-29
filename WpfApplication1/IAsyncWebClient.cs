using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public interface IAsyncWebClient
    {
        Task<string> GetStringAsync(
            Uri uri,
            IProgress<ProgressChangedEventArgs> progressChanged = null);

        Task<string> GetStringAsync(
            Uri uri,
            CancellationToken cancellationToken,
            IProgress<ProgressChangedEventArgs> progressChanged = null);

        Task<byte[]> GetDataAsync(
            Uri uri,
            IProgress<ProgressChangedEventArgs> progressChanged = null);

        Task<byte[]> GetDataAsync(
            Uri uri,
            CancellationToken cancellationToken,
            IProgress<ProgressChangedEventArgs> progressChanged = null);
    }

    public class AsyncWebClient : IAsyncWebClient
    {
        public Task<string> GetStringAsync(
            Uri uri,
            IProgress<ProgressChangedEventArgs> progressChanged = null)
        {
            return GetStringAsync(uri, CancellationToken.None, progressChanged);
        }

        public Task<string> GetStringAsync(
            Uri uri,
            CancellationToken cancellationToken,
            IProgress<ProgressChangedEventArgs> progressChanged = null)
        {
            // create completion source
            var tcs = new TaskCompletionSource<string>();

            // create a web client for downloading the string
            var wc = new WebClient();

            // Set up variable for referencing anonymous event handler method. We
            // need to first assign null, and then create the method so that we
            // can reference the variable within the method itself.
            DownloadStringCompletedEventHandler downloadCompletedHandler = null;

            // Set up an anonymous method that will handle the DownloadStringCompleted
            // event.
            downloadCompletedHandler = (s, e) =>
            {
                // Unsubscribe the event listener (to allow the WebClient to
                // be garbage collected).
                wc.DownloadStringCompleted -= downloadCompletedHandler;
                if (e.Cancelled || cancellationToken.IsCancellationRequested)
                {
                    // If the download was cancelled, signal cancellation to the
                    // task completion source.
                    tcs.TrySetCanceled();
                }
                else if (e.Error != null)
                {
                    // If the download failed, set the error on the task completion source
                    tcs.TrySetException(e.Error);
                }
                else
                {
                    // If the download was successful, set the result on the task completion
                    // source
                    tcs.TrySetResult(e.Result);
                }

                wc.Dispose();
            };

            // Subscribe to the completed event
            wc.DownloadStringCompleted += downloadCompletedHandler;
            wc.DownloadProgressChanged += (sender, args) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    wc.CancelAsync();
                }

                if (progressChanged != null)
                {
                    progressChanged.Report(args);
                }
            };

            // Start the asynchronous download

            wc.DownloadStringAsync(uri);

            // Return the Task object from the TaskCompletionSource. This object will be monitored
            // by the calling code for the result, that is provided by the TrySetXxxx calls above.
            return tcs.Task;
        }

        public Task<byte[]> GetDataAsync(
            Uri uri,
            IProgress<ProgressChangedEventArgs> progressChanged = null)
        {
            return GetDataAsync(uri, CancellationToken.None, progressChanged);
        }

        public Task<byte[]> GetDataAsync(
            Uri uri,
            CancellationToken cancellationToken,
            IProgress<ProgressChangedEventArgs> progressChanged = null)
        {
            // create completion source
            var tcs = new TaskCompletionSource<byte[]>();

            // create a web client for downloading the string
            var wc = new WebClient();

            // Set up variable for referencing anonymous event handler method. We
            // need to first assign null, and then create the method so that we
            // can reference the variable within the method itself.
            DownloadDataCompletedEventHandler downloadCompletedHandler = null;

            // Set up an anonymous method that will handle the DownloadStringCompleted
            // event.
            downloadCompletedHandler = (s, e) =>
            {
                // Unsubscribe the event listener (to allow the WebClient to
                // be garbage collected).
                wc.DownloadDataCompleted -= downloadCompletedHandler;
                if (e.Cancelled || cancellationToken.IsCancellationRequested)
                {
                    // If the download was cancelled, signal cancellation to the
                    // task completion source.
                    tcs.TrySetCanceled();
                }
                else if (e.Error != null)
                {
                    // If the download failed, set the error on the task completion source
                    tcs.TrySetException(e.Error);
                }
                else
                {
                    // If the download was successful, set the result on the task completion
                    // source
                    tcs.TrySetResult(e.Result);
                }

                wc.Dispose();
            };

            // Subscribe to the completed event
            wc.DownloadDataCompleted += downloadCompletedHandler;
            wc.DownloadProgressChanged += (sender, args) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    wc.CancelAsync();
                }

                if (progressChanged != null)
                {
                    progressChanged.Report(args);
                }
            };

            // Start the asynchronous download

            wc.DownloadDataAsync(uri);

            // Return the Task object from the TaskCompletionSource. This object will be monitored
            // by the calling code for the result, that is provided by the TrySetXxxx calls above.
            return tcs.Task;
        }
    }
}