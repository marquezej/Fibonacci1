var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

FibonacciSequenceTracker f = new FibonacciSequenceTracker(92);

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
            return f.Next();
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
            return f.Next();
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
        return Results.NotFound();
    }
);

app.Run();

class FibonacciSequenceTracker
{
    private int Ceiling;
    private List<Int64> FullSequence ;
    private int cursor;

    private Int64 previous;
    private Int64 current;
    private Int64 next;

    public FibonacciSequenceTracker(int c){
        if(c != 92)
            throw new ArgumentException("Anything other than 92 is currently unsupported", c.ToString());
        
        previous = 4660046610375530309;
        current = -2880067194370816120;
        next = 1779979416004714189;
        Ceiling = c;
        FullSequence = new List<Int64>();

        FullSequence.Add(previous);
        for(int i = 1; i < Ceiling * 2; i++){
            FullSequence.Add(current);
            previous = current;
            current = next;
            next = current + previous;           
        }
        cursor = c - 1;

    }
    public string Next()
    {
        if(cursor + 1 >= FullSequence.Count)
            throw new OutofFibonacciRangeException();
        return FullSequence[++cursor].ToString();
    }
    public string Previous()
    {
        if(cursor - 1 < 0)
            throw new OutofFibonacciRangeException();
        return FullSequence[--cursor].ToString();
    }
    public string Current()
    {
        return FullSequence[cursor].ToString();
    }
    public void Reset()
    {
        cursor = Ceiling - 1;
    }
    public int FindCeiling()
    {
        return Ceiling;
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