using AutoMapper;
using System;

public class UlidTypeConverter : ITypeConverter<string, Ulid>, ITypeConverter<Ulid, string>
{
    public Ulid Convert(string source, Ulid destination, ResolutionContext context)
    {
        return Ulid.Parse(source);
    }

    public string Convert(Ulid source, string destination, ResolutionContext context)
    {
        return source.ToString();
    }
}
