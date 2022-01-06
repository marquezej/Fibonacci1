var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

FibonacciSequenceTracker f = new FibonacciSequenceTracker(92);

app.Urls.Add("http://localhost:5037");

app.MapGet("/",  () =>
    {
        f.Reset();
         return "Fib sequence reset";
    }
);

app.MapGet("/current",  () =>
    {
        return f.Current();
    }
);

app.MapGet("/previous",  () =>
    {
        try
        {
            return f.Previous();
        }
        catch(OutofFibonacciRangeException E)
        {
            f.Reset();
            throw;
        }
    }
);

app.MapGet("/next",  () =>
    {
        try
        {
            string s =f.Next();
            return s;
        }
        catch(OutofFibonacciRangeException E)
        {
            f.Reset();
            throw;
            //return Results.NotFound();
        }
        
    }
);

app.MapGet("/bird",  () =>
    {      
        return Results.NotFound( 
            new { error = "FIB-0001",
                  message = "Out of range of calculable Fibonacci Numbers",
                  detail = "Only up to 64bit integers are implemented in this server, anything higher than 92nd Fibonacci number will need to be"}
         );
    }
);

app.Run();

class FibonacciSequenceTracker
{
    private int _ceiling;
    private int _cursor;
    private List<Int64> _fullSequence ;
    private Int64 _previous;
    private Int64 _current;
    private Int64 _next;


    public FibonacciSequenceTracker(int maximumFibonacciNumberN){
        if(maximumFibonacciNumberN != 92)
            throw new ArgumentException("Anything other than 92 is currently unsupported", maximumFibonacciNumberN.ToString());
        
        _previous = 4660046610375530309;
        _current = -2880067194370816120;
        _next = 1779979416004714189;
        _ceiling = maximumFibonacciNumberN;
        _fullSequence = new List<Int64>();

        _fullSequence.Add(_previous);
        for(int i = 1; i < _ceiling * 2; i++){
            _fullSequence.Add(_current);
            _previous = _current;
            _current = _next;
            _next = _current + _previous;           
        }
        _cursor = maximumFibonacciNumberN - 1;

    }
    public string Next()
    {
        if(_cursor + 1 >= _fullSequence.Count)
            throw new OutofFibonacciRangeException();
        return _fullSequence[++_cursor].ToString();
    }
    public string Previous()
    {
        if(_cursor - 1 < 0)
            throw new OutofFibonacciRangeException();
        return _fullSequence[--_cursor].ToString();
    }
    public string Current()
    {
        return _fullSequence[_cursor].ToString();
    }
    public void Reset()
    {
        _cursor = _ceiling - 1;
    }
    public int FindCeiling()
    {
        return _ceiling;
    }

}

[Serializable]
public class OutofFibonacciRangeException : Exception
{
    public OutofFibonacciRangeException() : base() { }
    public OutofFibonacciRangeException(string message) : base(message) { }
    public OutofFibonacciRangeException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected OutofFibonacciRangeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}