using System;
using System.Collections.Generic;
using System.Text;

namespace WPFTest
{
    public class MatrixElement
    {
     
        private string _img;
        public MatrixElement(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

       

        public string Image
        {
            get { return _img; }
            set
            {
                _img = value;
               
            }
        }

     
    }
}
