using AutoMapper;
using System;

public static class UlidTypeConverter //:ITypeConverter<string, Ulid>, ITypeConverter<Ulid, string>
{
    public static Ulid ConvertToUlid(this string source, Ulid destination, ResolutionContext context)
    {
        return Ulid.Parse(source);
    }

    public static string ConvertFromUlid(Ulid source, string destination, ResolutionContext context)
    {
        return source.ToString();
    }
}
