using System.Collections;
using System.Collections.Generic;
using System;


public class NameGenerator
{
    private static readonly string[] FirstNames = {
        "Aria", "Daydream", "Nova", "Sunrise", "River", "Ivy", "Milo", "Tokyo", "Leo", "Luna", "Kai", "Whisper", "Princess", "Love", "Valentina", "Fredward", "Lavender", "Moonage", "Rhapsody", "Maze", "Lana", "Cyber", "Hope", "Sonnet", "Adagio", "Ether"
    };

    private static readonly string[] LastNames = {
        "Everest", "Stone", "Wilder", "Blake", "Rivera", "Knight", "Fox", "Vale", "Quinn", "Skye", "Vendetta", "Starwhale", "Halcyon", "Nocturne", "Beloved", "Serendipitous", "Aubade", "Stormblossom", "Runner", "Rose", "Lumalily", "Sonora", "Trustfell", "True", "Velvet", "Solstice"
    };

    private static readonly Random random = new Random();

    public static string GenerateRandomName()
    {
        string firstName = FirstNames[random.Next(FirstNames.Length)];
        string lastName = LastNames[random.Next(LastNames.Length)];
        return $"{firstName} {lastName}";
    }
}
