// See https://aka.ms/new-console-template for more information
using MongoDB.Driver;
using MyHostAPI.Domain;
using MyHostAPI.Domain.PredefinedFilter;
using MyHostAPI.Domain.Premise;
using Spectre.Console;

Console.WriteLine("Time is money friend!");

var connectionString = "mongodb+srv://admin:de199291@cluster0.x6livux.mongodb.net/?retryWrites=true&w=majority";
var databaseName = "MyHost";
var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);

while (true)
{
    var collection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Which [green]collection[/] you want to clean and seed?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to chose collection)[/]")
            .AddChoices(new[] {
            "User", "Premise", "Reservation",
            "Event", "PremiseType", "Tag",
            "OperatingCity", "PredefinedFilter", "Exit"
            }));

    switch (collection)
    {
        case "User":
            break;
        case "Premise":
            var premiseCollection = database.GetCollection<Premise>(typeof(Premise).Name);
            CleanAndSeed<Premise>(premiseCollection, PremiseData());
            break;
        case "Reservation":
            break;
        case "Event":
            break;
        case "PremiseType":
            var premiseTypeCollection = database.GetCollection<PremiseType>(typeof(PremiseType).Name);
            CleanAndSeed<PremiseType>(premiseTypeCollection, PremiseTypeData());
            break;
        case "Tag":
            var tagCollection = database.GetCollection<MyHostAPI.Domain.Tag>(typeof(MyHostAPI.Domain.Tag).Name);
            CleanAndSeed<MyHostAPI.Domain.Tag>(tagCollection, TagData());
            break;
        case "OperatingCity":
            var operatingCityCollection = database.GetCollection<OperatingCity>(typeof(OperatingCity).Name);
            CleanAndSeed<OperatingCity>(operatingCityCollection, OperatingCityData());
            break;
        case "PredefinedFilter":
            var predefinedFilterCollection = database.GetCollection<PredefinedFilter>(typeof(PredefinedFilter).Name);
            CleanAndSeed<PredefinedFilter>(predefinedFilterCollection, PredefinedFilterData());
            break;
        case "Exit":
            return;
        default:
            return;
    }

    AnsiConsole.WriteLine($"{collection} clean and seed completed!");
}

#region mockedData

void CleanAndSeed<T>(IMongoCollection<T> entityCollection, List<T> data) where T : BaseEntity
{
    var result = entityCollection.DeleteMany(_ => true);
    entityCollection.InsertMany(data);
}

List<Premise> PremiseData()
{
    double lng = 45.2426776;
    double lat = 19.8187811;
    List<Premise> data = new();
    for (int i = 0; i < 50; i++)
    {
        data.Add(new Premise()
        {
            Name = "Restaurant" + i,
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in " +
            "voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            PhoneNumbers = new List<string>() { "12345678", "81730274" },
            Images = new List<Image>() { new Image() { Order = 1, Path = "https://myhoststorage.blob.core.windows.net/myhostcontainer/image1.jpeg" }, new Image() { Order = 2, Path = "https://myhoststorage.blob.core.windows.net/myhostcontainer/image1.jpeg" } },
            ManagerId = "635414ad0547b40e0a860e3b",
            Location = new Location() { Lat = lng, Lng = lat, City = "Novi Sad", Number = "41", Street = "Teodora Pavlovica", Zip = "21000" },
            MenuItems = new List<Image>() { new Image() { Order = 1, Path = "https://myhoststorage.blob.core.windows.net/myhostcontainer/10439460_10203288040630535_5714273851971599548_n.jpg" },
                new Image() { Order = 2, Path = "https://myhoststorage.blob.core.windows.net/myhostcontainer/22788695_10209981392202477_8382035570080097892_n.jpg" },
                new Image() { Order = 3, Path = "https://myhoststorage.blob.core.windows.net/myhostcontainer/20915269_949353411899718_3739571095339162068_n.jpg" } },
            RatingAverage = i % 5,
            TimeSettings = new PremiseTimeSettings
            {
                DefaultDurationInMinutes = 120,
                MinDurationInMinutes = 60,
                MaxDurationInMinutes = 180,
                MinStartTimeFromNowInMinutes = 30,
                WorkHours = new List<PremiseWorkHours>() {
                new PremiseWorkHours() { ClosingTimeInMinutes = 1440, DayOfWeek = DayOfWeek.Monday, OpeningTimeInMinutes = 480 },
                new PremiseWorkHours() { ClosingTimeInMinutes = 1440, DayOfWeek = DayOfWeek.Tuesday, OpeningTimeInMinutes = 480 },
                new PremiseWorkHours() { ClosingTimeInMinutes = 1440, DayOfWeek = DayOfWeek.Wednesday, OpeningTimeInMinutes = 480 },
                new PremiseWorkHours() { ClosingTimeInMinutes = 1440, DayOfWeek = DayOfWeek.Thursday, OpeningTimeInMinutes = 480 },
                new PremiseWorkHours() { ClosingTimeInMinutes = 1500, DayOfWeek = DayOfWeek.Friday, OpeningTimeInMinutes = 480 },
                new PremiseWorkHours() { ClosingTimeInMinutes = 1500, DayOfWeek = DayOfWeek.Saturday, OpeningTimeInMinutes = 540 },
                new PremiseWorkHours() { ClosingTimeInMinutes = -1, DayOfWeek = DayOfWeek.Sunday, OpeningTimeInMinutes = -1 }},
                MinutInterval = 15,
            }
        });
        lng += 0.1;
        lat += 0.1;
    }

    return data;
}

