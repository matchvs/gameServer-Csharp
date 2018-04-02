using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace gameServer.Test
{
    [TestFixture]
    public class CancelTask
    {
        Dictionary<int, int> dic;
        CancellationTokenSource cancelToken;
        public CancelTask()
        {
            cancelToken = new CancellationTokenSource();
            dic = new Dictionary<int, int>();
        }

        [Test]
        public void TestCancelTask()
        {
            Task.Factory.StartNew(MyTask, cancelToken.Token);
            int count = 0;
            while(count < 3)
            {
                Assert.AreNotEqual(3, count, string.Format("sleeping count=%d", count));
                Thread.Sleep(1000);
                count++;
            }

            cancelToken.Cancel();
            Thread.Sleep(3000);
        }
        private void MyTask()
        {
            int count = 0;
            while (cancelToken.IsCancellationRequested == false)
            {
                Assert.AreNotEqual(5, count, string.Format("MyTask sleeping count=%d", count));
                count++;
                Thread.Sleep(1000);
            }
            Assert.AreNotEqual(5, count, string.Format("MyTask Over, count=%d", count));
        }
        [Test]
        public void TestDic()
        {
            dic.Add(0, 0);
        }
    }
}
