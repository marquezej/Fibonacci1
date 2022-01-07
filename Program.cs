// The specifications were pretty vague in a lot of ways and you can go pretty crazy with this if you take it as far as humanly possible so
// I set some limits and made some assumptions.
// - There are a limited amount of Fibonacci numbers possible to express with predefined types, so I went with 64 bit integers, n = 91 being the last fibonacci
//   number available.
// - Negative fibonacci numbers are okay too ie: (-21 13 -8 5 -3 2 -1 1 0 1 1 2 3 5 8 13)
// - The one cpu is capable of multithreading
// - Recovery does not involve picking back up where the fibonacci sequence was previously. Recovery is simply starting the sequence again from scratch.

using System;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

FibonacciSequenceTracker f = new FibonacciSequenceTracker(92);

// Choose a URL to listen on.
app.Urls.Add("http://0.0.0.0:5037");


// register an exception handler to handle errors
app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        
        {

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();
            
            // This error is expected to happen when the user tries to access a Fibonacci number out of range, so a 404 is used.
            if (exceptionHandlerPathFeature?.Error is OutofFibonacciRangeException)
            {
                f.Reset();
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = Text.Plain;
                await context.Response.WriteAsync("Out of range of calculable Fibonacci Numbers, resetting....");
            }
            // This error is unexpected so I am using a 500 server error
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = Text.Plain;
                await context.Response.WriteAsync("An exception was thrown.");

            }
        });
    });


// The root path will reset the tracker, mainly for debug purposes.
app.MapGet("/",  () =>
    {
        f.Reset();
         return "Fib sequence reset";
    }
);

// Return the current fibonacci number
app.MapGet("/current",  () =>
    {
        return f.Current();
    }
);

// Return the previous fibonacci number
app.MapGet("/previous",  () =>
    {
      return f.Previous();
    }
);

// Return the next fibonacci number
app.MapGet("/next",  () =>
    {
       return f.Next();
    }
);

app.Run();

// 
class FibonacciSequenceTracker
{
    private int _ceiling;
    private int _cursor;
    private List<Int64> _cache ;
    private Int64 _previous;
    private Int64 _current;
    private Int64 _next;


    public FibonacciSequenceTracker(int maximumFibonacciNumberN){
        
        // When the nth Number of the Fibonacci sequence is greater than 91, this exceeds the upper bound of 64 bit integers so we're only going to go this far
        if(maximumFibonacciNumberN != 92)
            throw new ArgumentException("Anything other than 92 is currently unsupported", maximumFibonacciNumberN.ToString());
        
        // 4660046610375530309, -2880067194370816120, 1779979416004714189 are at the lower bound of fibonacci numbers supported by 64 bit integers so I am choosing 
        // them for the starting point
        _previous = 4660046610375530309;
        _current = -2880067194370816120;
        _next = 1779979416004714189;
        _ceiling = maximumFibonacciNumberN;
        _cache = new List<Int64>();

        // Precalculating all Fibonacci numbers in range of 64 bit integers and filling the cache with them.
        _cache.Add(_previous);
        for(int i = 1; i < _ceiling * 2; i++){
            _cache.Add(_current);
            _previous = _current;
            _current = _next;
            _next = _current + _previous;           
        }
        // Setting the fibonacci tracker at n = 0. It is important to note that since we have negative fibonacci numbers the _cursor is not equal to n.
        _cursor = maximumFibonacciNumberN - 1;

    }
    // This will iterate to the next fibonacci number, we simply move our cursor forward n+1 in the fib sequence. If the user requests a Fib number outside 
    // the precalculated range and exception will be thrown.
    public string Next()
    {
        if(_cursor + 1 >= _cache.Count)
            throw new OutofFibonacciRangeException();
        return _cache[++_cursor].ToString();
    }

    // This will descend to the previous fibonacci number, we simply move our cursor forward n-1 in the fib sequence. If the user requests a Fib number outside 
    // the precalculated range and exception will be thrown.
    public string Previous()
    {
        if(_cursor - 1 < 0)
            throw new OutofFibonacciRangeException();
        return _cache[--_cursor].ToString();
    }
    // Do nothing except tell user which Fib number is being used currently.
    public string Current()
    {
        return _cache[_cursor].ToString();
    }
    // Reset the Fib sequence to n = 0;
    public void Reset()
    {
        _cursor = _ceiling - 1;
    }
    // Gives user the ceiling of how far the range is calculated.
    public int FindCeiling()
    {
        return _ceiling;
    }

}
// Error to describe attempting to access a Fib number out of range.
[Serializable]
public class OutofFibonacciRangeException : Exception
{
    public OutofFibonacciRangeException() : base() { }
    public OutofFibonacciRangeException(string message) : base(message) { }
    public OutofFibonacciRangeException(string message, Exception inner) : base(message, inner) { }

    protected OutofFibonacciRangeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}