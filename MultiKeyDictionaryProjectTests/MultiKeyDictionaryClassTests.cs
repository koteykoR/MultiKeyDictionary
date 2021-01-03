using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using TestProject;

namespace Tests
{
    [TestClass()]
    public class MultiKeyDictionaryClassTests
    {
        [TestMethod()]
        public void GetByIdTest()
        {
            var doubleKeyDictionary = Initialize<int, int, int>(1);
            doubleKeyDictionary.Add((1, 2), 1);
            doubleKeyDictionary.Add((1, 3), 1);
            var expected = 2;
            var actual = doubleKeyDictionary.GetById(1).Count();
            Assert.AreEqual(expected, actual);

        }
        private MultiKeyDictionary<TKeyId, TKeyName, TValue> Initialize<TKeyId, TKeyName, TValue>(int count)
        {
            var doubleKeyDictionary = new MultiKeyDictionary<TKeyId, TKeyName, TValue>();
            var typeId = Nullable.GetUnderlyingType(typeof(TKeyId)) ?? typeof(TKeyId);
            var typeName = Nullable.GetUnderlyingType(typeof(TKeyName)) ?? typeof(TKeyName);
            var typeValue = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            for (var i = 0; i < count; i++)
            {
                doubleKeyDictionary.Add(((TKeyId)Convert.ChangeType(i, typeId, CultureInfo.InvariantCulture), (TKeyName)Convert.ChangeType(i, typeName, CultureInfo.InvariantCulture)), (TValue)Convert.ChangeType(i, typeValue, CultureInfo.InvariantCulture));
            }
            return doubleKeyDictionary;
        }

        [TestMethod()]
        public void GetByNameTest()
        {
            var doubleKeyDictionary = Initialize<int, string, int>(1);
            doubleKeyDictionary.Add((1, "Joe"), 1);
            doubleKeyDictionary.Add((2, "Joe"), 1);
            doubleKeyDictionary.Add((1, "Joe1"), 1);
            var expected = 2;
            var actual = doubleKeyDictionary.GetByName("Joe").Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddTest()
        {
            var doubleKeyDictionary = Initialize<int, int, int>(1);
            doubleKeyDictionary.Add((1, 1), 1);
            var expected = 2;
            var actual = doubleKeyDictionary.Count;
            Assert.AreEqual(expected, actual);

        }

        public void AddSameObjectsTest()
        {
            var count = 1;
            var doubleKeyDictionary = Initialize<int, string, double>(count);
            Assert.ThrowsException<ArgumentException>(() => doubleKeyDictionary.Add((0, "0"), 0.0));
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = Initialize<int, string, double>(count);
            for (var i = 1; i < 4; i++)
            {
                doubleKeyDictionary.Add((0, i.ToString()), 0.5);
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var count = 1;
            var doubleKeyDictionary = Initialize<int, string, double>(count);
            for (var i = 1; i < 4; i++)
            {
                doubleKeyDictionary.Add((i, "0"), 0.5);
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }




        [TestMethod()]
        public void ClearTest()
        {
            var count = 2;
            var doubleKeyDictionary = Initialize<int, string, string>(count);
            doubleKeyDictionary.Clear();

            var expected = 0;

            var actual = doubleKeyDictionary.Count();

            Assert.AreEqual(expected, actual);
        }



        [TestMethod()]
        public void ContainsTest()
        {
            var count = 5;
            var doubleKeyDictionary = Initialize<int, int, int>(count);

            foreach (var item in doubleKeyDictionary)
            {
                Assert.IsTrue(doubleKeyDictionary.Contains(item));
            }

        }

        [TestMethod()]
        public void AddIdNameTest()
        {
            var count = 5;
            var doubleKeyDictionary = Initialize<int, int, int>(count);
            var expected = count;
            var actual = doubleKeyDictionary.Keys.Count();
            Assert.AreEqual(expected, actual);


        }
        [TestMethod()]
        public void ContainsKeyTest()
        {
            var count = 2;
            var doubleKeyDictionary = Initialize<int, int, string>(count);
            doubleKeyDictionary.Add((1, 2), "Dima");
            Assert.AreEqual(doubleKeyDictionary.ContainsKey((1, 1)), true);
            Assert.IsFalse(doubleKeyDictionary.ContainsKey((1, 3)));
            Assert.AreEqual(doubleKeyDictionary.ContainsKey((1, 2)), true);

        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {

        }

        [TestMethod()]
        public void RemoveTest()
        {
            var count = 2;
            var doubleKeyDictionary = Initialize<int, int, int>(count);
            doubleKeyDictionary.Clear();

            var expected = 0;

            var actual = doubleKeyDictionary.Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveTest1()
        {
            var count = 3;
            var doubleKeyDictionary = Initialize<int, int, int>(count);
            doubleKeyDictionary.Remove((1, 1));
            var expected = 2;
            var actual = doubleKeyDictionary.Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TryGetValueTest()
        {
            var count = 3;
            var doubleKeyDictionary = Initialize<string, int, double>(count);

            var expected = new List<double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = doubleKeyDictionary.Values.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        public class UserType : IEquatable<UserType>
        {
            public string Value;
            public UserType(string value)
            {
                Value = value;
            }
            public override int GetHashCode() => Value.GetHashCode();
            public bool Equals(UserType other) => Value.Equals(other.Value);

            public override bool Equals(object obj)
            {
                var res = false;
                try
                {
                    res = Value.Equals(((UserType)obj).Value);
                }
                catch { res = false; }
                return res;
            }
        }
    }

    }


    