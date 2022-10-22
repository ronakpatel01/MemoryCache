using Finbourne.MemoryCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MemoryCacheTest
{
    class User
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
    }



    [TestClass]
    public class MemoryCacheTests
    {
        [TestMethod]
        public void TestAddToCache()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });

            Assert.AreEqual(1, mc.GetCount());

            User userFred = mc.Get("Fred") as User;
            Assert.AreEqual("Fred", userFred.FirstName);
            Assert.AreEqual("Smith", userFred.Surname);
            Assert.AreEqual(new DateTime(1995, 01, 15), userFred.DOB);
        }

        [TestMethod]
        public void TestAddExistingwithoutReplacing()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("Fred", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });

            Assert.AreEqual(1, mc.GetCount());

            User userFred = mc.Get("Fred") as User;
            Assert.AreEqual("Fred", userFred.FirstName);
            Assert.AreEqual("Smith", userFred.Surname);
            Assert.AreEqual(new DateTime(1995, 01, 15), userFred.DOB);
        }

        [TestMethod]
        public void TestAddExistingwithReplacing()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.AddOrReplace("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.AddOrReplace("Fred", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });

            Assert.AreEqual(1, mc.GetCount());

            User userFred = mc.Get("Fred") as User;
            Assert.AreEqual("James", userFred.FirstName);
            Assert.AreEqual("Layley", userFred.Surname);
            Assert.AreEqual(new DateTime(1990, 08, 03), userFred.DOB);
        }

        [TestMethod]
        public void TestMultipleAdds()
        {
            lock ("test4")
            {
                IMemoryCache mc = MemoryCache.Instance;
                mc.Clear();
                mc.UpdateCacheSize(5);

                mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
                mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
                mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

                Assert.AreEqual(3, mc.GetCount());

                User userFred = mc.Get("Fred") as User;
                Assert.AreEqual("Fred", userFred.FirstName);
                Assert.AreEqual("Smith", userFred.Surname);
                Assert.AreEqual(new DateTime(1995, 01, 15), userFred.DOB);

                User userJames = mc.Get("James") as User;
                Assert.AreEqual("James", userJames.FirstName);
                Assert.AreEqual("Layley", userJames.Surname);
                Assert.AreEqual(new DateTime(1990, 08, 03), userJames.DOB);

                User userMike = mc.Get("Mike") as User;
                Assert.AreEqual("Mike", userMike.FirstName);
                Assert.AreEqual("Bloggs", userMike.Surname);
                Assert.AreEqual(new DateTime(1963, 09, 17), userMike.DOB);
            }
        }

        [TestMethod]
        public void TestContains()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

            Assert.IsTrue(mc.Contains("Fred"));
            Assert.IsTrue(mc.Contains("James"));
            Assert.IsTrue(mc.Contains("Mike"));
            Assert.IsFalse(mc.Contains("Paul"));
        }

        [TestMethod]
        public void TestRemove()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

            mc.Remove("James");

            Assert.IsTrue(mc.Contains("Fred"));
            Assert.IsFalse(mc.Contains("James"));
            Assert.IsTrue(mc.Contains("Mike"));
        }

        [TestMethod]
        public void TestOverflow()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(2);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

            Assert.IsFalse(mc.Contains("Fred"));
            Assert.IsTrue(mc.Contains("James"));
            Assert.IsTrue(mc.Contains("Mike"));
        }

        [TestMethod]
        public void TestOverflowAfterRead()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(2);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            User userFred = mc.Get("Fred") as User;
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

            Assert.IsTrue(mc.Contains("Fred"));
            Assert.IsFalse(mc.Contains("James"));
            Assert.IsTrue(mc.Contains("Mike"));
        }

        [TestMethod]
        public void TestGetAll()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(5);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });

            Assert.IsTrue(mc.Contains("Fred"));
            Assert.IsTrue(mc.Contains("James"));
            Assert.IsTrue(mc.Contains("Mike"));

            foreach (var cachedObject in mc)
            {
                switch (cachedObject.Key)
                {
                    case "Fred":
                        User userFred = cachedObject.Value as User;
                        Assert.AreEqual("Fred", userFred.FirstName);
                        Assert.AreEqual("Smith", userFred.Surname);
                        Assert.AreEqual(new DateTime(1995, 01, 15), userFred.DOB);
                        break;
                    case "James":
                        User userJames = cachedObject.Value as User;
                        Assert.AreEqual("James", userJames.FirstName);
                        Assert.AreEqual("Layley", userJames.Surname);
                        Assert.AreEqual(new DateTime(1990, 08, 03), userJames.DOB);
                        break;
                    case "Mike":
                        User userMike = cachedObject.Value as User;
                        Assert.AreEqual("Mike", userMike.FirstName);
                        Assert.AreEqual("Bloggs", userMike.Surname);
                        Assert.AreEqual(new DateTime(1963, 09, 17), userMike.DOB);
                        break;
                    default:
                        Assert.Fail("User should not exist");
                        break;
                }
            }
        }

        [TestMethod]
        public void TestEvictedList()
        {
            IMemoryCache mc = MemoryCache.Instance;
            mc.Clear();
            mc.UpdateCacheSize(2);

            mc.Add("Fred", new User { FirstName = "Fred", Surname = "Smith", DOB = new DateTime(1995, 01, 15) });
            mc.Add("James", new User { FirstName = "James", Surname = "Layley", DOB = new DateTime(1990, 08, 03) });
            mc.Add("Mike", new User { FirstName = "Mike", Surname = "Bloggs", DOB = new DateTime(1963, 09, 17) });
            mc.Add("Rod", new User { FirstName = "Rod", Surname = "Peters", DOB = new DateTime(1983, 03, 25) });

            var evicted = mc.GetEvictedKeys();

            Assert.AreEqual(1, evicted.FindAll(t => t.Item1 == "Fred").Count);
            Assert.AreEqual(1, evicted.FindAll(t => t.Item1 == "James").Count);
            Assert.AreEqual(0, evicted.FindAll(t => t.Item1 == "Mike").Count);
            Assert.AreEqual(0, evicted.FindAll(t => t.Item1 == "Rod").Count);
        }
    }
}
