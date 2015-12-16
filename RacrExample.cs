using System;


static class Accessors {
	public static double GetValue(this Racr.AstNode n) {
		return n.Child<double>("value");
	}
	public static Racr.AstNode GetExp(this Racr.AstNode n) {
		return n.Child("Exp");
	}
	public static Racr.AstNode GetA(this Racr.AstNode n) {
		return n.Child("A");
	}
	public static Racr.AstNode GetB(this Racr.AstNode n) {
		return n.Child("B");
	}
	public static double Eval(this Racr.AstNode n) {
		return n.AttValue<double>("Eval");
	}
	public static string Print(this Racr.AstNode n) {
		return n.AttValue<string>("Print");
	}
}

class RacrExample {
	public static void Main() {


		var spec = new Racr.Specification();

		spec.AstRule("Exp->");
		spec.AstRule("BinExp:Exp->Exp<A-Exp<B");
		spec.AstRule("AddExp:BinExp->");
		spec.AstRule("MulExp:BinExp->");
		spec.AstRule("Number:Exp->value");
		spec.CompileAstSpecifications("Exp");

		spec.RegisterAgRules(typeof(RacrExample));
		spec.CompileAgSpecifications();

		var root = spec.CreateAst("AddExp", spec.CreateAst("Number", 3.0), spec.CreateAst("Number", 4.0));

		Console.WriteLine("The expression '{0}' evaluates to {1}", root.Print(), root.Eval());
	}


	[Racr.AgRule("Eval", "Number", Cached = true, Context = "*")]
	private static double EvalNumber(Racr.AstNode node) {
		return node.GetValue();
	}

	[Racr.AgRule("Eval", "AddExp", Cached = true, Context = "*")]
	private static double EvalAddExp(Racr.AstNode node) {
		return node.GetA().Eval() + node.GetB().Eval();
	}

	[Racr.AgRule("Eval", "MulExp", Cached = true, Context = "*")]
	private static double EvalMulExp(Racr.AstNode node) {
		return node.GetA().Eval() * node.GetB().Eval();
	}

	[Racr.AgRule("Print", "MulExp", Cached = true, Context = "*")]
	private static string PrintMulExp(Racr.AstNode node) {
		return node.GetA().Print() + "*" + node.GetB().Print();
	}

	[Racr.AgRule("Print", "AddExp", Cached = true, Context = "*")]
	private static string PrintAddExp(Racr.AstNode node) {
		return node.GetA().Print() + "+" + node.GetB().Print();
	}

	[Racr.AgRule("Print", "Number", Cached = true, Context = "*")]
	private static string PrintNumber(Racr.AstNode node) {
		return node.GetValue().ToString();
	}
}

