using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyExplode.Utils.Extensions;

internal static class HttpExtensions
{
    public static async ValueTask<HttpResponseMessage> HeadAsync(
        this HttpClient http,
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Head, requestUri);
        return await http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );
    }

    public static async ValueTask<Stream> GetStreamAsync(
        this HttpClient http,
        string requestUri,
        long? from = null,
        long? to = null,
        bool ensureSuccess = true,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Range = new RangeHeaderValue(from, to);

        var response = await http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        if (ensureSuccess)
            response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    public static async ValueTask<long?> TryGetContentLengthAsync(
        this HttpClient http,
        string requestUri,
        bool ensureSuccess = true,
        CancellationToken cancellationToken = default)
    {
        using var response = await http.HeadAsync(requestUri, cancellationToken);

        if (ensureSuccess)
            response.EnsureSuccessStatusCode();

        return response.Content.Headers.ContentLength;
    }

    public static async ValueTask<string> GetAsync(
        this HttpClient http,
        string url,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async ValueTask<string> PostAsync(
        this HttpClient http,
        string url,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async ValueTask<string> PostAsync(
        this HttpClient http,
        string url,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        for (var j = 0; j < headers.Count; j++)
            request.Headers.TryAddWithoutValidation(headers.ElementAt(j).Key, headers.ElementAt(j).Value);

        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async ValueTask<string> PostAsync(
        this HttpClient http,
        string url,
        IDictionary<string, string>? headers,
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        for (var j = 0; j < headers?.Count; j++)
            request.Headers.TryAddWithoutValidation(headers.ElementAt(j).Key, headers.ElementAt(j).Value);

        request.Content = content;

        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async ValueTask<long> GetFileSizeAsync(
        this HttpClient http,
        string url,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Head, url);
        for (var j = 0; j < headers.Count; j++)
            request.Headers.TryAddWithoutValidation(headers.ElementAt(j).Key, headers.ElementAt(j).Value);

        using var response = await http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})." +
                Environment.NewLine +
                "Request:" +
                Environment.NewLine +
                request
            );
        }

        return response.Content.Headers.ContentLength ?? 0;
    }

    public static async ValueTask<string> ExecuteAsync(
        this HttpClient http,
        string url,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async ValueTask<string> ExecuteAsync(
        this HttpClient http,
        string url,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        for (var j = 0; j < headers.Count; j++)
            request.Headers.TryAddWithoutValidation(headers.ElementAt(j).Key, headers.ElementAt(j).Value);

        return await http.ExecuteAsync(request, cancellationToken);
    }

    public static async Task<string> ExecuteAsync(
        this HttpClient http,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        // User-agent
        if (!request.Headers.Contains("User-Agent"))
        {
            request.Headers.Add(
                "User-Agent",
                //The generated user-agent will not work on mobile devices in some cases.
                //Http.ChromeUserAgent()
                "Other"
            );
        }

        using var response = await http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        if (response.StatusCode == HttpStatusCode.NotFound)
            return string.Empty;

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})." +
                Environment.NewLine +
                "Request:" +
                Environment.NewLine +
                request
            );
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}