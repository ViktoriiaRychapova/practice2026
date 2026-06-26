using System;

namespace Task04
{
    public interface ISpaceship
    {
        void MoveForward();        // Движение вперед
        void Rotate(int angle);    // Поворот на угол (градусы)
        void Fire();               // Выстрел ракетой
        int Speed { get; }         // Скорость корабля
        int FirePower { get; }     // Мощность выстрела
    }

    public class Fighter : ISpaceship
    {
        public int Speed => 100;
        public int FirePower => 20;

        public void MoveForward()
        {
            Console.WriteLine("Истребитель стремительно летит вперед!");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Истребитель резко развернулся на {angle} градусов.");
        }

        public void Fire()
        {
            Console.WriteLine($"Истребитель выпустил слабую ракету с мощностью {FirePower}.");
        }
    }

    public class Cruiser : ISpaceship
    {
        public int Speed => 50;
        public int FirePower => 100;

        public void MoveForward()
        {
            Console.WriteLine("Крейсер тяжело движется вперед...");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Крейсер медленно поворачивает на {angle} градусов.");
        }

        public void Fire()
        {
            Console.WriteLine($"Крейсер произвел залп мощными ракетами с мощностью {FirePower}!");
        }
    }
}