using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.SmartEnum;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace EnumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
        }
    }

    public enum OrderStatus
    {
        Open,
        Pending,
        Delivered,
        Cancelled,
        Completed
    }

    [MemoryDiagnoser]
    public class Benchy
    {
        [Benchmark]
        public string NativeToString() => OrderStatus.Completed.ToString();

        [Benchmark]
        public string FastToString() => FastToString(OrderStatus.Completed);

        [Benchmark]
        public string StringEnumToString() => OrderStatusType.Completed.ToString();

        [Benchmark]
        public string SmartEnum() => OrderStatusSmartEnum.Completed.ToString(); // output = "Idle"
        

        private static string FastToString(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Open => nameof(OrderStatus.Open),
                OrderStatus.Pending => nameof(OrderStatus.Pending),
                OrderStatus.Delivered => nameof(OrderStatus.Delivered),
                OrderStatus.Cancelled => nameof(OrderStatus.Cancelled),
                OrderStatus.Completed => nameof(OrderStatus.Completed),
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
        
    }
    
    public sealed class OrderStatusSmartEnum : SmartEnum<OrderStatusSmartEnum>
    {
        public static OrderStatusSmartEnum Open = new(nameof(Open), 1);
        public static OrderStatusSmartEnum Pending = new(nameof(Pending), 2);
        public static OrderStatusSmartEnum Delivered= new(nameof(Delivered), 3);
        public static OrderStatusSmartEnum Cancelled = new(nameof(Cancelled), 4);
        public static OrderStatusSmartEnum Completed = new(nameof(Completed), 5);

        private OrderStatusSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
    
    public  class OrderStatusType:Enumeration
    {
        protected OrderStatusType(int id, string name) : base(id, name)
        {

        }
        public static OrderStatusType Open = new(1, "Open");
        public static OrderStatusType Pending = new(2, "Pending");
        public static OrderStatusType Delivered= new(3, "Delivered");
        public static OrderStatusType Cancelled = new(4, "Cancelled");
        public static OrderStatusType Completed = new(5, "Completed");
    }
    
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}