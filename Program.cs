using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
/*
 * References:
 * https://sankarsan.wordpress.com/2011/11/20/roslyn-ctpa-walk-through-the-syntax-treepart-i/
 * http://www.swat4net.com/roslyn-you-part-iii-playing-with-syntax-trees/
 * https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
 * C# 8.0 in a Nutshell: The Definitive Reference Chapter 27
 */
class Program
{
    static void Main(string[] args)
    {
        string rootpath = @""; // add your file path-----------------------------------------------------------------------
        var files = Directory.GetFiles(rootpath, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.Substring(file.Length - 3) == ".cs")
            {
                Console.WriteLine(file);
                string programText = "";
                programText = File.ReadAllText(file);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
                var root = tree.GetRoot();
                // Get nodes that are Class Declarations
                var cds = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in cds)
                {
                    Console.WriteLine($"Class {classNode.Identifier}");

                    if (classNode.BaseList != null)
                    {
                        var baseTypes = classNode.BaseList.Types;
                        Console.WriteLine($"Derived from {string.Join(", ", baseTypes)}");
                        Console.WriteLine();
                    }

                    var properties = classNode.Members.OfType<PropertyDeclarationSyntax>();
                    var fields = classNode.Members.OfType<FieldDeclarationSyntax>();
                    var delegates = classNode.Members.OfType<DelegateDeclarationSyntax>();
                    var constructors = classNode.Members.OfType<ConstructorDeclarationSyntax>();
                    var methods = classNode.Members.OfType<MethodDeclarationSyntax>();

                    if (constructors.Any())
                        Console.WriteLine("Constructors: ");
                    foreach (var constructor in constructors)
                    {
                        Console.WriteLine($"   {constructor.Identifier}");
                        foreach (var parameter in constructor.ParameterList.Parameters)
                        {
                            Console.WriteLine($"     {parameter.Identifier}: {parameter.Type} {parameter.Default}");
                        }
                    }
                    Console.WriteLine();

                    if (properties.Any())
                        Console.WriteLine("Properties: ");
                    foreach (var property in properties)
                    {
                        Console.WriteLine($"   {property.Identifier}: {property.Type} ({property.Type.Kind()})");
                    }
                    Console.WriteLine();

                    if (fields.Any())
                        Console.WriteLine("Fields: ");
                    foreach (var field in fields)
                    {
                        Console.WriteLine($"   {string.Join(" ", field.Declaration.Variables)}: " +
                            $"{field.Declaration.Type} ({field.Declaration.Type.Kind()})");
                    }
                    Console.WriteLine();

                    if (methods.Any())
                        Console.WriteLine("Methods: ");
                    foreach (var method in methods)
                    {
                        Console.WriteLine($"   {method.Identifier}: {method.ReturnType}");
                        foreach (var parameter in method.ParameterList.Parameters)
                        {
                            Console.WriteLine($"     {parameter.Identifier}: {parameter.Type} {parameter.Default}");
                        }

                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                

            }

        }
        Console.ReadLine();

    }
}
