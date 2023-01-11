// See https://aka.ms/new-console-template for more information
using MorseCodeEncDec;

string encoded = MorseCode.Encode("Nazdar, pěnkavo moje! Chór v kostele zní nádherně. 1234567890 ()");

System.Console.WriteLine(encoded);
System.Console.WriteLine(MorseCode.Decode(encoded));

