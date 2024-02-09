using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Buutyful.Coding_Tracker.Abstraction;

public interface IState
{
    ICommand GetCommand();
    void Render();
}
