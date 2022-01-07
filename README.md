# Fibonacci1
Web API for the Fibonacci sequence.

## Running with Docker:
```
docker build -t fibonacci . 
docker run -p 5037:5037 --cpus=1.00 -m 512m fibonacci 
```

## Simple usage
- Next number in sequence `http://localhost:5037/next`
- Previous number in sequence `http://localhost:5037/previous`
- Reset Sequence `http://localhost:5037/next`