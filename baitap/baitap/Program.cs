using baitap;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("\n Nhập số lượng ClientCode có chứa kí tự abc ở 6 kí tự cuối: ");
        int clientCodeNumber = int.Parse(Console.ReadLine()); 

        IHandle handle = new Handle();
        handle.AddString(clientCodeNumber);
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n============================================Menu: ");
            Console.WriteLine("1. Cách lấy 6 số");
            Console.WriteLine("2. Cách lấy 4 số");
            Console.WriteLine("3. Cách chuyển ASCII");
            Console.WriteLine("0. Exit");

            Console.Write("\nNhập lựa chọn: ");
            string choice = Console.ReadLine();


            switch (choice)
            {
                case "1":
                    handle.HandleExecution(4);
                    break;
                case "2":
                    handle.HandleExecution(6);
                    break;
                case "3":
                    handle.HandleExecution(0);
                    break;
                case "0":
                    Console.WriteLine("Exiting...");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }
}