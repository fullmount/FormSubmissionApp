using System;

public class StorageLimitExceededException : Exception
{
    public StorageLimitExceededException(string message) : base(message) { }
} 