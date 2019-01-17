using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;

namespace QA.Validation.Xaml.Tests
{

    [TestClass]
    public class CodeDomTests
    {
        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom1()
        {
            Func<dynamic, bool> func = GenerateFunction("x.Name.Length == 4 && x.Name.Substring(1).Length != 0");

            dynamic obj = new ExpandoObject();

            obj.Name = "Test";
            var value = func(obj);

            Assert.IsTrue(value);
        }

        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom2()
        {
            Func<dynamic, bool> func = GenerateFunction("x.Name.Length == 4 && x.Name.Substring(1).Length != 0");

            dynamic obj = new ExpandoObject();

            obj.Name = "Te";
            var value = func(obj);

            Assert.IsFalse(value);
        }

        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom3()
        {
            Func<dynamic, bool> func = GenerateFunction("x.Name.Length == 3");

            dynamic obj = new ExpandoObject();

            obj.Name = "Te3";
            var value = func(obj);

            Assert.IsTrue(value);
        }


        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom3_Mixed_Lambdas()
        {
            dynamic obj = new ExpandoObject();

            obj.Name = "123";
            Assert.IsTrue(GenerateFunction("x.Name.Length == 3").Invoke(obj));

            obj.Name = "12345";
            Assert.IsTrue(GenerateFunction("x.Name.Length == 5").Invoke(obj));

            obj.Name = "1234545";
            Assert.IsFalse(GenerateFunction("x.Name.Length == 5").Invoke(obj));

            obj.Name = "23";
            Assert.IsTrue(GenerateFunction("x.Name.Length >= 2 && int.Parse(x.Name) < 98").Invoke(obj));
        }

        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom3_Check_Selection()
        {
            dynamic obj = new ExpandoObject();

            obj.Name = "123";

            var func = GenerateSelection("result.Name = x.Name + DateTime.Now + String.Empty; result.Flag=true;");
            var result = func.Invoke(obj);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Name);

            result = func.Invoke(result);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Name);

            func = GenerateSelection("result=x; result.Q=1;");

            var result1 = func(result);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result1.Name);
            Assert.IsNotNull(result1.Q);

            Assert.AreSame(result, result1);
        }


        [TestCategory("CSharpCodeProvider")]
        [TestMethod]
        public void CodeDom3_Check_Selection_Bench()
        {
            dynamic obj = new ExpandoObject();

            obj.Name = "123";

            var func = GenerateSelection("result.Name = x.Name + DateTime.Now + String.Empty; result.Flag=true;");
            var result = func.Invoke(obj);

            for (int i = 0; i < 300; i++)
            {
                result = func(result); 
            }
        }

        private Func<dynamic, bool> GenerateFunction(string lambda)
        {
            string format = @"
            using System;
            using System.Linq.Expressions;
            using System.Dynamic;

            class ExpressionContainer
            {
                public Func<dynamic, bool> TheExpression {get; set;}
                public string Length;

                public ExpressionContainer()
                {
                    TheExpression = x => <lambda>;
                }
            }
";
            string source = format.Replace("<lambda>", lambda);

            var result = GenerateInternal(source);

            return (Func<dynamic, bool>)result;
        }

        private Func<dynamic, dynamic> GenerateSelection(string lambda)
        {
            string format = @"
            using System;
            using System.Linq.Expressions;
            using System.Dynamic;

            class ExpressionContainer
            {
                public Func<dynamic, dynamic> TheExpression {get; set;}
                public string Length;

                public ExpressionContainer()
                {
                    TheExpression = x => {dynamic result = new ExpandoObject(); <lambda> return result; };
                }
            }
";
            string source = format.Replace("<lambda>", lambda);

            var result = GenerateInternal(source);

            return (Func<dynamic, dynamic>)result;
        }

        private static object GenerateInternal(string source)
        {
            System.Reflection.Assembly a;
            Dictionary<string, string> options = new Dictionary<string, string>{ 
            { "CompilerVersion", "v4.0" }};

            using (CSharpCodeProvider provider = new CSharpCodeProvider(options))
            {
                List<string> assemblies = new List<string>();

                foreach (Assembly x in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        assemblies.Add(x.Location);
                    }
                    catch (NotSupportedException)
                    {

                    }
                }

                CompilerResults r = provider.CompileAssemblyFromSource(new CompilerParameters(assemblies.ToArray())
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                }, source);

                if (r.Errors.HasErrors)
                {
                    throw new Exception("Errors compiling expression: " + 
                        string.Join(Environment.NewLine, r
                            .Errors
                            .OfType<CompilerError>()
                            .Select(e => e.ErrorText).ToArray()));
                }

                a = r.CompiledAssembly;
            }

            object o = a.CreateInstance("ExpressionContainer");

            if (o == null)
            {
                throw new InvalidOperationException("Cannot create instance of ExpressionContainer");
            }

            var propertyInfo = o.GetType().GetProperty("TheExpression");

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Cannot get function");
            }

            var result = propertyInfo.GetValue(o, null);
            o = null;
            return result;
        }
    }
}