List<MyHostAPI.Domain.Tag> TagData()
{
    List<MyHostAPI.Domain.Tag> data = new();
    var premisesTable = database.GetCollection<Premise>(typeof(Premise).Name);

    var ids = premisesTable.Find(_ => true).Skip(0).Limit(5).ToList().Select(x => x.Id).ToList() ?? new List<string>();
    data.Add(new MyHostAPI.Domain.Tag()
    {
        Name = "Restaurant",
        PremiseIds = ids
    });

    return data;
}

List<PredefinedFilter> PredefinedFilterData()
{
    List<PredefinedFilter> data = new();
    var premisesTable = database.GetCollection<Premise>(typeof(Premise).Name);

    var ids = premisesTable.Find(_ => true).Skip(0).Limit(5).ToList().Select(x => x.Id).ToList() ?? new List<string>();
    data.Add(new PredefinedFilter()
    {
        Name = "Garden",
        PremiseIds = new List<string>() { ids[1] }
    });

    data.Add(new PredefinedFilter()
    {
        Name = "LiveMusic",
        PremiseIds = new List<string>() { ids[2] }
    });

    data.Add(new PredefinedFilter()
    {
        Name = "PetFriendly",
        PremiseIds = new List<string>() { ids[3] }
    });

    return data;
}

List<OperatingCity> OperatingCityData()
{
    List<OperatingCity> data = new()
    {
        new OperatingCity()
        {
            Name = "Beograd",
            Lat = 44.83383893008614,
            Lng = 20.40967147135253
        },
        new OperatingCity()
        {
            Name = "Novi Sad",
            Lat = 45.30400409417275,
            Lng = 19.890529828744818

        },
        new OperatingCity()
        {
            Name = "Nis",
            Lat = 43.40424864324375,
            Lng = 21.932138335820564
        }
    };

    return data;
}

List<PremiseType> PremiseTypeData()
{
    List<PremiseType> data = new();
    var listOfPremiseTypes = new List<PremiseType>
    {
        new PremiseType() { Name = "Restaurant", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/Restaurant.jpeg", Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/plateIcon.png" },
        new PremiseType() { Name = "Pub", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/pub.jpeg" , Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/beerIcon.png" },
        new PremiseType() { Name = "PastryShop", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/PastryShop.jpeg", Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/cakeIcon.png" },
        new PremiseType() { Name = "Fastfood", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/Fastfood.jpeg", Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/burgerIcon.png" },
        new PremiseType() { Name = "CoffeeShop", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/CoffeeShop.jpeg", Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/cupIcon.png" },
        new PremiseType() { Name = "NightClub", CoverImage = "https://myhoststorage.blob.core.windows.net/myhostcontainer/NightClub.jpeg", Icon = "https://myhoststorage.blob.core.windows.net/myhostcontainer/glassIcon.png" }
    };
    var premisesTable = database.GetCollection<Premise>(typeof(Premise).Name);
    var ids = premisesTable.Find(_ => true).ToList().Select(x => x.Id).ToList();
    var pages = ids.Count / 6;
    int i = 0;
    foreach (var nameType in listOfPremiseTypes)
    {
        data.Add(new PremiseType()
        {
            Name = nameType.Name,
            CoverImage = nameType.CoverImage,
            PremiseIds = ids.Skip(i * pages).Take(pages).ToList(),
            Icon = nameType.Icon
        });
        i++;
    }

    return data;
}
#endregion