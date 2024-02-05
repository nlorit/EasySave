// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, EasySave!");
Console.WriteLine("Louis est de retour !");

int Ajouter(int x, int y)
{
    return x + y;
}
int number1; 
int number2;

Console.WriteLine("Bienvenue sur la calculatrice qui ne fait que des additions");
Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("Premier nombre :");
number1 = int.Parse(Console.ReadLine());
Console.WriteLine("Deuxième nombre :");
number2 = int.Parse(Console.ReadLine());
Console.WriteLine("Le résultat est :" + Ajouter(number1, number2));

