namespace RomeCalc
{
    public interface IOperation
    {
        int DoOperation(int val1, int val2);
    }

    public class Sum : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 + val2;
    }

    public class Subtraction : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 - val2;
    }

    public class Division : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 / val2;
    }
    public class Multiplication : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 * val2;
    }    
    
    public class Remainder : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 % val2;
    }

    public class Less : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 < val2 ? 1: 0; 
    }
    public class LessEquals : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 <= val2 ? 1: 0; 
    }
    public class Bigger : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 > val2 ? 1: 0; 
    }
    public class BiggerEquals : IOperation
    {
        public int DoOperation(int val1, int val2) => val1 >= val2 ? 1: 0; 
    }
}
