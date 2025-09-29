using LargeDictionary_R7;
using System.Diagnostics;

var len = int.MaxValue >> 2;
var sw = new Stopwatch();


StartTestDict();        //1m 40 sec - 2m 48 sec / get 00.085 sec
StartTestLargeDict();   //25-30 sec / get 00.058 sec

void Add(Dictionary<long, long> dict, long len, Stopwatch sw)
{
    for (long i = 0; i < len; i++)
    {
        dict[i] = i;
        if (i % 1000000 == 0 && i != 0)
        {
            Console.WriteLine($"{i};{sw.Elapsed}");
        }
    }
}

void Get(Dictionary<long, long> dict, long len, Stopwatch sw)
{
    for (long i = 1; i * 10000000 < len; i++)
    {
        Console.WriteLine($"{dict[i * 10000000]};{sw.Elapsed}");
    }
}

void Add2(LargeDictionary<long, long> dict, long len, Stopwatch sw)
{
    for (long i = 0; i < len; i++)
    {
        dict[i] = i;
        if (i % 1000000 == 0 && i != 0)
        {
            Console.WriteLine($"{i};{sw.Elapsed}");
        }
    }
}

void Get2(LargeDictionary<long, long> dict, long len, Stopwatch sw)
{
    for (long i = 1; i * 10000000 < len; i++)
    {
        Console.WriteLine($"{dict[i * 10000000]};{sw.Elapsed}");
    }
}

void StartTestDict()
{
    var dict = new Dictionary<long, long>(210);
    sw.Start();
    Add(dict, len, sw);
    sw.Stop();
    sw.Restart();
    Get(dict, len, sw);
    sw.Stop();
}

void StartTestLargeDict()
{
    var dict = new LargeDictionary<long, long>();
    sw.Start();
    Add2(dict, len, sw);
    sw.Stop();
    sw.Restart();
    Get2(dict, len, sw);
    sw.Stop();
}