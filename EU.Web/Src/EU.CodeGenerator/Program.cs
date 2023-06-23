using EU.Core.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JianLian.HDIS.CodeGenerator;
class Program
{
    static void Main(string[] args)
    {
        EU.CodeGenerator.Utilities.BasePath = Environment.CurrentDirectory.Replace(@"EU.CodeGenerator\bin\Debug\net7.0", "");

        var container = new ServiceCollection();
        var Configuration = new ConfigurationBuilder()
           .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
           .Build();
        AppSetting.Init(container, Configuration);

        Console.WriteLine("请输入表名!");
        string table = Console.ReadLine() ?? "SmApiLog";

        EU.CodeGenerator.Utilities.FileName = ToUnderscoreCase(table)[0].ToUpper();

        //Console.WriteLine($"FileName：{FileName}");
        ModelGenerator.Generator(table);
        // ALL Completed
        Console.WriteLine("ALL Completed!");
        Console.ReadKey();
        Thread.Sleep(Timeout.Infinite);

    }

    public static List<string> ToUnderscoreCase(string str)
    {
        var str1 = string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        return str1.Split('_').ToList();
    }
}