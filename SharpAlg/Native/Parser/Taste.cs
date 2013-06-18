using NUnit.Framework;
using System;
using System.IO;
using System.Text;namespace Taste {[TestFixture]public class Taste {
    const string program = @"
// This is a test program which can be compiled by the Taste-compiler.
// It reads a sequence of numbers and computes the sum of all integers 
// up to these numbers.

program Test {
	int i;
	
	void Foo() {
		int a, b, max;
		read a; read b;
        bool x;
		if (a > b) max = a; else max = b;
		write max;
	}

	void SumUp() {
		int sum;
		sum = 0;
		while (i > 0) { sum = sum + i; i = i - 1; }
		write sum;
	}

	void Main() {
		read i;
		while (i > 0) {
			SumUp();
			read i;
		}
	}
}";
    [Test]
    public void MainTest() {
        Scanner scanner = new Scanner(new MemoryStream(Encoding.ASCII.GetBytes(program)));
        Parser parser = new Parser(scanner);
        parser.tab = new SymbolTable(parser);
        parser.gen = new CodeGenerator();
        parser.Parse();
        Assert.AreEqual(0, parser.errors.count);
        parser.gen.Decode();
        string result = parser.gen.Interpret(new MemoryStream(Encoding.ASCII.GetBytes("13 5 120 0")));
        Assert.AreEqual("91 15 7260 ", result);
    }
	}} // end namespace