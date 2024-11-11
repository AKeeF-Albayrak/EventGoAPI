using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, RateLimitInfo> _clients = new();
    private readonly int _maxRequests;
    private readonly TimeSpan _resetTime;

    public RateLimitingMiddleware(RequestDelegate next, int maxRequests = 60, int resetTimeInSeconds = 60)
    {
        _next = next;
        _maxRequests = maxRequests;
        _resetTime = TimeSpan.FromSeconds(resetTimeInSeconds);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(clientIp))
        {
            await _next(context);
            return;
        }

        var rateLimitInfo = _clients.GetOrAdd(clientIp, new RateLimitInfo(_maxRequests, _resetTime));

        if (!rateLimitInfo.AllowRequest())
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Too many requests. Please try again later.");
            return;
        }

        await _next(context);
    }
}

public class RateLimitInfo
{
    private int _requestCount;
    private DateTime _resetTime;

    public RateLimitInfo(int maxRequests, TimeSpan resetTime)
    {
        MaxRequests = maxRequests;
        ResetTimeSpan = resetTime;
        _resetTime = DateTime.UtcNow.Add(ResetTimeSpan);
        _requestCount = 0;
    }

    public int MaxRequests { get; }
    public TimeSpan ResetTimeSpan { get; }

    public bool AllowRequest()
    {
        if (DateTime.UtcNow > _resetTime)
        {
            _requestCount = 0;
            _resetTime = DateTime.UtcNow.Add(ResetTimeSpan);
        }

        if (_requestCount < MaxRequests)
        {
            _requestCount++;
            return true;
        }

        return false;
    }
}
