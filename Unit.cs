using System;
namespace flowsharp
{
    public class Unit
    {
        public static Unit Instance { get; } = new Unit();
        private Unit()
        {
        }
    }
}
