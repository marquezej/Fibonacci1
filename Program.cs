var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const int fibNCeiling = 92;

Int64 previous = 4660046610375530309;
Int64 current = -2880067194370816120;
Int64 next = 1779979416004714189;

List<Int64> FullSequence = new List<Int64>();

FullSequence.Add(previous);

for(int i = 1; i < fibNCeiling * 2; i++){
    //Console.WriteLine(current);
    FullSequence.Add(current);
    previous = current;
    current = next;
    next = current + previous;           
}

foreach(var z in FullSequence){
    Console.WriteLine(z);
}

int cursor = fibNCeiling - 1;

app.MapGet("/",  () =>
    {
        cursor = fibNCeiling - 1;
        return "Fib sequence reset";
    }
);

app.MapGet("/current",  () =>
    {
        return FullSequence[cursor];
    }
);

app.MapGet("/previous",  () =>
    {
        if(--cursor < 0)
            cursor = 0;
        return FullSequence[cursor];
    }
);

app.MapGet("/next",  () =>
    {
       if(++cursor >= FullSequence.Count)
            cursor = FullSequence.Count - 1;
        return FullSequence[cursor];
    }
);

app.MapGet("/bird",  () =>
    {      
        return Results.NotFound();
    }
);

app.Run();

