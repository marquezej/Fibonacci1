var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int previous = 0;

int current = 0;

int next = 1;

app.MapGet("/",  () =>
    {
        previous = 0;
        current = 0;
        next = 1;
        return "Fib sequence reset";
    }
);

app.MapGet("/current",  () =>
    {
        return current;
    }
);

app.MapGet("/previous",  () =>
    {
        previous = current - previous;
        next = current;
        current = current - previous;     
        return current;
    }
);

app.MapGet("/next",  () =>
    {
        previous = current;
        current = next;
        next = current + previous;     
        return current;
    }
);
app.Run();
