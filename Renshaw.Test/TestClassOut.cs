using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renshaw.Commom;

namespace Renshaw.Test
{
    public class TestClassOut:TestClass5
    {
        public void Test()
        {
            
        }
    }

    class TestClass
    {
        private void Foo_Private()
        {
            
        }
        internal void Foo_Internal()
        {

        }
        protected void Foo_Protected()
        {
            
        }
        protected internal void Foo_ProtectedInternal()
        {
            
        }
        public void Foo_Public()
        {
            
        }

        class InterClass1
        {
            
        }
    }

    class TestClass1
    {
        public class TestClass2
        {

        }
        internal class TestClass3
        {
            
        }
        protected class TestClass4
        {
            
        }
        protected internal class TestClass5
        {
            
        }
        private class TestClass6
        {
            
        }
    }
}
