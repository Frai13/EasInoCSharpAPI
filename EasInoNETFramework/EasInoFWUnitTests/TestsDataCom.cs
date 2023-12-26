using EasInoAPI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasInoFWUnitTests
{
    public class TestsDataCom
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructor()
        {
            Helper.DataComConstructor("EINO::hello;world;::END", new List<string>() { "hello", "world" });
            Helper.DataComConstructor("EINO::hello;::END", new List<string>() { "hello" });
            Helper.DataComConstructor("EINO::hello;;world;::END", new List<string>() { "hello", "", "world" });
            Helper.DataComConstructor("EINO::::END", new List<string>() { });
            Helper.DataComConstructor("EINO::hello::END", new List<string>() { "hello" });

            Helper.DataComConstructor("EINO::hello;world;::EN", new List<string>() { });
            Helper.DataComConstructor("INO::hello;world;::END", new List<string>() { });
            Helper.DataComConstructor("EINO:hello;world;::END", new List<string>() { });
            Helper.DataComConstructor("EINO::hello;world;:END", new List<string>() { });
            Helper.DataComConstructor("hello;world;", new List<string>() { });

            Helper.DataComConstructor("beginEINO::hello;world;::ENDend", new List<string>() { "hello", "world" });
            Helper.DataComConstructor("beginEINO::hello;world;::END", new List<string>() { "hello", "world" });
            Helper.DataComConstructor("EINO::hello;world;::ENDend", new List<string>() { "hello", "world" });
            Helper.DataComConstructor(";EINO::hello;world;::END;", new List<string>() { "hello", "world" });
            Helper.DataComConstructor(";EINO::hello;world;::END", new List<string>() { "hello", "world" });
            Helper.DataComConstructor("EINO::hello;world;::END;", new List<string>() { "hello", "world" });
            Helper.DataComConstructor("EINO:: hello ; world ;::END", new List<string>() { " hello ", " world " });

            Helper.DataComConstructor("", new List<string>() { });
            Helper.DataComConstructor("", new List<string>() { });
        }

        [Test]
        public void TestToString()
        {
            Assert.That(new DataCom("EINO::hello;world;::END").ToString() == "EINO::hello;world;::END");
            Assert.That(new DataCom("EINO::hello;;::END").ToString() == "EINO::hello;;::END");

            Assert.That(new DataCom("").ToString() == "EINO::;;::END");
            Assert.That(new DataCom("INO::hello;world;::END").ToString() == "EINO::;;::END");
        }
    }
}
