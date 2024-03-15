using System.Diagnostics;

namespace baitap;

public class Handle: IHandle
{
    private readonly GenerateClientCode _generateClientCode;
    private readonly HashSet<string> _clientCodes;
    private readonly List<string> _group0;
    private readonly List<string> _group1;
    private readonly List<string> _group2;
    private const int _instanceTotal = 3;

    public Handle()
    {
        _generateClientCode = new GenerateClientCode();
        _clientCodes = _generateClientCode.Generate();
        _group0 = new List<string>();
        _group1 = new List<string>();
        _group2 = new List<string>();
    }
    public void HandleExecution(int handleType)
    {
        ClearGroup();
        var stopwatch = MeasureExecutionTime(() =>
        {
            foreach (var clientCode in _clientCodes)
            {
                int number = -1;
                if (handleType == 0)
                    number = System.Text.Encoding.ASCII.GetBytes(clientCode).Sum(b => b);
                else
                {
                    var lastChars = clientCode.Substring(handleType);
                    if (!int.TryParse(lastChars, out number))
                    {
                        number = -1;
                        _group0.Add(clientCode);
                    }
                }
                if (number != -1)
                    ClassifyClientCode(number, clientCode);
            }
        });
        
        ConsoleResult(_group0.Count, _group1.Count, _group2.Count, stopwatch.ElapsedMilliseconds);
    }

    private void ClassifyClientCode(int number, string clientCode)
    {
        var remainder = number % _instanceTotal;
        switch (remainder)
        {
            case 0:
                _group0.Add(clientCode);
                break;
            case 1:
                _group1.Add(clientCode);
                break;
            case 2:
                _group2.Add(clientCode);
                break;
        }
    }

    private void ClearGroup()
    {
        _group0.Clear();
        _group1.Clear();
        _group2.Clear();
    }

    private void ConsoleResult(int group0Count, int group1Count, int group2Count, long timeMs)
    {
        Console.WriteLine("Số lượng phần tử trong nhóm 0: {0}", group0Count);
        Console.WriteLine("Số lượng phần tử trong nhóm 1: {0}", group1Count);
        Console.WriteLine("Số lượng phần tử trong nhóm 2: {0}", group2Count);
        Console.WriteLine("Thời gian thực thi: {0} ms", timeMs);
    }

    public void AddString(int clientCodeNumber)
    {
        for (int i = 0; i < clientCodeNumber; i++)
        {
            _clientCodes.Add($"058C{i:D3}ABC");
        }
    }

    //
    private Stopwatch MeasureExecutionTime(Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        action.Invoke();
        stopwatch.Stop();
        return stopwatch;
    }
}

public class GenerateClientCode
{
    public HashSet<string> Generate()
    {
        // Tạo tập hợp các mã
        var codes = new HashSet<string>();
        for (int i = 0; i < 1000000; i++)
        {
            codes.Add($"058C{i:D6}"); // Định dạng chuỗi mã 6 chữ số
        }
        return codes;
    }
}


