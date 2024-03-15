using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baitap
{
    internal interface IHandle
    {
        void AddString(int clientCodeNumber);
        void HandleExecution(int handleType);
    }
}
