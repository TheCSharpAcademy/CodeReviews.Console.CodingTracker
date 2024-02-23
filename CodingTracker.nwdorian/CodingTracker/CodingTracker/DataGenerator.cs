using Bogus;
using CodingTracker.Models;

namespace CodingTracker;
internal class DataGenerator
{
    internal static List<Coding> GenerateCodingData()
    {
        Randomizer.Seed = new Random(1337);

        var faker = new Faker<Coding>()
            .RuleFor(c => c.StartTime, f => f.Date.Recent(100))
            .RuleFor(c => c.EndTime, (f, c) => c.StartTime.AddMinutes(f.Random.Number(1, 600)));

        var coding = faker.Generate(100);

        return coding;
    }
}
