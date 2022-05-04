using Spectre.Console;
using System.Diagnostics;


AnsiConsole.Write(new FigletText("Proxies.RIP")
    .Centered()
    .Color(Color.Cyan2));
Var.Proxy = ChangeProxy();

while (true)
{
    switch (Menu()) {
        case "Country selection":
            switch (CountriesMenu())
            {
                case "Select countries":
                    Var.countries = SelectCountries();
                    break;
                default:
                    break;
            }
            break;
        case "Sticky sessions":
            switch (StickyMenu())
            {
                case "Amount":
                    Var.Amount = ChangeAmount();
                    break;
                default:
                    break;
            }
            break;
        case "Generate":
            Generate();
            break;
        case "Change Proxy":
            Var.Proxy = ChangeProxy();
            break;
        case "Reset to defaults":
            Var.countries = null;
            Var.Amount = 0;
            break;
        default:
            Process.GetCurrentProcess().Kill();
            break;
    };
}

static string Menu()
{
    Console.Clear();
    AnsiConsole.Write(
    new FigletText("Proxies.RIP")
        .Centered()
        .Color(Color.Cyan2));
    return AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"    [red]Current proxy: [/]{Var.Proxy} [blue]Amount: [/]{Var.Amount} [blue]Selected Countries: [/]" + (Var.countries is not null ? Var.countries.Aggregate((a, b) => a + ", " + b) : "None"))
        .PageSize(10)
        .AddChoices(new[] {
            "Generate","Country selection", "Sticky sessions","Change Proxy","Reset to defaults", "[red]Exit[/]",
        }));
}


static string CountriesMenu()
{
    Console.Clear();
    AnsiConsole.Write(
    new FigletText("Proxies.RIP")
        .Centered()
        .Color(Color.Cyan2));
    return AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"    [red]Current proxy: [/]{Var.Proxy} [cyan]Selected Countries: [/]" + (Var.countries is not null ? Var.countries.Aggregate((a, b) => a + ", " + b):  "None"))
        .PageSize(10)
        .AddChoices(new[] {
            "Select countries", "[yellow]return[/]"
        }));
}
static string StickyMenu()
{
    Console.Clear();
    AnsiConsole.Write(
    new FigletText("Proxies.RIP")
        .Centered()
        .Color(Color.Cyan2));
    return AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"    [red]Current proxy: [/]{Var.Proxy} [cyan]Amount of sticky sessions: [/]" + Var.Amount)
        .PageSize(10)
        .AddChoices(new[] {
            "Amount", "[yellow]return[/]"
        }));
}

static string ChangeProxy()
{
    return AnsiConsole.Ask<string>("(ip:port:user:pass) ex. s2.proxies.rip:1234:username:p4ssw0rd\n[blue]proxy:[/] ");
}

static int ChangeAmount()
{
    return Convert.ToInt32(AnsiConsole.Ask<string>("[cyan]Amount:[/] "));
}

static List<string> SelectCountries()
{
    return AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .PageSize(20)
        .Title("Select [green]countries[/]")
        .InstructionsText("[grey](Press [blue][[space]][/] to toggle a country, [green][[enter]][/] to accept)[/]")
        .AddChoices(new[]
        {
            "UnitedStates","Canada","Afghanistan","Albania","Algeria","Argentina","Armenia","Aruba","Australia","Austria","Azerbaijan","Bahamas","Bahrain","Bangladesh","Belarus","Belgium","BosniaandHerzegovina","Brazil","BritishVirginIslands","Brunei","Bulgaria","Cambodia","Cameroon","Canada","Chile","China","Colombia","CostaRica","Croatia","Cuba","Cyprus","Czechia","Denmark","DominicanRepublic","Ecuador","Egypt","ElSalvador","Estonia","Ethiopia","Finland","France","Georgia","Germany","Ghana","Greece","Guatemala","Guyana","HashemiteKingdomofJordan","HongKong","Hungary","India","Indonesia","Iran","Iraq","Ireland","Israel","Italy","Jamaica","Japan","Kazakhstan","Kenya","Kosovo","Kuwait","Latvia","Liechtenstein","Luxembourg","Macedonia","Madagascar","Malaysia","Mauritius","Mexico","Mongolia","Montenegro","Morocco","Mozambique","Myanmar","Nepal","Netherlands","NewZealand","Nigeria","Norway","Oman","Pakistan","Palestine","Panama","PapuaNewGuinea","Paraguay","Peru","Philippines","Poland","Portugal","PuertoRico","Qatar","RepublicofLithuania","RepublicofMoldova","Romania","Russia","SaudiArabia","Senegal","Serbia","Seychelles","Singapore","Slovakia","Slovenia","Somalia","SouthAfrica","SouthKorea","Spain","SriLanka","Sudan","Suriname","Sweden","Switzerland","Syria","Taiwan","Tajikistan","Thailand","TrinidadandTobago","Tunisia","Turkey","Uganda","Ukraine","UnitedArabEmirates","UnitedKingdom","UnitedStates","Uzbekistan","Venezuela","Vietnam","Zambia"
        }));
}

static void Generate()
{
    List<string> lines = new();
    if (Var.Amount == 0)
    {
        string proxy = Var.Proxy;
        if (Var.countries is not null)
        {
            foreach (string country in Var.countries)
            {
                proxy += "_country-" + country;
            }
        }
        lines.Add(proxy);
    }
    else
    {
        for (int i = 0; i < Var.Amount; i++)
        {
            string proxy = Var.Proxy + "_session-" + i;
            if (Var.countries is not null)
            {
                foreach (string country in Var.countries)
                {
                    proxy += "_country-" + country;
                }
            }
            lines.Add(proxy);
        }
    }


    string file = Path.GetTempFileName() + ".txt";

    File.WriteAllLines(file, lines);
    var fileToOpen = file;
    var process = new Process
    {
        StartInfo = new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = fileToOpen
        }
    };
    AnsiConsole.WriteLine("Waiting for file to close....");
    process.Start();
    process.WaitForExit();
}
class Var
{
    public static string Proxy = String.Empty;
    public static int Amount = 0;
    public static List<string>? countries = null;
}

