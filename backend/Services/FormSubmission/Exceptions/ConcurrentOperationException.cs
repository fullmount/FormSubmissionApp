using System;

public class ConcurrentOperationException : Exception
{
    public ConcurrentOperationException(string message) : base(message) { }
} 