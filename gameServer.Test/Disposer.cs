using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace gameServer.Test
{
    [TestFixture(Description ="测试用例")]
    class Disposer:IDisposable
    {
        public Disposer()
        {
            Console.WriteLine("Disposer");
        }
        [Test]
        public void Dispose()
        {
            Console.WriteLine("Dispose haha");
        }
    }
}
