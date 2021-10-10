using System;
using SCVE.Core.Entities;
using SCVE.Core.Services;

namespace SCVE.Null
{
    public class WindowManagerNull : WindowManager
    {
        public override ScveWindow Create(WindowProps props)
        {
            Console.WriteLine($"{nameof(WindowManagerNull)}: Creating window {props}");
            return null;
        }

        public override void OnInit()
        {
            Console.WriteLine("WindowManageNull: Init");
        }

        public override void PollEvents()
        {
            Console.WriteLine("WindowManageNull: PollEvents");
        }

        public override bool WindowShouldClose(ScveWindow window)
        {
            Console.WriteLine("WindowManageNull: WindowShouldClose");
            return false;
        }

        public override void Close(ScveWindow window)
        {
            Console.WriteLine("WindowManageNull: Close");
        }

        public override void OnTerminate()
        {
            Console.WriteLine("WindowManageNull: Terminate");

        }
    }
}