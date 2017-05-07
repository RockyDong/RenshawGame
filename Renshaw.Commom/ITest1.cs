using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    /*protected internal interface ITest1
    {
        void Foo();
    }

    protected internal enum Test10
    {
        
    }

    protected internal struct TestStruct
    {
        
    }

    protected class TestClass1
    {
        public virtual void Foo()
        {
            
        }
    }*/

    class TestClass2
    {
        
    }

    enum TestEnum1
    {
        
    }

    internal enum TestEnum3
    {
        
    }

   /* private enum TestEnum4
    {
        
    }*/

    public enum TestEnum2
    {
        
    }

    struct TestStruct
    {
        
    }

    internal struct TestStruct1
    {
        
    }

    public struct TestStruct2
    {
        
    }

    internal class TestClass3
    {
        private TestClass2 te;
        private TestClass5.TestClass8 t8;
        
    }

    /*private class TestClass4 //private 不能用作class的访问修饰符
    {
        
    }*/

    public class TestClass5
    {
        protected internal void Foo_p_i()
        {
            
        }

        protected void Foo_protected()
        {
            
        }

        internal void Foo_internal()
        {
            
        }

        private void Foo_private()
        {
            
        }

        public void Foo_public()
        {
            
        }

        private class TestClass6
        {
            private TestEnum1 e1;

            public void Func()
            {
                
            }
        }

        protected class TestClass7
        {
            
        }

        protected internal class TestClass8
        {
            
        }

        public class TestClass9
        {
            private TestClass10 te;
            private TestClass6 te1;
        }

        class TestClass10
        {
            
        }

        internal class TestClass11
        {
            
        }
    }
}
