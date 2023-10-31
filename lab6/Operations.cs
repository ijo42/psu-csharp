namespace RomeCalc
{
    public interface IOperation
    {
        int DoOp(int val1, int val2);
    }

    public class Sum : IOperation
    {
        public int DoOp(int val1, int val2) => val1 + val2;
    }

    public class Sub : IOperation
    {
        public int DoOp(int val1, int val2) => val1 - val2;
    }

    public class Div : IOperation
    {
        public int DoOp(int val1, int val2) => val1 / val2;
    }
    public class Mult : IOperation
    {
        public int DoOp(int val1, int val2) => val1 * val2;
    }    
    
    public class Remainder : IOperation
    {
        public int DoOp(int val1, int val2) => val1 % val2;
    }

    public class Less : IOperation
    {
        public int DoOp(int val1, int val2) => val1 < val2 ? 1: 0; 
    }
    public class LessEquals : IOperation
    {
        public int DoOp(int val1, int val2) => val1 <= val2 ? 1: 0; 
    }
    public class Bigger : IOperation
    {
        public int DoOp(int val1, int val2) => val1 > val2 ? 1: 0; 
    }
    public class BiggerEquals : IOperation
    {
        public int DoOp(int val1, int val2) => val1 >= val2 ? 1: 0; 
    }
}
