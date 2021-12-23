// See https://aka.ms/new-console-template for more information
using MagicNumber;

Console.WriteLine("Hello, World!");

var files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "files"));
var magic = new Magic();
foreach (var file in files)
{ 
    var ext = Path.GetExtension(file).Replace(".", "").ToLower();
    Console.WriteLine(file);
    using (FileStream stream = File.Open(file, FileMode.Open))
    {
        Console.WriteLine(magic.Match(stream, ext));
    }
}
