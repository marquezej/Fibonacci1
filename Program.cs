var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Int64 previous = 0;
Int64 current = 0;
Int64 next = 1;

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


int GetNthFibonacci_Ite(int n)  
{  
    int number = n - 1; //Need to decrement by 1 since we are starting from 0  
    int[] Fib = new int[number + 1];  
    Fib[0]= 0;  
    Fib[1]= 1;  
    for (int i = 2; i <= number;i++)  
    {  
       Fib[i] = Fib[i - 2] + Fib[i - 1];  
    }  
    return Fib[number];  
}